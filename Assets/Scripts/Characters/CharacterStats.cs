using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {

	[Range(0, 100)] public float health = 100;
	public int faction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		health = Mathf.Clamp (health, 0, 100);
	}
}
