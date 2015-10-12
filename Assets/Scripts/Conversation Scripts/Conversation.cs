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
		//Vaildate Choices
		//Remove un-needed choices
        //If choice removed automaticly choose remaing one.



	}


    public void ProcessDialogue()
    {
        //Increaces DialogueLevel for next piece of Dialogue.
        DialogueLevel++;
        
        //Still more Dialogue.
        if(DialogueLevel <= int.Parse(Conversation.SelectSingleNode("initalDialogue/DialogueCount").InnerText))
        {
            //prints Dialogue.
            DialogueUI.text = Conversation.SelectSingleNode("initalDialogue").ChildNodes[DialogueLevel].InnerText;
        }
    



    }


}
