using UnityEngine;
using System.Collections;
using System.Xml;

public class QTE : MonoBehaviour {

	public int QTEFileNumber;
	public bool IsTriggeredByMovement; 
	public GameObject Prompt;
	public GameObject MainCamera;
	private XmlDocument QTEInstructions = new XmlDocument();
	private GameObject PromptOnScreen;
	private int QTELevel = 1;
	
	// Use this for initialization
	void Start () {

		QTEInstructions.Load("Assets/Quick Time Events/No." + QTEFileNumber + ".xml");		                 
	}

	void IsTriggerEnter2D(Collider2D Other)
	{

		if (IsTriggeredByMovement) 
		{
			//Instantiating a prompt object
			PromptOnScreen = GameObject.Instantiate(Prompt,new Vector3(MainCamera.GetComponent<Transform>().position.x,MainCamera.GetComponent<Transform>().position.y + 300,0), new Quaternion()) as GameObject;

			StartQTE();
		
		
		}


	}

	private void StartQTE()
	{

		DisplayQTE();
	}

	private void DisplayQTE ()
	{
		int Button = int.Parse(QTEInstructions.SelectSingleNode ("QTE/Level" + QTELevel + "/Button").InnerText);
			PromptOnScreen.GetComponent<Animator>().SetInteger("ButtonValue",Button);

	}
	
}
