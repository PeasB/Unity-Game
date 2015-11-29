﻿//By: Ryan Dailey
using UnityEngine;
using System.Xml;

public class AI_Character : MonoBehaviour {

	//Global Var
	Rigidbody2D Body;
	Animator Anim;
	public int WalkSpeed;
    CircleCollider2D CircleCollition;
    public AI_Action Action;
    public int EventType = 0;
    int PreviousEventType = 0; //Checks if the EventType has made a change, if so, create new Original X and Y points
    double Original_X = 0;
    double Original_Y = 0;
    int EventStepCase = 0; //The step case for an event. 

    int PlayerPositionCounter = 10; //Everytime it hits 10, take a snapshot of the players x and y position
    double[,] PlayerPreviousPosition = new double[120, 2]; //int[,] PlayerPreviousPosition = new int[120, 2];
    int ArrayCount = 0; //Goes up to 119
    int AIarrayPart = 0; //Where in the array is the AI?
    int Counter7 = 0; //when it hits 11, AIarrayCount++

    int CounterPos = 0; //After every 150 frames, write AI's x and y position to SaveData

    //double PlayerPreviousX = Player.Body.position.x; //Previous X position of player after x frames
    //double PlayerPreviousY = Player.Body.position.y; //Previous Y position of player after x frames

    bool FollowOtherAI = false; //if false, follow player. if true, follow other AI
    
    public GameObject OtherAI_Object;
    private AI_Character Script_OtherAIObject;

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

        //Check for players position and action
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");
        
        if (this.gameObject.name == "AI Josh" & SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/X").InnerText != "" && SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Y").InnerText != "")
        {
            //Set players x and y to what its saved in the save data
            this.GetComponent<Transform>().position = new Vector3(float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/X").InnerText), float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Y").InnerText));
            if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Action").InnerText == "FollowPlayer") Action = AI_Action.FollowPlayer;
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Action").InnerText = "";
        }
        else if (this.gameObject.name == "AI Matt" & SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/X").InnerText != "" && SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Y").InnerText != "")
        {
            //Set players x and y to what its saved in the save data
            this.GetComponent<Transform>().position = new Vector3(float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/X").InnerText), float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Y").InnerText));
            if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Action").InnerText == "FollowPlayer") Action = AI_Action.FollowPlayer;
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Action").InnerText = "";
        }
        else if (this.gameObject.name == "AI Kate" & SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/X").InnerText != "" && SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Y").InnerText != "")
        {
            //Set players x and y to what its saved in the save data
            this.GetComponent<Transform>().position = new Vector3(float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/X").InnerText), float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Y").InnerText));
            if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Action").InnerText == "FollowPlayer") Action = AI_Action.FollowPlayer;
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Action").InnerText = "";
        }
        else if (this.gameObject.name == "AI April" & SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/X").InnerText != "" && SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Y").InnerText != "")
        {
            //Set players x and y to what its saved in the save data
            this.GetComponent<Transform>().position = new Vector3(float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/X").InnerText), float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Y").InnerText));
            if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Action").InnerText == "FollowPlayer") Action = AI_Action.FollowPlayer;
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Action").InnerText = "";
        }
        else if (this.gameObject.name == "AI Ethan" & SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/X").InnerText != "" && SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Y").InnerText != "")
        {
            //Set players x and y to what its saved in the save data
            this.GetComponent<Transform>().position = new Vector3(float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/X").InnerText), float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Y").InnerText));
            if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Action").InnerText == "FollowPlayer") Action = AI_Action.FollowPlayer;
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Action").InnerText = "";
        }

        //Save XML
        SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

        Body = GetComponent<Rigidbody2D>();
		Anim = GetComponent<Animator>();
        CircleCollition = GetComponent<CircleCollider2D>();


        //if (this.gameObject.name != "AI Matt" && GameObject.Find("AI Matt") != null)
        //{
        //    if (GameObject.Find("AI Matt").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
        //        OtherAI_Object = GameObject.Find("AI Matt");
        //}
        //if (this.gameObject.name != "AI Josh" && GameObject.Find("AI Josh") != null)
        //{
        //    if (GameObject.Find("AI Josh").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
        //        OtherAI_Object = GameObject.Find("AI Josh");    
        //}
        //if (this.gameObject.name != "AI Kate" && GameObject.Find("AI Kate") != null)
        //{
        //    if (GameObject.Find("AI Kate").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
        //        OtherAI_Object = GameObject.Find("AI Kate");
        //}
        //if (this.gameObject.name != "AI April" && GameObject.Find("AI April") != null)
        //{
        //    if (GameObject.Find("AI April").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
        //        OtherAI_Object = GameObject.Find("AI April");
        //}
        //if (this.gameObject.name != "AI Ethan" && GameObject.Find("AI Ethan") != null)
        //{
        //    if (GameObject.Find("AI Ethan").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
        //        OtherAI_Object = GameObject.Find("AI Ethan");
        //}


        //if (OtherAI_Object != null)
        //    Script_OtherAIObject = OtherAI_Object.GetComponent<AI_Character>();



    }

    private double FindPath() //Distance from Player to AI
    {
        //Find Distance from AI to Player using distance formula
        //Body.position.x & Body.position.y is the x and y position of the AI
        //Player.Body.position.x & Player.Body.position.y is the x and y position of the Player

        double Distance;

        if (OtherAI_Object != null && Script_OtherAIObject.Action == AI_Action.FollowPlayer)
        {
            double AI_To_Player = Mathf.Sqrt(Mathf.Pow((Player.Body.position.x - Body.position.x), 2) + Mathf.Pow((Player.Body.position.y - Body.position.y), 2));
            double OtherAI_To_Player = Mathf.Sqrt(Mathf.Pow((Player.Body.position.x - Script_OtherAIObject.Body.position.x), 2) + Mathf.Pow((Player.Body.position.y - Script_OtherAIObject.Body.position.y), 2));
            double AI_To_OtherAI = Mathf.Sqrt(Mathf.Pow((Body.position.x - Script_OtherAIObject.Body.position.x), 2) + Mathf.Pow((Body.position.y - Script_OtherAIObject.Body.position.y), 2));

            if (AI_To_Player < OtherAI_To_Player)
            {
                Distance = AI_To_Player;
                FollowOtherAI = false;
            }
            else
            {
                Distance = AI_To_OtherAI;
                FollowOtherAI = true;
            }
        }
        else
        {
            Distance = Mathf.Sqrt(Mathf.Pow((Player.Body.position.x - Body.position.x), 2) + Mathf.Pow((Player.Body.position.y - Body.position.y), 2));
            FollowOtherAI = false;
        }
        
        //Distance = Mathf.Sqrt(Mathf.Pow((Player.Body.position.x - Body.position.x), 2) + Mathf.Pow((Player.Body.position.y - Body.position.y), 2));
        return Distance; //Hey Method, are you happy now that you finally get a return value?
    }
    
    private int Find_X_Distance()
    {
        double X_Distance;

        
            X_Distance = PlayerPreviousPosition[AIarrayPart, 0] - Body.position.x; //(int)Player.Body.position.x - (int)Body.position.x;
        

        
        int X_Direction = 0;

        if (X_Distance < -0.05)
        {
            X_Direction = -1;
        }
        else if (X_Distance > 0.05)
        {
            X_Direction = 1;
        }
                
        return X_Direction;
    }

    private int Find_Y_Distance()
    {
        double Y_Distance;

        
            Y_Distance = PlayerPreviousPosition[AIarrayPart, 1] - Body.position.y;
      


        int Y_Direction = 0;

        if (Y_Distance < -0.05)
        {
            Y_Direction = -1;
        }
        else if (Y_Distance > 0.05)
        {
            Y_Direction = 1;
        }
                
        return Y_Direction;
    }
    
        // Update is called once per frame
    void Update ()
    {

        #region Check for AI Follow Link
        if (OtherAI_Object == null)
        {
            if (this.gameObject.name != "AI Matt" && GameObject.Find("AI Matt") != null)
            {
                if (GameObject.Find("AI Matt").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
                    OtherAI_Object = GameObject.Find("AI Matt");
            }
            if (this.gameObject.name != "AI Josh" && GameObject.Find("AI Josh") != null)
            {
                if (GameObject.Find("AI Josh").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
                    OtherAI_Object = GameObject.Find("AI Josh");
            }
            if (this.gameObject.name != "AI Kate" && GameObject.Find("AI Kate") != null)
            {
                if (GameObject.Find("AI Kate").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
                    OtherAI_Object = GameObject.Find("AI Kate");
            }
            if (this.gameObject.name != "AI April" && GameObject.Find("AI April") != null)
            {
                if (GameObject.Find("AI April").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
                    OtherAI_Object = GameObject.Find("AI April");
            }
            if (this.gameObject.name != "AI Ethan" && GameObject.Find("AI Ethan") != null)
            {
                if (GameObject.Find("AI Ethan").GetComponent<AI_Character>().Action == AI_Action.FollowPlayer)
                    OtherAI_Object = GameObject.Find("AI Ethan");
            }


            if (OtherAI_Object != null)
                Script_OtherAIObject = OtherAI_Object.GetComponent<AI_Character>();
        }
        #endregion

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

            if (Random.Range(0, 150) == 69) //Choose random number from 1 - 150. If the number is 69, change directions
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

            #region SavePosition
            //Check if you need to write AI position to save data
            CounterPos++;
            if (CounterPos >= 150)
            {
                //Read in SaveGame.xml
                XmlDocument SaveGameDoc = new XmlDocument();
                SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                if (this.gameObject.name == "AI Josh")
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/X").InnerText = transform.position.x.ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Y").InnerText = transform.position.y.ToString();
                }
                else if (this.gameObject.name == "AI Matt")
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/X").InnerText = transform.position.x.ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Y").InnerText = transform.position.y.ToString();
                }
                else if (this.gameObject.name == "AI Kate")
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/X").InnerText = transform.position.x.ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Y").InnerText = transform.position.y.ToString();
                }
                else if (this.gameObject.name == "AI April")
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/X").InnerText = transform.position.x.ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Y").InnerText = transform.position.y.ToString();
                }
                else if (this.gameObject.name == "AI Ethan")
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/X").InnerText = transform.position.x.ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Y").InnerText = transform.position.y.ToString();
                }

                //Save XML
                SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

                CounterPos = 0;
            }
            #endregion
            
            //Var for AI x and y
            float XMove = 0;
            float YMove = 0;
            
            Body.isKinematic = false;

            if (FindPath() > 1.75) //If AI far away from Player, go to the player
            {
                //if game just started and everything is set to 0 (and if everything is 0, it causes problems)
                if (PlayerPreviousPosition[0, 0] == 0 && PlayerPreviousPosition[0, 1] == 0 && AIarrayPart == 0)
                {
                    for (int i = 0; i < 120; i++) //fill entire array with players position (better than 0)
                    {
                        PlayerPreviousPosition[i, 0] = Player.Body.position.x;
                        PlayerPreviousPosition[i, 1] = Player.Body.position.y;
                    }
                }

                //find players position every 10 frames (1/6 a second). This is so it remembers the players path. (history path)               
                if (PlayerPositionCounter >= 10)
                    {                        
                        if (ArrayCount >= 120) //Go to beginning of array
                        {
                            ArrayCount = 0;
                        }
                        PlayerPreviousPosition[ArrayCount, 0] = Player.Body.position.x;
                        PlayerPreviousPosition[ArrayCount, 1] = Player.Body.position.y;
                        ArrayCount++;

                        PlayerPositionCounter = 0;
                    }
                    PlayerPositionCounter++;


                //AI follow the history path of player

                //Follow to interval 
                if (Counter7 >= 11) //Do not make is 11 or less!!!
                {
                    AIarrayPart++;
                    if (AIarrayPart >= 120)
                    {
                        AIarrayPart = 0;
                    }
                    Counter7 = 0;

                    //---Check if skip is needed---
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
                                if (AIarrayPart >= 120) //Check if is at end of array so it can start from beginning. Kinda like a special case
                                {
                                    AIarrayPart = 0;
                                    if (ArrayCount == 119 || PlayerPreviousPosition[AIarrayPart, 0] != PlayerPreviousPosition[119, 0] || PlayerPreviousPosition[AIarrayPart, 1] != PlayerPreviousPosition[119, 1])
                                    {
                                        break; //Exit loop
                                    }
                                }
                                else //non-special case / regular case
                                {
                                    if (AIarrayPart == ArrayCount + 1 || PlayerPreviousPosition[AIarrayPart, 0] != PlayerPreviousPosition[AIarrayPart - 1, 0] || PlayerPreviousPosition[AIarrayPart, 1] != PlayerPreviousPosition[AIarrayPart - 1, 1])
                                    {
                                        break; //Exit loop
                                    }
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
                                if (AIarrayPart >= 120) //Check if is at end of array so it can start from beginning. Kinda like a special case
                                {
                                    AIarrayPart = 0;
                                    if (ArrayCount == 119 || PlayerPreviousPosition[AIarrayPart, 0] != PlayerPreviousPosition[119, 0] || PlayerPreviousPosition[AIarrayPart, 1] != PlayerPreviousPosition[119, 1])
                                    {
                                        break; //Exit loop
                                    }
                                }
                                else //non-special case / regular case
                                {
                                    if (AIarrayPart == ArrayCount + 1 || PlayerPreviousPosition[AIarrayPart, 0] != PlayerPreviousPosition[AIarrayPart - 1, 0] || PlayerPreviousPosition[AIarrayPart, 1] != PlayerPreviousPosition[AIarrayPart - 1, 1])
                                    {
                                        break; //Exit loop
                                    }
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
            else if (FindPath() < 1.75) //If Player and AI collide
            {
                //Let Player go through AI, temporarily disable circle collider
                //(gameObject.GetComponent(typeof(Collider)) as Collider).isTrigger = true;
                CircleCollition.enabled = false;


                if (FollowOtherAI == true && Script_OtherAIObject.FindPath() > 1.75)
                {
                    //Do Nothing
                }
                else
                {
                    //Reset PreviousPlayerPosition Array
                    AIarrayPart = 0;
                    for (int i = 0; i < 120; i++)
                    {
                        PlayerPreviousPosition[i, 0] = Player.Body.position.x;
                        PlayerPreviousPosition[i, 1] = Player.Body.position.y;
                    }
                }
               
                

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
                Original_X = Body.position.x;
                Original_Y = Body.position.y;
                EventStepCase = 0;

                PreviousEventType = EventType;

                AI_Events.PreformEvent(Body, WalkSpeed, Anim, EventType, Original_X, Original_Y, EventStepCase);
            }

            #endregion
        }

        
        


    }
}
