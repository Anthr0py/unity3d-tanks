using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salva : Weapon {

	public GameObject indicator;

	// Not used for now	
	float minRange, maxRange;

	public int salvaCount;

	public bool resetFov;
	Vector3 indicatorPos;

	void Update()
	{
		if(indicator.activeInHierarchy)
		{
			resetFov = false;
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 80, 0.5f);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100))
          	{
				// Min-Max distance prototype
				//float newDistance = (Vector3.Distance(transform.position, hit.point));
				//if(newDistance >= minRange && newDistance <= maxRange)
				//{
					indicatorPos = hit.point;
					indicator.transform.up = hit.normal;
					indicator.transform.position = indicatorPos;
				//}
		  	}

			if(Input.GetButtonUp("Fire2"))
			{
				ShotUp(indicatorPos);
				Invoke("ResetCameraFov", 1.5f);
			}
		}

		if(resetFov)
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, 0.5f);
	}

	void ShotUp(Vector3 iPos)
	{
		indicator.SetActive(false);

		rot = barrelTip.eulerAngles;
		if(ammo == 0)
			return;

		if(canShot)
		{
			canShot = false;
			StartCoroutine(CreateSalva(iPos));
			StartCoroutine(weaponManager.SetWeaponCooldown((int)weaponClickType, fireRate));
			Invoke("ShotReset", fireRate);
			weaponManager.DisableWeapons(-1, false);
		}
		else
		{
			return;
		}
	}

	IEnumerator CreateSalva(Vector3 newPos)
	{
		int count;
		if(ammo >= salvaCount)
			count = salvaCount;
		else
			count = ammo;

		for(int i = 0; i < count; i++)
		{
			ammo--;
			Vector3 randomTargetPos = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
			newPos += randomTargetPos;
			GameObject newProjectile = (GameObject)Instantiate(projectile, barrelTip.position, Quaternion.Euler(rot));
			Projectile projectileSrc = newProjectile.GetComponent<Projectile>();
			projectileSrc.SetTarget(newPos);
			muzzleParticle.Emit(10);
			weaponManager.UpdateAmmoUI((int)weaponClickType, ammo);
			yield return StartCoroutine(DelaySalva(0.2F));
		}
	}

	IEnumerator DelaySalva(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}

	void ResetCameraFov()
	{
		resetFov = true;
	}

	public override void Shot()
	{
		DisplayIndicator();
		weaponManager.DisableWeapons(1, true);
	}

	void DisplayIndicator()
	{
		indicator.SetActive(true);
	}
}