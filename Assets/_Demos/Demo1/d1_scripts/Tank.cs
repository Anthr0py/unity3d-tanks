using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour {

	public float speed, rotSpeed, maxAngle, health, boostRecoveryTime;
	public Transform top, bottom;
	public GameObject disabledParticle, explosionDebris, boostUI, boostParticleL, boostParticleR, repairParticle;

	public Text txtHealth;

	Rigidbody rb;
	float maxHealth;
	bool isDisabled, isBoosting, canBoost, isRepairing;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		maxHealth = health;
		txtHealth.text = health + " Health";
		canBoost = true;
		Cursor.visible = false;
	}

	void Update()
	{
		Movement();


		// Camera pos
		Camera.main.transform.position = new Vector3(transform.position.x, 30, transform.position.z);
	}
	void Movement()
	{
		if(!isDisabled)
		{
			disabledParticle.SetActive(false);

			float angle = Mathf.LerpAngle(bottom.localEulerAngles.y, bottom.localEulerAngles.y + (Input.GetAxis("Horizontal") * 50), Time.deltaTime * rotSpeed);
			bottom.localEulerAngles = new Vector3(0, angle, 0);
			rb.velocity = bottom.forward * (Input.GetAxis("Vertical") * speed);

			// Booster -> Not finished, need bug fixes
			// Booster -> Change from internal tank to SpaceClick weapon
			// Booster -> BUG: Invoke method overlaping with other invokes in weapons
			/*if(Input.GetKey(KeyCode.LeftShift) && !isBoosting && canBoost)
			{
				speed = 9;
				boostParticleL.SetActive(true);
				boostParticleR.SetActive(true);
				canBoost = false;
				isBoosting = true;
				Invoke("StopBoost", 0.65f);
			}*/
		}

		Plane playerPlane = new Plane(Vector3.up, transform.position);
    	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

    	float hitdist = 0.0f;
    	if (playerPlane.Raycast (ray, out hitdist)) 
		{
        	Vector3 targetPoint = ray.GetPoint(hitdist);
        	Quaternion targetRotation = Quaternion.LookRotation(targetPoint - top.position);
        	top.rotation = Quaternion.Slerp(top.rotation, targetRotation, speed * Time.deltaTime);
		}

		top.localEulerAngles = new Vector3(0, top.localEulerAngles.y, 0);
	}

	void StopBoost()
	{
		speed = 5;
		isBoosting = false;
		boostUI.SetActive(true);
		boostParticleL.SetActive(false);
		boostParticleR.SetActive(false);
		Invoke("ResetBoost", boostRecoveryTime);
	}

	void ResetBoost()
	{
		canBoost = true;
		boostUI.SetActive(false);
	}

	void TakeDamage(float damage)
	{
		if(!isRepairing)
			CancelInvoke();

		health -= damage;
		txtHealth.text = health + " Health";
		if(health <= 25 && health > 0 && !isDisabled)
		{
			DisableTank();
		}
		else if(health <= 0)
		{
			DestroyTank();
		}
		else if(health > maxHealth)
		{
			health = maxHealth;
			repairParticle.SetActive(false);
		}
	}

	void RepairDamage()
	{
		TakeDamage(-10);
	}

	void DisableTank()
	{
		isDisabled = true;
		disabledParticle.SetActive(true);
	}

	public void StartTankRepair()
	{
		isRepairing = true;
		repairParticle.SetActive(true);
		InvokeRepeating("RepairDamage", 2.0f, 2.0f);
	}

	public void StopTankRepair()
	{
		isRepairing = false;
		repairParticle.SetActive(false);
		CancelInvoke();
	}

	public void RepairTankStop()
	{
		isRepairing = false;
	}

	void DestroyTank()
	{
		health = 0;
		CreateExplosion();
		transform.position = Vector3.zero;
		TakeDamage(-maxHealth);
		isDisabled = false;
	}

	void CreateExplosion()
	{
		GameObject newExplosion = (GameObject)Instantiate(explosionDebris, transform.position, Quaternion.identity);
		Destroy(newExplosion, 1);
	}
}