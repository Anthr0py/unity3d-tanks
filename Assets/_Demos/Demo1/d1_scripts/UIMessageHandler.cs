using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageHandler : MonoBehaviour {

	public Text message;

	public void SetMessage(string msg, float time)
	{
		message.text = msg;
		message.gameObject.SetActive(true);
		Invoke("DisableMessage", time);
	}

	void DisableMessage()
	{
		message.text = "";
		message.gameObject.SetActive(false);
	}
}