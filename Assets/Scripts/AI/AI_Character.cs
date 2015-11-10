//By: Ryan Dailey
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

    int PlayerPositionCounter = 0; //Everytime it hits 10, take a snapshot of the players x and y position
    int[,] PlayerPreviousPosition = new int[120, 2];
    int ArrayCount = 0; //Goes up to 119
    int AIarrayPart = 0; //Where in the array is the AI?
    int Counter7 = 0; //when it hits 11, AIarrayCount++

    int PlayerPreviousX = (int)Player.Body.position.x; //Previous X position of player after 30 frames
    int PlayerPreviousY = (int)Player.Body.position.y; //Previous Y position of player after 30 frames

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
        int X_Distance = PlayerPreviousPosition[AIarrayPart, 0] - (int)Body.position.x; //(int)Player.Body.position.x - (int)Body.position.x;
        
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
        int Y_Distance = PlayerPreviousPosition[AIarrayPart, 1] - (int)Body.position.y; //(int)Player.Body.position.y - (int)Body.position.y;

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
                //find players position every 10 frames (1/6 a second). This is so it remembers the players path. (history path)               
                    if (PlayerPositionCounter >= 10)
                    {                        
                        if (ArrayCount >= 120) //Go to beginning of array
                        {
                            ArrayCount = 0;
                        }
                        PlayerPreviousPosition[ArrayCount, 0] = (int)Player.Body.position.x;
                        PlayerPreviousPosition[ArrayCount, 1] = (int)Player.Body.position.y;
                        ArrayCount++;

                        PlayerPositionCounter = 0;
                    }
                    PlayerPositionCounter++;


                //AI follow the history path of player

                //Follow to interval 
                if (Counter7 >= 11) //Do not make is 10 or less!!!
                {
                    AIarrayPart++;
                    if (AIarrayPart >= 120)
                    {
                        AIarrayPart = 0;
                    }
                    Counter7 = 0;

                    //check 1 behind ai count to see if the (x,y) is the same, and if so, preform a skip (for a max skip length of one full period (120)
                    //special case of when array index is 0, it looks back at index 119
                    if (AIarrayPart == 0) //Special Case
                    {
                        //Look back in time
                        if (PlayerPreviousPosition[AIarrayPart, 0] == PlayerPreviousPosition[119, 0] && PlayerPreviousPosition[AIarrayPart, 1] == PlayerPreviousPosition[119, 1])
                        {
                            //If the same, do a skip
                            for (int i = 0; i < 120; i++)
                            {
                                AIarrayPart++; //Increment AIarrayPart counter
                                if (AIarrayPart >= 120) //Check if is at end of array so it can start from beginning
                                {
                                    AIarrayPart = 0;
                                }

                                if (AIarrayPart == ArrayCount || PlayerPreviousPosition[AIarrayPart, 0] != PlayerPreviousPosition[119, 0])
                                {
                                    break; //Exit loop
                                }
                            }
                        }
                    }
                    else //Regular Case
                    {
                        //Look back in time
                        if (PlayerPreviousPosition[AIarrayPart, 0] == PlayerPreviousPosition[AIarrayPart - 1, 0] && PlayerPreviousPosition[AIarrayPart, 1] == PlayerPreviousPosition[AIarrayPart - 1, 1])
                        {
                            //If the same, do a skip
                            for (int i = 0; i < 120; i++)
                            {
                                AIarrayPart++; //Increment AIarrayPart counter
                                if (AIarrayPart >= 120) //Check if is at end of array so it can start from beginning
                                {
                                    AIarrayPart = 0;
                                }

                                if (AIarrayPart == ArrayCount || PlayerPreviousPosition[AIarrayPart, 0] != PlayerPreviousPosition[AIarrayPart - 1, 0])
                                {
                                    break; //Exit loop
                                }
                            }
                        }
                    }


                }
                Counter7++;


                    if (Body.position.x != PlayerPreviousPosition[AIarrayPart, 0])
                    {
                        XMove = Find_X_Distance();
                    }

                    if (Body.position.y != PlayerPreviousPosition[AIarrayPart, 1])
                    {
                        YMove = Find_Y_Distance();
                    }

            }
            else if (FindPath() < 25) //If Player and AI collide
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
