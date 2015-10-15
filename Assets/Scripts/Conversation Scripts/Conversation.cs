using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConversationManager {

	private GameObject Choice1UI;
	private GameObject Choice2UI;
    private Text DialogueUI;
    private int DialogueLevel; //Only used In Dialogue Process.
    private int ConversationLevel;
    private XmlNode Conversation;


    //Publics (used to mainly to tell states.
    public bool IsActive = false;
    public bool ChoicesIsActive = false;
    public int ID;
    public string DialogueLocation;
	public bool HasRun;

    public ConversationManager(Canvas CanvasUI)
    {
        Choice1UI = CanvasUI.transform.FindChild("ChoiceUI 1").gameObject;
        Choice2UI = CanvasUI.transform.FindChild("ChoiceUI 2").gameObject;
        DialogueUI = CanvasUI.transform.FindChild("DialogueUI").gameObject.GetComponent<Text>();
		DialogueUI.enabled = false;
		Choice1UI.SetActive(false);
		Choice2UI.SetActive(false);
    }


    public void StartConversation(int SceneID, int ConvoID) //Start Conversation (Setup Method)
    {

        ID = ConvoID; //Load ID property
        IsActive = true;
		ConversationLevel = 1;
		DialogueLevel = 0;

        //Load Scene Conversation XML
        XmlDocument Doc = new XmlDocument();
        Doc.Load("Assets/Conversation Files/Scene" + SceneID + ".xml");

        //Find Correct Conversation in Scene XML file.
        foreach(XmlNode Node in Doc.SelectNodes("Conversations/Conversation"))
        {
            if(Node.SelectSingleNode("ID").InnerText == ID.ToString())
            {
                Conversation = Node;
                break;
            }    
        }

        //show First line of Dialogue.
        DialogueLocation = "Level1/initialDialogue"; //Locaion
        ProcessDialogue();

    }

	//Checks if Conditions in Path Location are valid and returns true for valid and false for not valid.
	private bool ValidateConditions(string ConditionNodePath)
	{

        //Load Save and Create Variables
        XmlDocument Save = new XmlDocument();
        Save.Load("Assets/Scripts/SaveGame.xml");
		bool ConditionsValid = true;	

            #region  All Conditions
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
                #region Relationship Conditions
                case "Relationship": //Relationship Requirment

                        string CurrentPlayer = GameObject.FindWithTag("Player").name;
                        CurrentPlayer = CurrentPlayer.Remove(0, 7); //Removing "Player " in Game object name. ex: Player Josh -> Josh

                        foreach (XmlNode Person in Condition.ChildNodes)
                        {
                            //if Condition does not meet min Relationship
                            if (!(int.Parse(Save.SelectSingleNode("SaveData/Relationships/" + CurrentPlayer + "/" + Person.Name).InnerText) >= int.Parse(Person.InnerText)))
                            {
                                ConditionsValid = false;
                            }
                        }

                        break;
                #endregion
                #region In Scene Conditions
                case "InScene": //Character in Scene Requirment check at In Scene Requirments.

                        foreach (XmlNode Person in Condition.ChildNodes)
                        {
                            //If Character does not exist in Scene.
                            if (!((GameObject.Find("AI " + Person.Name) != null) == bool.Parse(Person.InnerText)))
                            {
                                ConditionsValid = false;
                                break;
                            }
                        }

                        break;
                #endregion
            }
        }

        #endregion

        return ConditionsValid;
    }


    public void ProcessDialogue()
    {
		if(!ChoicesIsActive)
		{
		
	        //Increaces DialogueLevel for next piece of Dialogue.
	        DialogueLevel++;

	        //Still more Dialogue.
	        if(DialogueLevel <= int.Parse(Conversation.SelectSingleNode(DialogueLocation + "/DialogueCount").InnerText))
	        {
				//Show the Dialogue.
				DialogueUI.enabled = true;

	            //prints Dialogue.
	            DialogueUI.text = Conversation.SelectSingleNode(DialogueLocation).ChildNodes[DialogueLevel].InnerText;

	        }
			//Last Dialogue has been shown and Choices will now be shown.
	        else if (DialogueLevel >= int.Parse(Conversation.SelectSingleNode(DialogueLocation + "/DialogueCount").InnerText))
	        {
				if(DialogueLevel >= int.Parse(Conversation.SelectSingleNode(DialogueLocation + "/DialogueCount").InnerText))
				{
					if(DialogueLocation == "Level" + ConversationLevel + "/initialDialogue")
						DisplayChoices(ConversationLevel);       
					else
						ShouldEndConversation();
				}
	        }
		}
    }

    public void DisplayChoices(int ConversationLevel)
    {
        
        bool Choice1Valid = ValidateConditions("Level" + ConversationLevel + "/Choice1");
        bool Choice2Valid = ValidateConditions("Level" + ConversationLevel + "/Choice2");

        if (Choice1Valid || Choice2Valid)
        {
           ChoicesIsActive = true;
           Choice1UI.SetActive(true);
		   Choice2UI.SetActive(true);
           Choice1UI.GetComponentInChildren<Text>().text = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice1/Text").InnerText;
           Choice2UI.GetComponentInChildren<Text>().text = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice2/Text").InnerText;
		   

        } 
 
    }


    public void ChooseChoice(int ChoiceNumber = 2)
    {
        //Choice Number can be one or two.
        //Conquences Happen here.

        XmlDocument Save = new XmlDocument();
        Save.Load("Assets/Scripts/SaveGame.xml");

        //Path Conquences
        foreach (XmlNode Situation in Save.SelectSingleNode("SaveData/StoryPaths"))
        {
            if (int.Parse(Situation.SelectSingleNode("ID").InnerText) == ID)
            {
                Situation.SelectSingleNode("Outcome").InnerText = ChoiceNumber.ToString();
            }
        }

        //hide buttons after choice
        Choice1UI.SetActive(false);
        Choice2UI.SetActive(false);
        ChoicesIsActive = false;

        //Process Dialogue Afterwards
        DialogueLevel = 0;
        DialogueLocation = "Level" + ConversationLevel + "/Choice" + ChoiceNumber + "/AdditionalDialogue";
        ProcessDialogue();



    }


    private bool ShouldEndConversation()
    {
        ConversationLevel++;

        foreach (XmlNode ChildNode in Conversation.ChildNodes)
        {
            if(ChildNode.Name == "Level" + ConversationLevel) //If another Level exists then start the next level.
            {
				DialogueLocation = "Level" + ConversationLevel + "/initialDialogue";
				DialogueLevel = 0;
                return false;
            }
        }

		HasRun = true;
		IsActive = false;
        DialogueUI.enabled = false;
        return true;
    }
}
