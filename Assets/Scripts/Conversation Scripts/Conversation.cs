using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConversationManager {

	private GameObject Choice1UI;
	private GameObject Choice2UI;
    private Text DialogueUI;
    private int ID;
    private int DialogueLevel;
    private int ConversationLevel;
    private XmlNode Conversation;
	public bool IsActive = false;

    public ConversationManager(Canvas CanvasUI)
    {
        Choice1UI = CanvasUI.transform.FindChild("ChoiceUI 1").gameObject;
        Choice2UI = CanvasUI.transform.FindChild("ChoiceUI 2").gameObject;
        DialogueUI = CanvasUI.transform.FindChild("DialogueUI").gameObject.GetComponent<Text>();
		DialogueUI.enabled = false;
		Choice1UI.SetActive(false);
		Choice2UI.SetActive(false);
    }


    public void StartConversation(int SceneID, int ConvoID) //Start Conversation
    {

        this.ConvoID = ConvoID; //Load ID property

        //Load Scene Conversation XML
        XmlDocument Doc = new XmlDocument();
        Doc.Load("Assets/Conversation Files/Scene" + SceneID + ".xml");

        //Find Correct Conversation in Scene.
        foreach(XmlNode Node in Doc.SelectNodes("Conversations/Conversation"))
        {

            if(Node.SelectSingleNode("ID").InnerText == this.ConvoID.ToString())
            {
                this.Conversation = Node;
                break;
            }
           
        }

        //show First line of Dialogue.
        ProcessDialogue();

    }

	//Checks if Conditions in Path Location are vaild and returns true for vaild and false for not vaild.
	private bool VaildateCondition(string ConditionNodePath)
	{

        //Load Save and Create Variables
        XmlDocument Save = new XmlDocument();
        Save.Load("Assets/Scripts/SaveGame.xml");
		bool ConditionsVaild = true;	

            #region Conditions
            //Condition Is Condition List ex: All Relationship Conditions are under Condition
            //Example List: For Relationship Condition <Kate>100</Kate> <Matt>50</Matt>
            foreach (XmlNode Condition in Conversation.SelectSingleNode(ConditionNodePath + "/Conditions").ChildNodes)
            {
                //No Conditions
                if (Condition.Name == "None")
                    break;
                
                //Different Condition Checks
                switch (Condition.Name)
                {
                    case "Relationship": //Relationship Requirment

                        string CurrentPlayer = GameObject.FindWithTag("Player").name;
                        CurrentPlayer = CurrentPlayer.Remove(0, 7); //Removing "Player " in Game object name. ex: Player Josh -> Josh

                        foreach (XmlNode Person in Condition.ChildNodes)
                        {
                            //if Condition does not meet min Relationship
                            if (!(int.Parse(Save.SelectSingleNode("SaveData/Relationships/" + CurrentPlayer + "/" + Person.Name).InnerText) >= int.Parse(Person.InnerText)))
                            {
                                ConditionsVaild = false;
                            }
                        }

                        break;

                    case "InScene": //Character in Scene Requirment check at In Scene Requirments.

                        foreach (XmlNode Person in Condition.ChildNodes)
                        {
                            //If Character does not exist in Scene.
                            if (!((GameObject.Find("AI " + Person.Name) != null) == bool.Parse(Person.InnerText)))
                            {
                                ConditionsVaild = false;
                                break;
                            }
                        }

                        break;
                }
                #endregion

            }

        }
	
    }


    public void ProcessDialogue()
    {
		//Show the Dialogue.
		DialogueUI.enabled = true;

        //Increaces DialogueLevel for next piece of Dialogue.
        DialogueLevel++;

        //Still more Dialogue.
        if(DialogueLevel <= int.Parse(Conversation.SelectSingleNode("initialDialogue/DialogueCount").InnerText))
        {
            //prints Dialogue.
            DialogueUI.text = Conversation.SelectSingleNode("initialDialogue").ChildNodes[DialogueLevel].InnerText;
        }
		//Last Dialogue has been shown and Choices will now be shown.
        else if (DialogueLevel == int.Parse(Conversation.SelectSingleNode("initialDialogue/DialogueCount").InnerText) + 1)
        {
            ConversationLevel++;
            DisplayChoices();
        }

    }

    public void DisplayChoices()
    {
		bool Choice1Vaild = VaildateCondtions();
		
        if(ChoiceVaild1 || ChoiceVaild2)
        {
		   
		   Choice1UI.SetActive(true);
		   Choice2UI.SetActive(true);
           Choice1UI.GetComponentInChildren<Text>().text = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice1/Text").InnerText;
           Choice2UI.GetComponentInChildren<Text>().text = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice2/Text").InnerText;
		   

        } 
        
        
        
         
    }
}
