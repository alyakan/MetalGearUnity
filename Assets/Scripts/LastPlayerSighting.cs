using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour {
	public Vector3 position = new Vector3 (1000f, 1000f, 1000f);
	public Vector3 resetPosition = new Vector3 (1000f, 1000f, 1000f);
	public float fadeSpeed = 7f;
	public float musicFadeSpeed = 1f;

	private AudioSource panicAudio;

	void Awake()
	{
		panicAudio = transform.Find ("secondaryMusic").GetComponent<AudioSource>();
	}

	void Update() {
		MusicFading ();
	}

	void MusicFading()
	{
		if (position != resetPosition) {
			GetComponent<AudioSource> ().volume = Mathf.Lerp (GetComponent<AudioSource> ().volume, 0f, musicFadeSpeed * Time.deltaTime);
			panicAudio.volume = Mathf.Lerp (panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
		} else {
			GetComponent<AudioSource> ().volume = Mathf.Lerp (GetComponent<AudioSource>  ().volume, 0.8f, musicFadeSpeed * Time.deltaTime);
			panicAudio.volume = Mathf.Lerp (panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
		}
	}
}
