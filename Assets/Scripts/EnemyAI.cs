using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public float patrolSpeed = 2f;
	public float chasingSpeed = 5f;
	// time to wait before action
	public float patrolWaitTime = 1f;
	public float chaseWaitTime = 5f;
	// patrol check points for each enemy
	public Transform[]patrolWayPoints;


	private EnemySight enemySight;
	private NavMeshAgent nav;
	private Transform player;
	private LastPlayerSighting lastPlayerSighting;
	// timers
	private float patrolTimer;
	private float chaseTimer;
	// enemry current check point
	private int wayPointIndex;

	void Awake(){
		enemySight = GetComponent<EnemySight> ();
		nav = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		lastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ();
	}

	void Update(){
		if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition)
			Chasing ();
		else
			Patrolling ();
		
	}

	void Chasing(){
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		// if distance between enemy and player > margin, chase
		if (sightingDeltaPos.sqrMagnitude > 4f)
			nav.destination = enemySight.personalLastSighting;
		nav.speed = chasingSpeed;
		// if enemy reached its destination
		if (nav.remainingDistance < nav.stoppingDistance) {
			chaseTimer += Time.deltaTime;
			// if reached destination and didn't find player, start patrolling again 
			if (chaseTimer > chaseWaitTime) {
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
			}
		} else
			chaseTimer = 0f;
			

	}

	void Patrolling(){
		nav.speed = patrolSpeed;
		// check if enemy has no position, or reached one of the position
		if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance) {
			patrolTimer += (Time.deltaTime * 2);

			if (patrolTimer >= patrolWaitTime) {
				if (wayPointIndex == patrolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;
				patrolTimer = 0f;
			}
					
		} else
			patrolTimer = 0f;
		nav.updatePosition = true;
		nav.SetDestination(patrolWayPoints [wayPointIndex].position);
	}
}
