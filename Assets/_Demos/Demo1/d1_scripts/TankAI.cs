using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour {

	Vector3 newPoint;
	Transform tank, target;
	float shotDistance;
	bool canShot;
	public NavMeshAgent agent;
	public float minShotDistance, fireRate, power, rotSpeed, health;
	public Transform top, barrelTip;
	public GameObject projectile, explosionDebris, muzzleParticle, disabledParticle;
	public Vector3[] spawnPoints;

	float maxHealth;
	bool isDisabled;

	UIMessageHandler ui;

	void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		ui = FindObjectOfType<UIMessageHandler>();
	}

	void Start()
	{
		maxHealth = health;
		tank = transform;
		newPoint = GetNewPoint();
		canShot = true;
	}

	void DisableTank()
	{
		isDisabled = true;
		disabledParticle.SetActive(true);
		ui.SetMessage("Tank disabled", 1.0f);
	}

	public void DestroyTank()
	{
		health = 0;

		CreateExplosion();
		ui.SetMessage("Tank destroyed", 1.0f);

		int spawnIndex = Random.Range(0, spawnPoints.Length);
		transform.position = spawnPoints[spawnIndex];

		newPoint = Vector3.zero;
		newPoint = GetNewPoint();

		DoDamage(-maxHealth);
		isDisabled = false;
		GetComponent<Animation>().Play();
	}

	public void DoDamage(float damage)
	{
		health -= damage;
		if(health <= 25 && health > 0 && !isDisabled)
		{
			DisableTank();
		}
		else if(health <= 0)
		{
			DestroyTank();
		}
	}


	void Update()
	{
		shotDistance = Vector3.Distance(tank.position, target.position);

		if(shotDistance <= minShotDistance)
		{
			CustomLookAt(target.position);
			if(canShot)
				Shot();
		}
		else
		{
			CustomLookAt(agent.destination);
		}

		if(!isDisabled)
		{
			disabledParticle.SetActive(false);
			agent.isStopped = false;
			if (Vector3.Distance(tank.position, newPoint) > 1.0f && newPoint != null)
			{
				transform.LookAt(agent.destination);
				agent.SetDestination(newPoint);	
			}
			else
			{
				newPoint = Vector3.zero;
				newPoint = GetNewPoint();
			}
		}
		else
		{
			agent.isStopped = true;
		}

		top.localEulerAngles = new Vector3(0, top.localEulerAngles.y, 0);
	}

	void CustomLookAt(Vector3 customTarget)
	{
		Quaternion targetRotation = Quaternion.LookRotation(customTarget - top.position);
        top.rotation = Quaternion.Slerp(top.rotation, targetRotation, rotSpeed * Time.deltaTime);
	}

	void Shot()
	{
		canShot = false;
		muzzleParticle.SetActive(true);
		Invoke("ShotReset", fireRate);
		GameObject newProjectile = (GameObject)Instantiate(projectile, barrelTip.position, barrelTip.rotation);
		newProjectile.GetComponent<Rigidbody>().AddForce(barrelTip.forward * power, ForceMode.Impulse);
	
	}

	void ShotReset()
	{
		canShot = true;
		muzzleParticle.SetActive(false);
	}

	Vector3 GetNewPoint()
	{
		int x = Random.Range(-30, 30);
		int z = Random.Range(-30, 30);

		return new Vector3(x, 0, z);
	}

	void CreateExplosion()
	{
		GameObject newExplosion = (GameObject)Instantiate(explosionDebris, transform.position, Quaternion.identity);
		Destroy(newExplosion, 1);
	}
}