using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Rigidbody2D Body; //Made this public static so that AI_Character can access it (Needs the x and y position of the player)
    Animator Anim;
    public int WalkSpeed;


	// Use this for initialization
	void Start () {

        Body = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
                                                         

	}
	
	// Update is called once per frame
	void Update () {

        //If i is pressed down on keyboard or Y in controller, open inventory
        if (Input.GetButtonDown("Button 3") == true)
        {
            InventoryScript.DisplayInventory();
        }


        //Movement
        float XMove = Input.GetAxis("Horizontal");
		float YMove = Input.GetAxis("Vertical");
        
        PlayerMovement.PlayerMove(Body,WalkSpeed,Anim,XMove, YMove);




	}
}
