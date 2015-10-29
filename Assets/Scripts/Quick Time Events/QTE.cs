using UnityEngine;
using System.Collections;
using System.Xml;

//This Class was orignally writen for the 2015 Game Compeition
//It was writen by Adam Uncao
//This Class can be used on a hidden object that has a colider box.
//This class displays the Button supplied by the XML and waits the given time and hide


public class QTE : MonoBehaviour {
	
	public int QTEFileNumber; //the File Number for the Quick Time event inside of the Quick Time event folder.
	public bool IsTriggeredByMovement; //Triggered By Player entering Area.
	public GameObject Prompt; //The Prompt Prefab Canvas.
	public GameObject MainCamera; //used for the location to put the Prompt.
	public bool OverallPass; // The Overall Outcome of the QTE's. True if Overall Sucesss and False is overall Failure.
	public bool IsOver = false; //Has Happened.
	public bool IsActive = false; //Read if Active.
	public int SuccessCount = 0;
	
	private XmlDocument QTEInstructions = new XmlDocument(); //The Quick time event Xml.
	private GameObject PromptCanvasOnScreen; //The new On screen Prompt object.
	private int QTELevel = 1; //The QTE level in File.
	private int FailLimit; //The Max Fails that can happen
	private int FailCount; //The Current Amount of fails that has happened.

    private string PendingButton; //The Button being waited on.
    private float CurrentTimeRemaining; //The Time the player has.
	
	// Use this for initialization
	void Start () {

		QTEInstructions.Load("Assets/Quick Time Events/No." + QTEFileNumber + ".xml");		                 
	}

	//Update.
	void Update()
	{
		
		if (IsActive) //QTE is currently happening.
		{
			//Check if the pending Button is has been Pressed.
			if (PendingButton != string.Empty && Input.GetButtonDown(PendingButton) && CurrentTimeRemaining > 0)
			{
				ProcessSucessQTE();
			}
			//No time remaing.
			if (CurrentTimeRemaining <= 0 )
			{
				ProcessFailedQTE();
			}
			//Remove Time.
			CurrentTimeRemaining -= Time.deltaTime;
		}
		
	}

	void OnTriggerEnter2D(Collider2D Other)
	{
		if (IsTriggeredByMovement && Other.tag == "Player") 
		{
			//Instantiating a prompt object
			PromptCanvasOnScreen = Instantiate(Prompt, new Vector3(MainCamera.GetComponent<Transform>().position.x, MainCamera.GetComponent<Transform>().position.y, 0), new Quaternion()) as GameObject;
            StartQTE();
		}
	}

	private void StartQTE()//The First time the Qte is triggered. Starts the QTE.
	{
        IsActive = true;
		FailLimit = int.Parse(QTEInstructions.SelectSingleNode("QTE/FailLimit").InnerText);
		FailCount = 0;

        ProcessNextQTE();
	}
    
    void ProcessNextQTE() //Process a new QTE.
    {

        PendingButton = QTEInstructions.SelectSingleNode("QTE/Level" + QTELevel + "/Button").InnerText;
        CurrentTimeRemaining = float.Parse(QTEInstructions.SelectSingleNode("QTE/Level" + QTELevel + "/TimeAllowed").InnerText);

		PromptCanvasOnScreen.transform.FindChild("Button").GetComponent<Animator>().SetInteger("ButtonValue",int.Parse(PendingButton.Remove(0,7)));
    }

	//Called when Current Qte is Passed.
    void ProcessSucessQTE()
    {
		SuccessCount++;
		PromptCanvasOnScreen.SetActive(false);


		if (ShouldEnd())
			OverallPass = true;
    }

	//Called when Current Qte is failed.
    void ProcessFailedQTE()
    {
		PromptCanvasOnScreen.SetActive(false);

		FailCount++;

		if(FailCount >= FailLimit) 
		{
			OverallPass = false;
		}

		ShouldEnd();

    }

	//Check if the whole QTE Event should end. No more Qtes or Too many fails.
	bool ShouldEnd()
	{


		//if overall fail no chance of a continued QTE.
		if (FailCount < FailLimit) {
			QTELevel++;
			foreach (XmlNode Child in QTEInstructions.SelectSingleNode("QTE")) {
				if (Child.Name == "Level" + QTELevel) {
					return false;
				}
			}

		}

		//Set all Closing States.
		IsActive = false;
		IsOver = true;
		PromptCanvasOnScreen.SetActive (false);

		return true;
	}
}
