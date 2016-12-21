using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {
	private GameObject player;
	private PlayerInventory playerInventory;

	private GameObject door;
	private Light doorSpotLight;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag(Tags.player);
		door =  GameObject.FindGameObjectWithTag(Tags.door);
		doorSpotLight =  GameObject.FindGameObjectWithTag(Tags.doorSpotLight).GetComponent<Light>();
		playerInventory = player.GetComponent<PlayerInventory> ();

	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player) {
			print ("######### keyPickedUp #########");
			playerInventory.hasKey = true;
			// pick up key
			Destroy (gameObject);
			// open door
			doorSpotLight.color = Color.green;
//			Destroy (door);
			
		}
	
	}
}
