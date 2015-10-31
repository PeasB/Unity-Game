using UnityEngine;
using System.Collections;

public class ExampleEventCaller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		GameObject QTEObject = GameObject.FindWithTag ("QTE");

		if(QTEObject.GetComponent<QTE>().IsActive && QTEObject.GetComponent<QTE> ().NewQTEReady) 
		{
			QTEObject.GetComponent<QTE>().ProcessNextQTE();
		
		}



	}
}
