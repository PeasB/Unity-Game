using UnityEngine;
using System.Collections;

public class AreaEvent : MonoBehaviour {

    public Conversation DialogueMangerConversation;
    public int ConversationID;



    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Player")
        {
            //Start Specfied Conversation (ID Number references ConversationDatabase).
            DialogueMangerConversation.StartConversation(ConversationID);
        }
    }


}
