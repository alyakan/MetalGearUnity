using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartMenuScript : MonoBehaviour {

	public Button startGameBtn;
	public Button howToPlayBtn;
	public Button creditsBtn;
	public Button quitGameBtn;
	public Button doneInstructions;
	public Button doneCredits;
	public Toggle mute;

	public Animator instructionsAnim;
	public Animator creditsAnim;

	// Use this for initialization
	void Start () {
		startGameBtn.onClick.AddListener(() => StartGame());
		howToPlayBtn.onClick.AddListener(() => HowToPlay());
		creditsBtn.onClick.AddListener(() => Credits());
		quitGameBtn.onClick.AddListener(() => QuitGame());
		doneInstructions.onClick.AddListener(() => DismissInstructions());
		doneCredits.onClick.AddListener(() => DismissCredits());
		// mute.onValueChanged.AddListener (Mute);
	}

	// Update is called once per frame
	void Update () {
		
	}

	void StartGame()
	{
		// PlayButtonClickSound ();
		// StartCoroutine (WaitForSound (transform.GetChild (0).transform.GetChild(1).GetComponent<AudioSource> ().clip.length));
		StartCoroutine (WaitForSound (0));
	}

	IEnumerator WaitForSound(float time)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene ("Scene");
	}

	void HowToPlay() 
	{
		PlayButtonClickSound ();
		instructionsAnim.SetTrigger ("ViewInstructions");
	}

	void Credits()
	{
		PlayButtonClickSound ();
		creditsAnim.SetTrigger ("ViewCredits");
	}

	void QuitGame()
	{
		Application.Quit ();
		
	}

	void DismissInstructions()
	{
		PlayButtonClickSound ();
		instructionsAnim.SetTrigger ("DismissInstructions");
	}

	void DismissCredits()
	{
		PlayButtonClickSound ();
		creditsAnim.SetTrigger ("DismissCredits");
	}

	void Mute(bool value)
	{
		if (value) {
			AudioListener.pause = true;
		} else if (!value && AudioListener.pause ) {
			AudioListener.pause = false;
		}
	}

	void PlayButtonClickSound()
	{
		transform.GetChild (0).transform.GetChild(1).GetComponent<AudioSource> ().Play ();
	}
}
