using UnityEngine;
using System.Collections;

public class AreaEvent : MonoBehaviour {

    public int ConversationID;
	public int SceneNum;
    public Canvas EventCanvas;
    private ConversationManager ConversationInstance;
    private bool Active = false;

    void Start()
    {
        ConversationInstance = new ConversationManager(EventCanvas);
    }


    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Player")
        {

            Active = true;

            //Start specified Conversation (ID Number references Conversation file).
            ConversationInstance.StartConversation(SceneNum,ConversationID);

        }
    }


    void Update()
    {
        if(Active) //Makes sure ConversationInstance has been initalized.
        {
            //if pressed show next Dialogue.
            if(Input.GetButtonDown("Button 0"))
            {
              ConversationInstance.ProcessDialogue();
            }

            
        }



    }

}
