//By: Ryan Dailey
using UnityEngine;

public class InteractionsItem : MonoBehaviour {

    
    bool PressToPickUp = false;

    int ItemID = 2;
    string ItemName = "";

    


    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "Player")
        {
            //Get Item info



            //Show "press x to pick up item"




            //enable pickup
            PressToPickUp = true;
        }
    }
    

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "Player")
        {
            //disable pickup
            PressToPickUp = false;



            //Stop showing "press x to pick up item"





            //Reset Item info




        }

    }




    // Use this for initialization
    void Start () {

        
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Button 0") == true && PressToPickUp == true) //A on controller, x on keyboard
        {
            //Pick up item
            
            //Check if you have enough space in your inventory
            if (InventoryScript.InventorySpace() == true) //If there is enough space
            {
                //disable pickup
                PressToPickUp = false;

                //Destroy object


                //Create item in inventory
                InventoryScript.CreateItem(ItemID);

                //Stop Showing pickup message


                //Reset item info
                ItemID = 0;
                ItemName = "";
                
                //Play picked up item sound               


            }
            else //Not Enough space
            {
                print("Error: Not enough space in inventory to pickup item");

            }



            
        }

        


        }
}
