using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController GC;

	private UserInput player { get { return FindObjectOfType<UserInput> (); } set { player = value; } }

	private PlayerUI playerUI { get { return FindObjectOfType<PlayerUI> (); } set { playerUI = value; } }

	private WeaponHandler wp { get { return player.GetComponent<WeaponHandler> (); } set { wp = value; } }

	void Awake () 
	{
		if (GC == null) 
		{
			GC = this;
		} 
		else 
		{
			if (GC != this) 
			{
				Destroy (gameObject);
			}
		}
	}

	void Update ()
	{
		UpdateUI ();
	}

	void UpdateUI()
	{
		if (player) 
		{
			if (playerUI) 
			{
				if (wp) 
				{
					if (playerUI.ammoText) {
						if (wp.currentWeapon == null) {
							playerUI.ammoText.text = "Unarmed.";
						} else {
							playerUI.ammoText.text = wp.currentWeapon.ammo.clipAmmo + "//" + wp.currentWeapon.ammo.carryingAmmo; 
						}
					}
				}

				if (playerUI.healthBar && playerUI.healthText) 
				{
					playerUI.healthText.text = Mathf.Round(playerUI.healthBar.value).ToString();
				}
			}
		}
	}
}
