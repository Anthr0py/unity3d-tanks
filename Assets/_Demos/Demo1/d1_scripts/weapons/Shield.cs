using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : Weapon {

	// (float)power = strength of the shield
	// (float)fireRate = duration
	// (int)ammo = cooldown
	// (gameObject)projectile = particle effect
	int baseCooldown;

	void Start()
	{
		base.Start();
		baseCooldown = ammo;
	}

	public override void Shot()
	{
		rot = barrelTip.eulerAngles;
		if(ammo == 0)
			return;

		if(canShot)
		{
			ammo = 0;
			projectile.SetActive(true);
			Invoke("ShotReset", fireRate);	// Duration
		}
		else
		{
			return;
		}
		StartCoroutine(weaponManager.SetWeaponCooldown((int)weaponClickType, baseCooldown));
	}

	void ShotReset()	// Duration
	{
		canShot = true;
		projectile.SetActive(false);
		Invoke("ResetCooldown", baseCooldown);
	}

	void ResetCooldown()
	{
		ammo = baseCooldown;
	}
}