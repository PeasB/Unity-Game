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
        //Find best possible path to character, one frame at a time
        
        //Find Distance from AI to Player using distance formula
        //Body.position.x & Body.position.y is the x and y position of the AI
        //Player.Body.position.x & Player.Body.position.y is the x and y position of the Player
        int Distance = (int)Mathf.Round(Mathf.Sqrt(Mathf.Pow((Body.position.x - (int)Player.Body.position.x), 2) + Mathf.Pow((Body.position.y - (int)Player.Body.position.y), 2)));

        
        return Distance; //Hey Method, are you happy now that you finally get a return value?
    }

    private int Find_X_Distance()
    {
        int X_Distance = 0;


            

        
        return X_Distance;
    }


	// Update is called once per frame
	void Update () {
	
        if (FindPath() > 40) //If AI far away from Player, go to the player
        {


        }


		float XMove = 0;
		float YMove = 0;

		PlayerMovement.PlayerMove (Body, WalkSpeed, Anim, XMove, YMove);

	}
}
