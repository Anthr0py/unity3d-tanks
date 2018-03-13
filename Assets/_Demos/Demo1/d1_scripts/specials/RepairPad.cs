using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPad : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Player"))
			collider.gameObject.SendMessage("StartTankRepair");
	}

	void OnTriggerExit(Collider collider)
	{
		if(collider.CompareTag("Player"))
			collider.gameObject.SendMessage("StopTankRepair");
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawCube(transform.position, new Vector3(5, 5, 5));
	}
}