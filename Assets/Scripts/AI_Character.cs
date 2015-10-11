using UnityEngine;

public class AI_Character : MonoBehaviour {

	//Global Var
	Rigidbody2D Body;
	Animator Anim;
	public int WalkSpeed;



	// Use this for initialization
	void Start () {

		Body = GetComponent<Rigidbody2D>();
		Anim = GetComponent<Animator>();
        

	}
	
    private int FindPath() //Just testing stuff out
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


	// Update is called once per frame
	void Update () {

        //Var for x and y
        float XMove = 0;
        float YMove = 0;

        if (FindPath() > 30) //If AI far away from Player, go to the player
        {
            if (Body.position.x != (int)Player.Body.position.x)
            {
                XMove = Find_X_Distance();
            }

            if (Body.position.y != (int)Player.Body.position.y)
            {
                YMove = Find_Y_Distance();
            }

            PlayerMovement.PlayerMove(Body, WalkSpeed, Anim, XMove, YMove);            
        }
        else //Stop Moving
        {
            PlayerMovement.PlayerMove(Body, WalkSpeed, Anim, XMove, YMove);
        }


	}
}
