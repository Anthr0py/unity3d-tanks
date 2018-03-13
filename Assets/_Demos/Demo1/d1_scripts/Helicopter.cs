using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Helicopter : MonoBehaviour {

	//public Vector2 randomDropArea;
	public float speed;
	public float rotSpeed;
	public NavMeshAgent agent;

	Vector3 curDropArea;

	public float distance;

	void Start()
	{
		curDropArea = GetNewDropArea();
	}

	void Update()
	{
		distance = Vector3.Distance(transform.position, curDropArea);
		if (distance > 1.0f)
        {
			transform.LookAt(agent.destination);
			agent.SetDestination(curDropArea);
        }
		else
		{
			DropPackage();
			curDropArea = GetNewDropArea();
		}
	}

	void DropPackage()
	{
		if(Random.value > 0.8f)
		{
			Debug.Log("Drop Package");
		}
		curDropArea = GetNewDropArea();
	}

	Vector3 GetNewDropArea()
	{
		int x = Random.Range(-30, 30);
		int z = Random.Range(-30, 30);

		return new Vector3(x, 0, z);
	}
}