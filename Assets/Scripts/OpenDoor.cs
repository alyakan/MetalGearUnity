using UnityEngine;
using System.Collections;

//Make an empty game object and call it "Door"
//Rename your 3D door model to "Body"
//Parent a "Body" object to "Door"
//Make sure thet a "Door" object is in left down corner of "Body" object. The place where a Door Hinge need be
//Add a box collider to "Door" object and make it much bigger then the "Body" model, mark it trigger
//Assign this script to a "Door" game object that have box collider with trigger enabled
//Press "f" to open the door and "g" to close the door
//Make sure the main character is tagged "player"

public class OpenDoor : MonoBehaviour {
	private float smooth = 2.0f;
	private float doorOpenAngle = 90.0f;
	private bool open = false;
	private bool enter = false;
	private bool cannotEnter = false;

	private Vector3 defaultRot;
	private Vector3 openRot;

	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag (Tags.player);
		defaultRot = transform.eulerAngles;
		openRot = new Vector3 (defaultRot.x, defaultRot.y + doorOpenAngle, defaultRot.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {
			// Open Door
			transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, openRot, Time.deltaTime * smooth);
		} else {
			// Close Door
			transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
		}

		if (Input.GetKeyDown ("f") && enter) {
			print ("OPEN");
			open = !open;
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(Screen.width/2 - 75, Screen.height - 100, 100, 20), "Hello World!");
		if (enter) {
			GUI.Label (new Rect (Screen.width/2 - 75, Screen.height - 100, 150, 30), "Press F to open the door");
		} 
		if (cannotEnter) {
			print ("ACQUIRE KEY FIRST");
			GUI.Label (new Rect (Screen.width/2 - 75, Screen.height - 100, 150, 30), "Please acquire Door Key first");
		}
	}

	//Activate the Main function when player is near the door
	void OnTriggerEnter (Collider other){
		bool hasKey = player.GetComponent<PlayerInventory> ().hasKey;
		if (other.gameObject == player && hasKey) {
			enter = true;
		} else {
			if (other.gameObject == player && !hasKey) {
				cannotEnter = true;
			}
		}
	}

	void OnTriggerExit (Collider other){
		if (other.gameObject == player) {
			enter = false;
			cannotEnter = false;
		}
	}
}
