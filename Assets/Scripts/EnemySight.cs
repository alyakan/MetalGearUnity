using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
	public float fieldOfViewAngle = 110f;
	public bool playerInSight = false;
	public Vector3 personalLastSighting;

	private NavMeshAgent nav;
	private SphereCollider col;
	private Animator anim;
	private GameObject player;
	private Animator playerAnim;
	private Vector3 previousSighting;
	private LastPlayerSighting lastPlayerSighting;
	private HashIDs hash;

	// Use this for initialization
	void Awake () {
		nav = GetComponent<NavMeshAgent> ();
		col = GetComponent<SphereCollider> ();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag (Tags.player);
		playerAnim = player.GetComponent<Animator> ();
		lastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();

		personalLastSighting = lastPlayerSighting.resetPosition;
		previousSighting = lastPlayerSighting.resetPosition;



	}
	
	// Update is called once per frame
	void Update () {
		if (lastPlayerSighting.position.x != previousSighting.x && lastPlayerSighting.position.z != previousSighting.z)
			personalLastSighting = lastPlayerSighting.position;
		
		previousSighting = lastPlayerSighting.position;
		// Debug raycast
		//Vector3 forward = player.transform.position - transform.position;
		//Debug.DrawRay(transform.position + transform.up * 0.5f , forward.normalized ,  Color.cyan);
	
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject == player) {
			playerInSight = false;
			// if player in field of vision of enemy 
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle (direction, transform.forward);

			if (angle < fieldOfViewAngle ) {
				// check if no obstacle between enemy and player, raycast is moved up so it won't collide with floor 
				RaycastHit hit;
				if (Physics.Raycast (transform.position + transform.up * 0.5f  , direction.normalized, out hit, col.radius)) {
					if (hit.collider.gameObject == player) {
						print ("######### playerInSight #########");
						//playerInSight = true;
						//lastPlayerSighting.position = player.transform.position;
					}
				}

			}

			// if player is heard, check distance to player position
			int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
			int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;

			if(playerLayerOneStateHash == hash.shoutState ){
				print ("######### Sound made #########");
				if (calculatePathLength (player.transform.position) <= col.radius) {
					personalLastSighting = player.transform.position;
				}
			}
		}
			
	}
		
	// calculate path to player 
	float calculatePathLength(Vector3 targetPosition){
		NavMeshPath path = new NavMeshPath ();
		if (nav.enabled)
			nav.CalculatePath (targetPosition, path);

		Vector3[] allwaypoints = new Vector3[path.corners.Length + 2];

		allwaypoints [0] = transform.position;
		allwaypoints [allwaypoints.Length - 1] = targetPosition;

		// get different points of path for late calculation of distance 
		for (int i = 0; i < path.corners.Length; i++) {
			allwaypoints [i + 1] = path.corners [i];
		}

		float pathLength = 0f;

		// calculate distance between every corner, then increment 
		for (int i = 0; i< allwaypoints.Length - 1; i++) {
			pathLength += Vector3.Distance (allwaypoints [i], allwaypoints [i + 1]);
		}

		return pathLength;

	}
}
