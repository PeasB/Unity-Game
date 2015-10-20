using UnityEngine;

public class AI_Character : MonoBehaviour {

	//Global Var
	Rigidbody2D Body;
	Animator Anim;
	public int WalkSpeed;
    CircleCollider2D CircleCollition;
    public AI_Action Action;
    public int EventType = 0;
    int PreviousEventType = 0; //Checks if the EventType has made a change, if so, create new Original X and Y points
    int Original_X = 0;
    int Original_Y = 0;
    int EventStepCase = 0; //The step case for an event. 

    //int Player_X = (int)Player.Body.position.x; //This is the players X position, NOT the AI's X position
    //int Player_Y = (int)Player.Body.position.y; //This is the players Y position, NOT the AI's Y position


    public enum AI_Action
    {
        Stationary, //AI is completely rest
        StationaryWithDir, //AI is at rest, but changes directions occationaly (at random interval)
        FollowPlayer, //AI follows player
        Event //An event occures with the AI (example: AI preforming a task during cut scene)
    }

	// Use this for initialization
	void Start () {

		Body = GetComponent<Rigidbody2D>();
		Anim = GetComponent<Animator>();
        CircleCollition = GetComponent<CircleCollider2D>();
        
	}
	
    private int FindPath() //Distance from Player to AI
    {        
        //Find Distance from AI to Player using distance formula
        //Body.position.x & Body.position.y is the x and y position of the AI
        //Player.Body.position.x & Player.Body.position.y is the x and y position of the Player
        int Distance = (int)Mathf.Round(Mathf.Sqrt(Mathf.Pow((Player.Body.position.x - Body.position.x), 2) + Mathf.Pow((Player.Body.position.y - Body.position.y), 2)));
                
        return Distance; //Hey Method, are you happy now that you finally get a return value?
    }
    
    private int Find_X_Distance()
    {
        int X_Distance = (int)Player.Body.position.x - (int)Body.position.x;

        int X_Direction = 0;

        if (X_Distance < 0)
        {
            X_Direction = -1;
        }
        else if (X_Distance > 0)
        {
            X_Direction = 1;
        }
                
        return X_Direction;
    }

    private int Find_Y_Distance()
    {
        int Y_Distance = (int)Player.Body.position.y - (int)Body.position.y;

        int Y_Direction = 0;

        if (Y_Distance < 0)
        {
            Y_Direction = -1;
        }
        else if (Y_Distance > 0)
        {
            Y_Direction = 1;
        }
                
        return Y_Direction;
    }

    
    //void OnCollisionEnter2D(Collision2D collisionInfo)
    //{
    //    print("Collision Detected");

    //    if (collisionInfo.gameObject.tag == "")
    //    {            
    //    }
    //}

    void OnCollisionStay2D(Collision2D collisionInfo)
    {       
        if (collisionInfo.gameObject.tag == "") //This is just to make the method happy 
        {
        }

        //The AI is stuck!!! :(
        //Find it's own path to come back

        

    }


        // Update is called once per frame
        void Update () {

        //var
        CircleCollition.enabled = true;

        if (Action == AI_Action.Stationary) //Stay at complete rest
        {
            #region Completely Stationary

            Body.isKinematic = true;

            #endregion
        }
        else if (Action == AI_Action.StationaryWithDir) //Stay at rest, but occationaly change directions
        {
            #region Stationary With Direction

            Body.isKinematic = true;

            if (Random.Range(0, 150) == 69) //Choose random number from 1 - 100. If the number is 69, change directions
            {
                int DirectionRange = Random.Range(0, 4); //Randomly choose a direction: 0 is down, 1 is right, 2 is up, 3 is left
                
                if (DirectionRange == 0) //Look up AI!
                {
                    Anim.SetInteger("Direction", 0);
                }
                else if (DirectionRange == 1) //Look right AI!
                {
                    Anim.SetInteger("Direction", 1);
                }
                else if (DirectionRange == 2) //Look down AI!
                {
                    Anim.SetInteger("Direction", 2);
                }
                else if (DirectionRange == 3) //Look left AI!
                {
                    Anim.SetInteger("Direction", 3);
                }
                //InventoryScript.DisplayInventory();  // <---Testing Purposes, to activate the inventory. Find better way like press of a button
            }
            

            #endregion
        }
        else if (Action == AI_Action.FollowPlayer) //Follow player
        {
            #region Follow Player

            //Var for AI x and y
            float XMove = 0;
            float YMove = 0;
            
            Body.isKinematic = false;

            if (FindPath() > 25) //If AI far away from Player, go to the player
            {
                if (Body.position.x != (int)Player.Body.position.x)
                {
                    XMove = Find_X_Distance();
                }

                if (Body.position.y != (int)Player.Body.position.y)
                {
                    YMove = Find_Y_Distance();
                }
                
            }
            else if (FindPath() < 18) //If Player and AI collide
            {
                //Let Player go through AI, temporarily disable circle collider
                //(gameObject.GetComponent(typeof(Collider)) as Collider).isTrigger = true;
                CircleCollition.enabled = false;
            }

            //Execute Movement
            PlayerMovement.PlayerMove(Body, WalkSpeed, Anim, XMove, YMove);

            #endregion
        }
        else if (Action == AI_Action.Event) //Do an event
        {
            #region Preform Event

            Body.isKinematic = true;

            if (EventType == PreviousEventType) //Continue on with event
            {
                EventStepCase = AI_Events.PreformEvent(Body, WalkSpeed, Anim, EventType, Original_X, Original_Y, EventStepCase); //Execute Event while retrieving the CaseStep
            }
            else //Start a new event
            {
                Original_X = (int)Body.position.x;
                Original_Y = (int)Body.position.y;
                EventStepCase = 0;

                PreviousEventType = EventType;

                AI_Events.PreformEvent(Body, WalkSpeed, Anim, EventType, Original_X, Original_Y, EventStepCase);
            }

            #endregion
        }

           
    }
}
