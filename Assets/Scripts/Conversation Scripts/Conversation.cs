using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Conversation : MonoBehaviour {

	public Button Choice1UI;
	public Button Choice2UI;
    public Text DialogueUI;
    int CurrentConvoID;

	public void StartConversation(int ConvoID)
	{
        //Start Conversation
        CurrentConvoID = ConvoID;
        //Pull infomation for database

       // DialogueUI.enabled = true;

        VaildateChoices(0);

	}

	void VaildateChoices(int ConversationLevel)
	{
		//Vaildate Choices
		//Remove un-needed choices
        //If choice removed automaticly choose remaing one.



	}


    //End of Convo. Files that has content tells next Convo number to set.

}
