﻿using UnityEngine;
using System.Collections;

//Writen By Adam Uncao
//This Class was originally writen for the 2015-2016 game competition.
//This class can be assigned to a Unity object.
//The object does not have a sprite renderer and has a collision box to detect when a player has entered the Conversation.

public class AreaEvent : MonoBehaviour {

    public int ConversationID;
	public int SceneNum;
    public Canvas ChoiceCanvas;
	public bool CanReactivate;
    private ConversationManager ConversationInstance;
    
    void Start()
    {
        ConversationInstance = new ConversationManager(ChoiceCanvas);
    }


	void OnTriggerEnter2D(Collider2D Other)
    {
		if(Other.tag == "Player")
        {
			//If the Conversation Hasnt run and Can be reactivated then Run the Conversation.
			if((ConversationInstance.HasRun && CanReactivate) || !ConversationInstance.HasRun)
			{
            		//Start specified Conversation (ID Number references Conversation file).
            		ConversationInstance.StartConversation(SceneNum,ConversationID);
			}
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

        //If the timer is active and Choices are active
        if (ConversationInstance.ChoicesIsActive && ConversationInstance.TimerIsActive)
        {
            //If there is still time.
            if (ConversationInstance.TimeToChoose > 0)
            {
                //Subtract the time since the last frame (seconds)
                ConversationInstance.TimeToChoose -= Time.deltaTime;
                ConversationInstance.DrainTimer(Time.deltaTime);  //Drains the Timer on the Canvas (visual).
            }
            else //No time remaining choose for the player.
            {
                ConversationInstance.ChooseChoice();
            }

        }

    }

}
