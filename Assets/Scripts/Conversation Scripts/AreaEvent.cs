using UnityEngine;
using System.Collections;

public class AreaEvent : MonoBehaviour {

    public Conversation DialogueManger;
    public int ConversationID;
	public int SceneNum;



    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Player")
        {
            //Start Specfied Conversation (ID Number references ConversationDatabase).
            DialogueManger.StartConversation(SceneNum,ConversationID);
        }
    }


}
