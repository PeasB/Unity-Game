using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	

	public GUIText[] textElements; // The array of GUIText elements to display and scroll
	public float scrollSpeed = 0.2f; // The scrolling speed
	public float Timer = 90.0f;

	void Update () {

			foreach (GUIText text in textElements) // go through each element
			{
				text.transform.Translate(Vector3.up * scrollSpeed); //scroll the text up		 
			}

		Timer -= Time.deltaTime;

		if (Timer <= 0) 
		{
			Application.LoadLevel("Main Menu");
		}

	}


}


