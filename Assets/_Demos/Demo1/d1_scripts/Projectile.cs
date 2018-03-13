using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float damage, speed;
	public GameObject explosion;
	Quaternion rot;
	[HideInInspector]
	public Vector3 targetPos = Vector3.zero;

	public AnimationCurve yFalloff;

	float timer;

	void Start()
	{
		rot = transform.rotation;
		//StartCoroutine(MoveProjectile(transform.position, targetPos, yFalloff, speed));
	}

	void Update()
	{
		if(targetPos != Vector3.zero)
		{
			transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

			Vector3 localPosition = transform.localPosition;
			localPosition.y = yFalloff.Evaluate(timer);
			transform.localPosition = localPosition;

			// Increase the timer by the time since last frame
			timer += Time.deltaTime;
		}
	}

	public void SetTarget(Vector3 target)
	{
		targetPos = target;
	}

	void OnCollisionEnter(Collision col)
	{
		string colTag = col.transform.tag;
		switch(colTag)
		{
			case "Enemy":
				col.transform.SendMessage("DoDamage", damage);
			break;
			case "Player":
				col.transform.SendMessage("TakeDamage", damage);
			break;
		}	

		CreateExplosion(col.contacts[0].point);
		Destroy(this.gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Shield"))
		{
			Debug.Log("Hit Shield");
		}
	}

	void CreateExplosion(Vector3 hitPos)
	{
		GameObject newExplosion = (GameObject)Instantiate(explosion, hitPos, Quaternion.Inverse(transform.localRotation));
		Destroy(newExplosion, 1);
	}
}