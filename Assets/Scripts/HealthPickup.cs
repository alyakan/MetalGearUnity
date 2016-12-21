using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag (Tags.player);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up * 2, Space.World);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject == player) {
			print ("Pickup health");
			player.GetComponent<PlayerInventory> ().items.Add (Tags.healthPickup);
			print (player.GetComponent<PlayerInventory> ().items[0]);
			gameObject.SetActive (false);
		}
	}
}
