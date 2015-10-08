using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Conversation : MonoBehaviour {

	public Button Choice1UI;
	public Button Choice2UI;
    public Text DialogueUI;
    int CurrentConvoID;

	public void StartConversation(int SceneID, int ConvoID)
	{
        //Start Conversation
        CurrentConvoID = ConvoID;
	
        //Pull infomation for database
		XDocument doc = XDocument.Load ("Assets/Conversation Files/Scene" + SceneID + ".xml");
		var ID = doc.Descendants().Elements ("Conversation");
		/*
		foreach (var IDNum in ID) {

			if((int.Parse(IDNum.Descendants("ID"))) == 1)
			{
				var Convo = IDNum;
				print(Convo.Value);
			}

		}
	*/

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
