using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Rigidbody2D Body; //Made this public static so that AI_Character can access it (Needs the x and y position of the player)
    Animator Anim;
    public int WalkSpeed;

    //crafting menu
    public GameObject Canvas;
    //public GameObject Camera;
    bool Paused = false;


    // Use this for initialization
    void Start () {

        Body = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        //Canvas.gameObject.SetActive(false); <--------------**********Causes conflict if the canvas is not in the scene****************

    }


    // Update is called once per frame
    void Update () {

        //If i is pressed down on keyboard or Y in controller, open inventory
        if (Input.GetButtonDown("Button 3") == true)
        {
                ////pause / unpause   <--------********Causes conflict if the canvas is not in the scene**********
                //if (Paused == true)
                //{
                //    Time.timeScale = 1.0f;
                //    Canvas.gameObject.SetActive(false);
                //    Paused = false;
                //}
                //else
                //{
                //    Time.timeScale = 0.0f;
                //    Canvas.gameObject.SetActive(true);
                //    Paused = true;

                //}
                
                //
                
                InventoryScript.DisplayInventory(false); //true inputs into crafting inventory, false inputs into inventory tab
        }
        else if (Input.GetButtonDown("Button 2") == true) //If o is pressed down on keyboard or X in controller, open inventory
        {
            InventoryScript.DisplayCrafting();
        }
        else if (Input.GetButtonDown("Button 6") == true) //If bksp is pressed down on keyboard or Back in controller, penis
        {
            //InventoryScript.DisplayItemFormula(5);
            InventoryScript.PerformCraft(5);
        }

        //Movement
        float XMove = Input.GetAxis("Horizontal");
		float YMove = Input.GetAxis("Vertical");
        
        PlayerMovement.PlayerMove(Body,WalkSpeed,Anim,XMove, YMove);


	}
}
