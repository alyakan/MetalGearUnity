using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {
	private GameObject player;
	private PlayerInventory playerInventory;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerInventory = player.GetComponent<PlayerInventory> ();

	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player) {
			print ("######### keyPickedUp #########");
			playerInventory.hasKey = true;
			Destroy (gameObject);
			
		}
	
	}
}
