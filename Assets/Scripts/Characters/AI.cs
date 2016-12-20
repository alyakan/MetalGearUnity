using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Animator))]
public class AI : MonoBehaviour {

	private NavMeshAgent navmesh;
	private CharacterMovement characterMove { get { return GetComponent<CharacterMovement> (); } set { characterMove = value; } }
	private Animator animator { get { return GetComponent<Animator> (); } set { animator = value; } }
	private CharacterStats characterStats { get { return GetComponent<CharacterStats> (); } set { characterStats = value; } }

	public enum AIState { Patrol, Attack, FindCover }
	public AIState aiState;

	[System.Serializable]
	public class PatrolSettings
	{
		public WaypointBase[] waypoints;
	}
	public PatrolSettings patrolSettings;

	[System.Serializable]
	public class SightSettings
	{
		public LayerMask sightLayers;
		public float sightRange = 30f;
		public float fieldOfView = 120f;
		public float eyeheight;
	}
	public SightSettings sight;

	private float currentWaitTime;
	private int waypointIndex;
	private Transform currentLookTransform;
	private bool walkingToDest;

	private float forward;

	private Transform target;
	private Vector3 targetLastKnownPosition;
	private CharacterStats[] allCharacters;

	// Use this for initialization
	void Start () {
		navmesh = GetComponentInChildren<NavMeshAgent> ();

		if (navmesh == null) {
			Debug.LogError ("We need a navmesh to traverse the world with.");
			enabled = false;
			return;
		}

		if (navmesh.transform == this.transform) {
			Debug.LogError ("The navmesh agent should be a child of the character: " + gameObject.name);
			enabled = false;
			return;
		}

		navmesh.speed = 0;
		navmesh.acceleration = 0;
		navmesh.autoBraking = false;

		if (navmesh.stoppingDistance == 0) {
			Debug.Log ("Auto settings stopping distance to 1.3f");
			navmesh.stoppingDistance = 1.3f;
		}

		GetAllCharacters ();
	}

	void GetAllCharacters () {
		allCharacters = GameObject.FindObjectsOfType<CharacterStats> ();
	}
	
	// Update is called once per frame
	void Update () {

		//TODO: Animate the strafe when the enemy is trying to shoot us.
		characterMove.Animate (forward, 0);
		navmesh.transform.position = transform.position;

		LookForTarget ();

		switch (aiState) {
		case AIState.Patrol:
			Patrol ();
			break;
		}
	}

	void LookForTarget () {
		if (allCharacters.Length > 0) {
			foreach (CharacterStats c in allCharacters) {
				if (c != characterStats && c.faction != characterStats.faction && c == ClosestEnemy()) {
					RaycastHit hit;
					Vector3 start = transform.position + (transform.up * sight.eyeheight);
					Vector3 dir = (c.transform.position + c.transform.up) - start;
					float sightAngle = Vector3.Angle (dir, transform.forward);
					if (Physics.Raycast (start, dir, out hit, sight.sightRange, sight.sightLayers) &&
						sightAngle < sight.fieldOfView && hit.collider.GetComponent<CharacterStats>()) {
						target = hit.transform;
						targetLastKnownPosition = Vector3.zero;
					} else {
						if (target != null) {
							targetLastKnownPosition = target.position;
							target = null;
						}
					}
				}
			}
		}
	}

	CharacterStats ClosestEnemy () {
		CharacterStats closestCharacter = null;
		float minDistance = Mathf.Infinity;
		foreach (CharacterStats c in allCharacters) {
			if (c != characterStats && c.faction != characterStats.faction) {
				float distToCharacter = Vector3.Distance (c.transform.position, transform.position);
				if (distToCharacter < minDistance) {
					closestCharacter = c;
					minDistance = distToCharacter;
				}
			}
		}

		return closestCharacter;
	}

	void Patrol () {
		if (!navmesh.isOnNavMesh) {
			Debug.Log ("We're off the navmesh");
			return;
		}

		if (patrolSettings.waypoints.Length == 0) {
			return;
		}

		navmesh.SetDestination (patrolSettings.waypoints [waypointIndex].destination.position);
		LookAtPosition (navmesh.steeringTarget);
		if (navmesh.remainingDistance <= navmesh.stoppingDistance) {
			walkingToDest = false;
			forward = LerpSpeed (forward, 0, 15);
			currentWaitTime -= Time.deltaTime;

			if (patrolSettings.waypoints [waypointIndex].lookAtTarget != null)
				currentLookTransform = patrolSettings.waypoints [waypointIndex].lookAtTarget;
			if (currentWaitTime <= 0) {
				waypointIndex = (waypointIndex + 1) % patrolSettings.waypoints.Length;
			}

		} else {
			walkingToDest = true;
			forward = LerpSpeed (forward, 0.5f, 15);
			currentWaitTime = patrolSettings.waypoints [waypointIndex].waitTime;
			currentLookTransform = null;
		}
	}

	float LerpSpeed (float curSpeed, float destSpeed, float time) {
		curSpeed = Mathf.Lerp (curSpeed, destSpeed, Time.deltaTime * time);
		return curSpeed;
	}

	void LookAtPosition (Vector3 pos) {
		Vector3 dir = pos - transform.position;
		Quaternion lookRot = Quaternion.LookRotation (dir);
		lookRot.x = 0;
		lookRot.z = 0;
		transform.rotation = Quaternion.Lerp (transform.rotation, lookRot, Time.deltaTime * 5);
	}

	void OnAnimatorIK () {
		if (currentLookTransform != null && !walkingToDest) {
			animator.SetLookAtPosition (currentLookTransform.position);
			animator.SetLookAtWeight (1, 0, 0.5f, 0.7f);
		} else {
			animator.SetLookAtWeight (0);
		}
	}
}

[System.Serializable]
public class WaypointBase 
{
	public Transform destination;
	public float waitTime;
	public Transform lookAtTarget;
}
