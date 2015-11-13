using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Player : MonoBehaviour {

    public static Rigidbody2D Body; //Made this public static so that AI_Character can access it (Needs the x and y position of the player)
    Animator Anim;
    public int WalkSpeed;

    //crafting menu
    public GameObject Canvas;
    public GameObject Inventory;
    public GameObject Crafting;
    public GameObject Map;
    public GameObject Dialogue;
    bool Paused = false;


    // Use this for initialization
    void Start () {

        Body = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        Canvas.gameObject.SetActive(false); //<--------------**********Causes conflict if the canvas is not in the scene**************** - canvas will be in every scene <- Not main menu
        Inventory.gameObject.SetActive(false);
        Dialogue.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update () {

        //If i is pressed down on keyboard or Y in controller, open inventory
        if (Input.GetButtonDown("Button 3") == true)
        {
            ////pause / unpause   <--------********Causes conflict if the canvas is not in the scene********** - canvas will be in every scene
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


            //KEEP THESE 3 IMPORTANT LINES!!!!!
            //print(AES_Crypto.DecryptText("lCb2+ZYYCIKjgc0Jxw5KL2i28oyvCYuca7tFg3iohp9TgfbHzp5VkuiKKCY3kzwYkoTIfXkz+uC3DoJ+9hmkTZuhjjMZdoZ5QU8ppV/xEQokkStJwm2Ngki8Pepo1zWwW3z4Bmd3l5Vt0NuY+DWjQCwWhclbU4q/Cy5CIYligWSzs6QeJNskUx1I77NDWHnqjv/It7szqZQ5m2BD9LNqDvNDOuDgdUL0e2dx0Wp+8zx/aJx7RbRK4RNFFQ1RTY5f+MGdaG4nMmrgAcqmuFZI1SF2lGm3rFs6c1dNd34u/cFCvZruzpFnc7lruxG/wz8WQuncSF9HYuctLIuU077fOayuIRy7ocI4CuXQPVudtXLtgy1gE86mGtFuCzIPh4vGdZEveQThQ2dZVOsZzESIIIv2mA9bbqAEz4+g5g87a1fuqbqCWbfeFGJ6r1yNIy45NaiGIaLrAVCJvV7VdBSqbXwGoXS66enHruyJHD7j3H/RfL6JMFbBRYzsry0WmubMGzCshun4K6hgMxhEWQaM7AGwIAGw/B1R2dAv1IkEyaHiFSu47H3ZAD/wlHIMWu8i2NiOICSkvI494y9eJiVqJQunTdfExuVkUGCHMAX5DCD6NQCM2IqLScJ42fnzQ3Ua+uaRNZr/H/NXJY9Iw0HkfKLlAyhP23yvosXKq800FBiP0TwK7/CdW/2oxEKZjK/NsBGSVHGt4cw9VJ/HrbyHNoGRn7qecl5+aRLmUGpPJ9J8e3PtjOV1DgpWAqzi/jA3HPZL2sDawCJLKi0FNN25aTk7dTsYUHw9mm36YckhNpp/YHik6BhH4KoXQf5VH2eR9PxO8kZkTIgqM9B/Nyhd9orQZlasOk7q3rdI9IeiJ2Mo37y3LcbaU2OgZFSnTR7KLUwRxUYRZQnemWqGYVqL8d9fbXvL4o92E4QYao3s8cCj8nnPTgHgRe/g3yvO1z4LMhCTmb+tNirg/O0nniGCoQwb5DKauQcyPXJ4k0pEis9VQF2otb8seLlC88DpQNv6fWUGsVQCUS1pXdc2Wpucwe8y5OCYQGJkCWbLc8ISdefny2yHr0HOGuUa6qfMtRCX5kKbusnnnBBeYJPQaCSHIg4LTn4LjwsI+e1ugvD4D50bw7FnX0qukWO1DH7VvtXrCRmZeNcZ/cGFnIxA68gsMcCF3c9qEh4HXQqM0otke0pmxv9ZPkiy19DHeHZ7na4+4HwtqPgMeMVSAblp9BJc13M6+D1zA14vpBnNS2jznorp7q2mAPUSN55+bAxaEHyjBxlvzkebjo+nKKE2Aw3OSlvQwLZcDK1fWulCUNdzbUwnF61qfOJNxwQHO68vszvVru7IwHRhuW4sXTzyh+wGxT6uOUbKfCsYCNMNv5i04oeS/ukpaVIee87QSXfHrGcpvsilP8In2YuXh1BU1LsHGyt1CbIawFjynSHKaXanr68XRWBhJa6LjUW39pvfO8s0UlSUQ0uYA9MbzTn7262BqwbaUdbTefYsnrOcHUN0kDJSmkWPpZKt9H4lfQoc0g8nxEMRCqn+9fNQD4/Y36ktM9X0sxm8u94OCZOb6o1h6pBKpCIqGcN0MeLutxOsAYvRnWQ2iFf7JTTCerT8RBVx6DNPLdnF0XO23SGwG0E5Kz+dtmChir5pINd8Yr3bbQlo"));
            //print(DoSaveGame.FetchSaveData());
            //InventoryScript.CreateItem(10);
            
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

    public void ToInventory()
    {

        Inventory.gameObject.SetActive(true);
        Crafting.gameObject.SetActive(false);

    }

    public void Test()
    {
        Debug.Log("Button was pressed");
    }

}
