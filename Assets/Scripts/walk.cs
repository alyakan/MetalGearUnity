using UnityEngine;
using System.Collections;

public class walk : MonoBehaviour {
	private NavMeshAgent enemy;
	public Transform destination;
	public Transform destination2;
	private bool arrivedAt1 = false;
	// Use this for initialization
	void Start () {
		enemy = GetComponent<NavMeshAgent> ();
	}

	// Update is called once per frame
	void Update () {
		if (!arrivedAt1) {
			enemy.SetDestination (destination.position);
		} else {
			print ("Change direction");
			enemy.SetDestination (destination2.position);
		}
		if (enemy.transform.position.x == destination.position.x && enemy.transform.position.z == destination.position.z) {
			arrivedAt1 = true;
		}
		if (enemy.transform.position.x == destination2.position.x && enemy.transform.position.z == destination2.position.z) {
			arrivedAt1 = false;
		}
	}
}
