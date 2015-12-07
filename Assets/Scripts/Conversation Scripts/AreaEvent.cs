using UnityEngine;
using System.Collections;

//Writen By Adam Uncao
//This Class was originally writen for the 2015-2016 game competition.
//This class can be assigned to a Unity object.
//The object does not have a sprite renderer and has a collision box to detect when a player has entered the Conversation.

public class AreaEvent : MonoBehaviour {

	public enum Direction //Direction Values.
	{
		Up = 2,
		Down = 0,
		Left = 3,
		Right = 1,
		Current = 4
	}


    public int ConversationID;
	public int SceneNum;
    public int StoryPart; //if this var == 0, do nothing. If a number is assigned, preform a task after the conversation is done
	public bool CanReactivate;
	public bool ButtonActivated;
	public bool IgnorePlayerLocking = false;
	public Direction PlayerDirection;
    [HideInInspector]
    public ConversationManager ConversationInstance;


	private Canvas ChoiceCanvas;
	private GameObject PlayerObject;
	private bool JustActivated = false;

    
    void Start()
    {
		//Checks if Area Event has been deleted. This would have happened if the gameobject can't be reactivated and the user has loaded the scene again.
		//Don't want to trigger again. so Remove it before it can be triggered.
		DeleteObjects.CheckIfDeleted (this.gameObject.name);

		ChoiceCanvas = GameObject.Find ("Choice Canvas").GetComponent<Canvas>() ; //Find the Choice Canvas in the Scene.

		if (ChoiceCanvas != null) //If the choice Canvas exists then set Conversation Instance.
			ConversationInstance = new ConversationManager (ChoiceCanvas);
		else
			Destroy (this.gameObject);
    }


	void OnTriggerEnter2D(Collider2D Other)
    {
		if(Other.tag == "Player" && !ButtonActivated)
        {
			//If the Conversation Hasnt run and Can be reactivated then Run the Conversation.
			if((ConversationInstance.HasRun && CanReactivate) || !ConversationInstance.HasRun)
			{
            	//Start specified Conversation (ID Number references Conversation file).
            	ConversationInstance.StartConversation(SceneNum,ConversationID);

				if(!IgnorePlayerLocking) //User can Set Ignore the Player Locking.
				{
					//Lock Player Location.
					Other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
					Other.GetComponent<Player>().enabled = false;

					if(PlayerDirection != Direction.Current) //If the User wants to use a different location then the one the player is at.
						Other.GetComponent<Animator>().SetInteger("Direction",(int)PlayerDirection);

					Other.GetComponent<Animator>().SetBool("Moving",false);	
				}

				PlayerObject = Other.gameObject;
			}
        }
    }

	void OnTriggerStay2D(Collider2D Other)
	{
		if (Other.tag == "Player" && Input.GetButtonDown ("Button 0") && !ConversationInstance.IsActive) 
		{
			//If the Conversation Hasnt run and Can be reactivated then Run the Conversation.
			if((ConversationInstance.HasRun && CanReactivate) || !ConversationInstance.HasRun)
			{
				//Start specified Conversation (ID Number references Conversation file).
				ConversationInstance.StartConversation(SceneNum,ConversationID);

				if(!IgnorePlayerLocking) //User can Set Ignore the Player Locking.
				{
					//Lock Player Location.
					Other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
					Other.GetComponent<Player>().enabled = false;
					
					if(PlayerDirection != Direction.Current) //If the User wants to use a different location then the one the player is at.
						Other.GetComponent<Animator>().SetInteger("Direction",(int)PlayerDirection);
					
					Other.GetComponent<Animator>().SetBool("Moving",false);	
				}
				
				PlayerObject = Other.gameObject;
			}
		
		
		}


	}
	
    void Update()
    {
        if (ConversationInstance.IsActive) //Makes sure ConversationInstance has been initalized.
        {
            //if pressed show next Dialogue. JustActivated prevents Dialogue from being processed when clicked to talk.
			//Process Dialogue is run in Start Conversation and running it again will cause the second dialogue to overwrite the first Dialogue.
            if(Input.GetButtonDown("Button 0") && !JustActivated)
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
        

		JustActivated = false;

		//If Conversation can't be reactivated and has already run then Destroy the object and will never show up again.
		if (!ConversationInstance.IsActive && ConversationInstance.HasRun && !CanReactivate) 
		{

            //if StoryPart is not 0, pass it in to a method to run an event
            if (StoryPart != 0)
            {
                if (ConversationInstance.CurrentChoice == 2) //If the optional choice is ran. If its 1 or 3, do nothing
                {
                    StoryPart++;
                }
                StorylineScript.DoActionForScene(StoryPart);
            }

            //Delete object
			DeleteObjects.DeleteObject(this.gameObject.name);
		}

		if (!ConversationInstance.IsActive && ConversationInstance.HasRun) 
		{
			if(!IgnorePlayerLocking)
			{
				//Lock Player Location.
				PlayerObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
				PlayerObject.GetComponent<Player>().enabled = true;
			}
			
		
		
		}

    }

}
