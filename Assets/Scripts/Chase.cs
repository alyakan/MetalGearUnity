using UnityEngine;
using System.Collections;

public class Chase : MonoBehaviour {
	public Transform player; 
	static Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (player.position, this.transform.position) < 10) {
			Vector3 direction = player.position - this.transform.position;
			direction.y = 0; 

			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, 
				Quaternion.LookRotation (direction), 0.1f);

			anim.SetBool ("IsIdle", false);
			if (direction.magnitude > 5) {
				this.transform.Translate (0, 0, 0.5f);
				anim.SetBool ("IsRunning", true);
				anim.SetBool ("IsAttacking", false);

			} else {
				anim.SetBool ("IsAttacking", true);
				anim.SetBool ("IsRunning", false);
			}
		} else {
			anim.SetBool ("IsIdle", true);
			anim.SetBool ("IsRunning", false);
			anim.SetBool ("IsAttacking", false);
		}
	
	}
}
