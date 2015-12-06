using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerOn : MonoBehaviour {


	void Start()
	{
		GameObject.Find("Power On").transform.FindChild("Finished Breaker Conversation").gameObject.SetActive(false);

	}

	void OnTriggerStay2D(Collider2D Other)
	{
		if (Input.GetButtonDown ("Button 0")) 
		{
			GameObject.Find("Power On").transform.FindChild("Light").gameObject.SetActive(true);
			GameObject.Find("Power On").transform.FindChild("Finished Breaker Conversation").gameObject.SetActive(true);
			GameObject Message = GameObject.Find ("Message_Canvas").transform.FindChild ("Text").gameObject;
			Message.gameObject.SetActive (false);
		}


	}

	void OnTriggerEnter2D(Collider2D Other)
	{
		GameObject Message = GameObject.Find ("Message_Canvas").transform.FindChild ("Text").gameObject;

		Message.gameObject.SetActive (true);
		Message.GetComponent<Text>().text = "Press X to turn the power on";

	}

	void OnTriggerExit2D(Collider2D Other)
	{
		GameObject Message = GameObject.Find ("Message_Canvas").transform.FindChild ("Text").gameObject;
		Message.gameObject.SetActive (false);
	}

}
