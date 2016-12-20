using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {

	public int locomotionState;
	public int shoutState;
	public int speedFloat;
	public int sneakingBool;
	public int shoutingBool;
	public int angularSpeedFloat;
	public int firingBool;

	void Awake() {
		locomotionState = Animator.StringToHash ("Base Layer.Locomotion");
		shoutState = Animator.StringToHash ("Shouting.yelling_out");
		speedFloat = Animator.StringToHash ("Speed");
		sneakingBool = Animator.StringToHash ("Sneaking");
		shoutingBool = Animator.StringToHash ("Shouting");
		angularSpeedFloat = Animator.StringToHash ("AngularSpeed");
		firingBool = Animator.StringToHash ("Firing");
	}
}
