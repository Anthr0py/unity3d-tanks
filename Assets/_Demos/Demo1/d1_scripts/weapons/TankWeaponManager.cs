using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankWeaponManager : MonoBehaviour {

	public Weapon LeftClickWeapon;
	public Weapon RightClickWeapon;
	public Weapon SpaceWeapon;

	public Weapon[] weaponArray;

	public Text[] UIAmmoText;
	public Image[] UIAbilityIcon;

	void Start()
	{
		weaponArray = new Weapon[3];
		weaponArray[0] = LeftClickWeapon;
		weaponArray[1] = RightClickWeapon;
		weaponArray[2] = SpaceWeapon;
	}

	void Update()
	{
		if(Input.GetButton("Fire1") && LeftClickWeapon != null && !LeftClickWeapon.isDisabled)
			LeftClickWeapon.Shot();
		
		if(Input.GetButtonDown("Fire2") && RightClickWeapon != null && !RightClickWeapon.isDisabled)
			RightClickWeapon.Shot();

		if(Input.GetButton("Jump") && SpaceWeapon != null && !SpaceWeapon.isDisabled)
			SpaceWeapon.Shot();
	}

	public void UpdateAmmoUI(int uiID, int value)
	{
		UIAmmoText[uiID].text = value.ToString();
	}

	public void SetWeapon(Weapon wpn, int clickID)
	{
		switch(clickID)
		{
			case 0:
				LeftClickWeapon = wpn;
			break;
			case 1:
				RightClickWeapon = wpn;
			break;
			case 2:
				SpaceWeapon = wpn;
			break;
		}
	}

	public IEnumerator SetWeaponCooldown(int uiID, float duration)
	{
		UIAbilityIcon[uiID].fillAmount = 0;

		float startTime = 0f;
		while (startTime <= duration)
		{
			startTime = startTime + Time.deltaTime;
			float percent = Mathf.Clamp01(startTime / duration);
			UIAbilityIcon[uiID].fillAmount = Mathf.Lerp(UIAbilityIcon[uiID].fillAmount, 1.2f, percent);
			yield return null;
		}
	}

	public void DisableWeapons(int ignoreWeaponID, bool disable)
	{
		// TODO: Fix this madness chaos
		if(ignoreWeaponID == -1)
		{
			for(int i = 0; i < 2; i++)
			{
				weaponArray[i].isDisabled = disable;
			}
		}
		else
		{
			for(int i = 0; i < 2; i++)
			{
				if(i != ignoreWeaponID)
				{
					weaponArray[i].isDisabled = disable;
				}
				else
				{
					weaponArray[i].isDisabled = !disable;
				}
			}
		}
	}
}