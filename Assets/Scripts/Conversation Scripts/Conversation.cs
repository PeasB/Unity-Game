using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.EventSystems;


//Writen By Adam Uncao.
//This Class was originally writen for the 2015-2016 game competition.
//This class can be used to show Dialogue happening between characters.
//It allows the player to choose Choices based on time or no Time.
//It takes a XML file with Conversation (File found by Scene Number).
//This Class is highly dynamic and almost every element in the Conversation can be Disabled or have Conditions. (see example XML for details).

public class ConversationManager {

	private GameObject Choice1UI;
	private GameObject Choice2UI;
    private Text DialogueUI;
    private int DialogueLevel; //Only used In Dialogue Process.
    private GameObject TimerUI;
    private int ConversationLevel;
    private XmlNode Conversation; //The Conversation based on the supplied ID


    //Publics: mainly to tell states.
    public bool IsActive = false;  //Read Only
    public bool ChoicesIsActive = false;  //Read Only
    public int ID;  //Read Only
    public XmlNode DialogueNode; //Can be assigned to choose the current Dialogue Node
	public bool HasRun; //Read Only. Has been run before.
    public float TimeToChoose; //Used in an update function to remove time from the time Remaining.
    public bool TimerIsActive; //Read Only. Can be used to test if the timer is active.

    //Called when the instance of this class is created.
    //Sets up all Canvas Objects for later use.
    public ConversationManager(Canvas CanvasUI)
    {
        Choice1UI = CanvasUI.transform.FindChild("ChoiceUI 1").gameObject;
        Choice2UI = CanvasUI.transform.FindChild("ChoiceUI 2").gameObject;
        TimerUI = CanvasUI.transform.FindChild("Timer").gameObject;
        DialogueUI = CanvasUI.transform.FindChild("DialogueUI").gameObject.GetComponent<Text>();
		DialogueUI.enabled = false;
		Choice1UI.SetActive(false);
		Choice2UI.SetActive(false);
        TimerUI.SetActive(false);
    }

    //Start Conversation (Setup Method)
    public void StartConversation(int SceneID, int ConvoID)
    {

        ID = ConvoID; //Load ID property
        IsActive = true;
		ConversationLevel = 1;
		DialogueLevel = 1;

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
        DialogueNode = Conversation.SelectSingleNode("Level1/initialDialogue"); //Locaion
        ProcessDialogue();

    }

	//Checks if Conditions in Path Location are valid and returns true for valid and false for not valid.
	private bool ValidateConditions(XmlNode ConditionNode)
	{

        //Load Save and Create Variables
        XmlDocument Save = new XmlDocument();
        Save.Load("Assets/Scripts/SaveGame.xml");
		bool ConditionsValid = true;

        #region  All Conditions

        //No conditions.
        if (ConditionNode.InnerText == "None")
            return true;

        //Condition Is a single Condition set under Conditions ex: All Relationship Conditions are under Condition.
        //Example List: For Relationship Condition <Kate>100</Kate> <Matt>50</Matt>
        foreach (XmlNode Condition in ConditionNode.ChildNodes)
        {

            //Different Condition Checks
            switch (Condition.Name)
            {
                #region Relationship Conditions
                case "Relationship": //Relationship Requirment

                    string CurrentPlayer = GameObject.FindWithTag("Player").name;
                    CurrentPlayer = CurrentPlayer.Remove(0, 7); //Removing "Player " in Game object name. ex: Player Josh -> Josh

                    foreach (XmlNode Person in Condition.ChildNodes)
                    {
                        //if Condition does not meet min Relationship value.
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

    //Processes and varifies all Dialogue
    public void ProcessDialogue()
    {
        if (!ChoicesIsActive)
        {
           //Setting DialogueFound to false.
            bool DialogueFound = false;

            //Checking for Vaild Dialogue at Dialogue Level. Increaces with every loop.
            //Detail about loop in below comment.
            for (int i = DialogueLevel; i <= int.Parse(DialogueNode.SelectSingleNode("DialogueCount").InnerText); i++)
            {
                if (ValidateConditions(DialogueNode.ChildNodes[i].SelectSingleNode("Conditions")))
                {
                    //Show the Dialogue.
                    DialogueUI.enabled = true;
                    //prints Dialogue.
                    DialogueUI.text = DialogueNode.ChildNodes[i].SelectSingleNode("Text").InnerText;
                    DialogueLevel++; //Increaces Dialogue Level for next time Process Dialogue is run. This prevents it finding the same Vaild Dialogue Over and Over.
                    DialogueFound = true; //Found Me!
                    break; //Leave
                }

                DialogueLevel++;
               
            }

            //Notes: For Below.
            //The Current Dialogue is the last vaild Dialogue in the Node.
            //Dialogue Level will be greater then than total Dialogue as when the player clicks for the next dialogue no Dialogue will be found.
            //Since no dialogue after the one displayed on the screen (last piece of Dialogue) then  it attempts to end the Conversation.
            //Conversation May not end b/c there might be another Conversation Level.
            //************** To summarize Dialogue Level with always be greater then the Dialogue Currently On Screen. Dialogue level represents the next level **********
            if (DialogueLevel > int.Parse(DialogueNode.SelectSingleNode("DialogueCount").InnerText) && DialogueNode.Name == "AdditionalDialogue" && !DialogueFound)
            {
                ShouldEndConversation();
            }

            //If it is the Next piece of Dialogue that is going to be the last then check if it is vaild.
            //If it is not vaild then display Choices now.
            if(DialogueLevel == int.Parse(DialogueNode.SelectSingleNode("DialogueCount").InnerText) && DialogueNode.Name == "initialDialogue")
            {
                if(!ValidateConditions(DialogueNode.ChildNodes[DialogueLevel].SelectSingleNode("Conditions")))
                {
                    DisplayChoices();
                }
            }
            else if(DialogueLevel > int.Parse(DialogueNode.SelectSingleNode("DialogueCount").InnerText) && DialogueNode.Name == "initialDialogue") //if Dialogue Level is greater then Dialogue Count, (No dialogue after) Then Displayed Dialogue must be the last piece.
            {
                DisplayChoices();
            }

        }
    }

    //Displays the Choices the Player has. Along with the Timer is needed.
    private void DisplayChoices()
    {
        //Vaildating Choice 1 and Choice 2.
        bool Choice1Valid = ValidateConditions(Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice1/Conditions"));
        bool Choice2Valid = ValidateConditions(Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice1/Conditions"));

        if (Choice1Valid && Choice2Valid)
        {
            //Choice Setup.
            ChoicesIsActive = true;
            Choice1UI.SetActive(true);
            Choice2UI.SetActive(true);
            Choice1UI.GetComponentInChildren<Text>().text = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice1/Text").InnerText;
            Choice2UI.GetComponentInChildren<Text>().text = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice2/Text").InnerText;

            if (Conversation.SelectSingleNode("Level" + ConversationLevel + "/TimeToChoose").InnerText != "None")
            {
                TimeToChoose = int.Parse(Conversation.SelectSingleNode("Level" + ConversationLevel + "/TimeToChoose").InnerText); //Amount of Time Player has to choose thier choice.
                TimerUI.SetActive(true);
                TimerUI.GetComponent<Slider>().maxValue = TimeToChoose;
                TimerUI.GetComponent<Slider>().value = TimeToChoose;
                TimerIsActive = true;
            }


        }
        else if (Choice1Valid && !Choice2Valid) //Choice One is only true. Choose Choice 1 automatically.
        {
            ChooseChoice(1);
        }
        else if (Choice2Valid && !Choice1Valid)  //Choice two is only true. Choose Choice 2 automatically.
        {
            ChooseChoice(2);
        }
        else
        {
            ChooseChoice(); //Should avoid reaching. Chould cause natural conversation flow issues. Dialogue after might not make sense. Default 2.
        }

    }

    //ran when the player chooses a choice. Saves the players choice, and moves on.
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

        //hide buttons after choice and stop Timer.
        Choice1UI.SetActive(false);
        Choice2UI.SetActive(false);        
        ChoicesIsActive = false;

        TimerUI.SetActive(false);
        TimerIsActive = false;



        //Process Dialogue Afterwards
        DialogueLevel = 1;
        DialogueNode = Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice" + ChoiceNumber + "/AdditionalDialogue");
        ProcessDialogue();



    }

    //Checks if the Conversation Can end. removes the Canvas, and sets the Conversation to not active.
    private bool ShouldEndConversation()
    {
        ConversationLevel++;

        foreach (XmlNode ChildNode in Conversation.ChildNodes)
        {
            if(ChildNode.Name == "Level" + ConversationLevel) //If another Level exists then start the next level.
            {
				DialogueNode = Conversation.SelectSingleNode("Level" + ConversationLevel + "/initialDialogue");
				DialogueLevel = 1;
                ProcessDialogue();
                return false;
            }
        }

		HasRun = true;
		IsActive = false;
        DialogueUI.enabled = false;
        return true;
    }

    //Drains the time on the timer bases on supplied amount.
    public void DrainTimer(float Amount)
    {
        TimerUI.GetComponent<Slider>().value -= Amount;
    }

}
