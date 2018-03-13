using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public enum WeaponClickType {
		Left,
		Right,
		Space
	}

	public WeaponClickType weaponClickType;

	[HideInInspector]
	public bool isDisabled = false;

	public float power, fireRate;
	public int ammo;
	public GameObject projectile;
	public ParticleSystem muzzleParticle;
	public Transform barrelTip;
	[HideInInspector]
	public bool canShot;
	[HideInInspector]
	public Vector3 rot;

	[HideInInspector]
	public TankWeaponManager weaponManager;

	void Awake()
	{
		weaponManager = FindObjectOfType<TankWeaponManager>();
	}

	public void Start()
	{
		weaponManager.UpdateAmmoUI((int)weaponClickType, ammo);
		canShot = true;
	}

	public virtual void Shot()
	{
		
		rot = barrelTip.eulerAngles;
		if(ammo == 0)
			return;

		if(canShot)
		{
			canShot = false;
			weaponManager.UpdateAmmoUI((int)weaponClickType, ammo);
			if(muzzleParticle != null)
				muzzleParticle.Emit(10);
			SetAmmo(-1);
			StartCoroutine(weaponManager.SetWeaponCooldown((int)weaponClickType, fireRate));
			Invoke("ShotReset", fireRate);
			GameObject newProjectile = (GameObject)Instantiate(projectile, barrelTip.position, Quaternion.Euler(rot));
			newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * power, ForceMode.Impulse);
		}
		else
		{
			return;
		}
	}
	void ShotReset()
	{
		canShot = true;
	}

	public void SetAmmo(int newValue)
	{
		ammo += newValue;
	}
}