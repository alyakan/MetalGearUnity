using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{ 
	public float deadZone = 5f;
//	public Transform destination;
//	public Transform destination2;
//	private bool arrivedAt1 = false;

	private Transform player;
	private EnemySight enemySight;
	private NavMeshAgent nav;
	private Animator anim;
	private HashIDs hash;
	private AnimatorSetup animSetup;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		enemySight = GetComponent<EnemySight> ();
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();

		nav.updatePosition = false;
		animSetup = new AnimatorSetup (anim, hash);

		deadZone *= Mathf.Deg2Rad; 
	}

	void Update()
	{
		NavAnimSetup ();
//		if (!arrivedAt1) {
//			nav.SetDestination (destination.position);
//		} else {
//			print ("Change direction");
//			nav.SetDestination (destination2.position);
//		}
//		if (transform.position.x == destination.position.x && transform.position.z == destination.position.z) {
//			arrivedAt1 = true;
//		}
//		if (transform.position.x == destination2.position.x && transform.position.z == destination2.position.z) {
//			arrivedAt1 = false;
//		}
	}

	void OnAnimatorMove()
	{
		nav.velocity = anim.deltaPosition / Time.deltaTime;
		transform.rotation = anim.rootRotation;
	}

	void NavAnimSetup()
	{
		float speed;
		float angle;

		if (enemySight.playerInSight) {
			speed = 0f;

			angle = FindAngle (transform.forward, player.position - transform.position, transform.up);
		} else
		{
			speed = Vector3.Project (nav.desiredVelocity, transform.forward).magnitude;

			angle = FindAngle (transform.forward, nav.desiredVelocity, transform.up);

			if (Mathf.Abs(angle) < deadZone)
			{
				transform.LookAt (transform.position + nav.desiredVelocity);
				angle = 0f;
			}
		}

		animSetup.Setup (speed, angle);
	}

	/*
		Returns the angle to which the enemy needs to rotate to face the player.
	*/
	float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
	{
		if (toVector == Vector3.zero)
			return 0f;

		float angle = Vector3.Angle (fromVector, toVector);
		Vector3 normal = Vector3.Cross (fromVector, toVector);
		angle *= Mathf.Sign (Vector3.Dot(normal, upVector));
		angle *= Mathf.Deg2Rad;

		return angle;
	}
}
