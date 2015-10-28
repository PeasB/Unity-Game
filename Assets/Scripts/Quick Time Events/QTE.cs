using UnityEngine;
using System.Collections;
using System.Xml;

public class QTE : MonoBehaviour {

	public int QTEFileNumber;
	public bool IsTriggeredByMovement; 
	public GameObject Prompt;
	public GameObject MainCamera;

	private XmlDocument QTEInstructions = new XmlDocument();
	private GameObject PromptCanvasOnScreen;
	private int QTELevel = 1;
    private bool IsActive = false;
    private string PendingButton;
    private float CurrentTimeRemaining;
	
	// Use this for initialization
	void Start () {

		QTEInstructions.Load("Assets/Quick Time Events/No." + QTEFileNumber + ".xml");		                 
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

	private void StartQTE()
	{
        IsActive = true;

        ProcessNewQTE();
	}

    //Displays QTE
	private void DisplayQTE ()
	{
        PromptCanvasOnScreen.transform.FindChild("Button").GetComponent<Animator>().SetInteger("ButtonValue",int.Parse(PendingButton.Remove(0,7)));
    }
    
    void ProcessNewQTE()
    {
        PendingButton = QTEInstructions.SelectSingleNode("QTE/Level" + QTELevel + "/Button").InnerText;
        CurrentTimeRemaining = float.Parse(QTEInstructions.SelectSingleNode("QTE/Level" + QTELevel + "/TimeAllowed").InnerText);
    }

    void Update()
    {

        if (IsActive)
        {
            if (PendingButton != string.Empty && Input.GetButtonDown(PendingButton) && CurrentTimeRemaining > 0)
            {
                ProcessSucessQTE();
            }

             if (CurrentTimeRemaining <= 0 )
            {
                ProcessFailedQTE();
            }

            CurrentTimeRemaining -= Time.deltaTime;
        }

    }


    void ProcessSucessQTE()
    {


    }

    void ProcessFailedQTE()
    {


    }

}
