using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConversationManager {

	private Button Choice1UI;
	private Button Choice2UI;
    private Text DialogueUI;
    private int ConvoID;
    private int DialogueLevel;
    private int ConversationLevel;
    private XmlNode Conversation;


    public ConversationManager(Canvas CanvasUI)
    {
        Choice1UI = CanvasUI.transform.FindChild("ChoiceUI 1").gameObject.GetComponent<Button>();
        Choice2UI = CanvasUI.transform.FindChild("ChoiceUI 2").gameObject.GetComponent<Button>();
        DialogueUI = CanvasUI.transform.FindChild("DialogueUI").gameObject.GetComponent<Text>();
    }


    public void StartConversation(int SceneID, int ConvoID) //Start Conversation
    {

        this.ConvoID = ConvoID; //Load ID property

        //Load Scene Conversation XML
        XmlDocument Doc = new XmlDocument();
        Doc.Load("Assets\\Conversation Files\\Scene" + SceneID + ".xml");

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

        //Show the Canvas.
        DialogueUI.GetComponentInParent<Canvas>().enabled = true;

    }

	private void VaildateChoices(int ConversationLevel)
	{

        //Load Save and Create Variables
        XmlDocument Save = new XmlDocument();
        Save.Load("Assets\\Scripts\\SaveGame.xml");
        bool ChoiceVaild = true;
        bool ChoiceOneVaild = true;
        bool ChoiceTwoVaild = false;

      
        for (int i = 1; i <= 2; i++)
        {
            //Condition Is Condition List ex: All Relationship Conditions are under Condition
            //Example List: For Relationship Condition <Kate>100</Kate> <Matt>50</Matt>
            foreach (XmlNode Condition in Conversation.SelectSingleNode("Level" + ConversationLevel + "/Choice" + i + "/Conditions").ChildNodes)
            {
                //No Conditions
                if (Condition.Name == "None")
                    break;
                #region Condition Checks
                //Different Condition Checks
                switch (Condition.Name)
                {
                    case "Relationship": //Relationship Requirment

                        string CurrentPlayer = GameObject.FindWithTag("Player").name;
                       CurrentPlayer = CurrentPlayer.Remove(0, 7); //Removing Player in Game object name. ex: Player Josh -> Josh

                        foreach (XmlNode Person in Condition.ChildNodes)
                        {
                            //if Condition does not meet min Relationship
                            if (!(int.Parse(Save.SelectSingleNode("Relationships/" + CurrentPlayer + "/" + Person.Name).InnerText) >= int.Parse(Person.InnerText)))
                            {
                                ChoiceVaild = false;
                            }
                        }

                        break;

                    case "InScene": //Character in Scene Requirment

                        foreach (XmlNode Person in Condition.ChildNodes)
                        {
                            //If Character does not exist in Scene.
                            if (!((GameObject.Find("Ai " + Person.Name) != null) == bool.Parse(Person.InnerText)))
                            {
                                ChoiceVaild = false;
                                break;
                            }


                        }

                        break;

                }
                #endregion

                if (!ChoiceVaild)
                {
                    if (i == 1)
                        ChoiceOneVaild = false;
                    else if (i == 2)
                        ChoiceTwoVaild = false;

                    break;
                }
            }

        }

        #region Choice Display Logic
        if (ChoiceOneVaild && ChoiceTwoVaild)
        {
            //Diplay both choices
        }
        else if(ChoiceOneVaild && !ChoiceTwoVaild)
        {
            //auto choose Choice 1
        }
        else if (ChoiceTwoVaild && !ChoiceOneVaild)
        {
            //auto choose Choice 2
        }
        else
        {
            //Move on with out choice
        }

        #endregion

    }


    public void ProcessDialogue()
    {
        //Increaces DialogueLevel for next piece of Dialogue.
        DialogueLevel++;
        
        //Still more Dialogue.
        if(DialogueLevel <= int.Parse(Conversation.SelectSingleNode("initialDialogue/DialogueCount").InnerText))
        {
            //prints Dialogue.
            DialogueUI.text = Conversation.SelectSingleNode("initialDialogue").ChildNodes[DialogueLevel].InnerText;
        }
        else if (DialogueLevel > int.Parse(Conversation.SelectSingleNode("initialDialogue/DialogueCount").InnerText))
        {
            ConversationLevel++;
            VaildateChoices(ConversationLevel);
        }



    }


}
