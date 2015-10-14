using UnityEngine;
using System.Collections;

public class AreaEvent : MonoBehaviour {

    public int ConversationID;
	public int SceneNum;
    public Canvas EventCanvas;
    private ConversationManager ConversationInstance;
    
    void Start()
    {
        ConversationInstance = new ConversationManager(EventCanvas);
    }


    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Player")
        {
            //Start specified Conversation (ID Number references Conversation file).
            ConversationInstance.StartConversation(SceneNum,ConversationID);
            ConversationInstance.DialogueLocation = "initialDialogue"; //Setting Dialogue Location
        }
    }


    void Update()
    {
        if(ConversationInstance.IsActive) //Makes sure ConversationInstance has been initalized.
        {
            //if pressed show next Dialogue.
            if(Input.GetButtonDown("Button 0"))
            {
                ConversationInstance.ProcessDialogue();
            }
            else if(Input.GetButtonDown("Button 2") && ConversationInstance.ChoicesIsActive) //X Button
            {
                ConversationInstance.ChooseChoice(1);
            }
            else if(Input.GetButtonDown("Button 1") && ConversationInstance.ChoicesIsActive)  //B Button
            {
                ConversationInstance.ChooseChoice(2);
            }
            
        }



    }

}
