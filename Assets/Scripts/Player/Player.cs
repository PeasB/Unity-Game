﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Xml;

public class Player : MonoBehaviour {

    public static Rigidbody2D Body; //Made this public static so that AI_Character can access it (Needs the x and y position of the player)
    Animator Anim;
    public int WalkSpeed;

    
	bool FlashLightToggle = false;

    int CounterPos = 0; //After every 150 frames, write players x and y position to SaveData
    
    

    
    // Use this for initialization
    void Start () {

		//--Check for players position--
		//Read in SaveGame.xml
		XmlDocument SaveGameDoc = new XmlDocument();
		SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

		if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/X").InnerText != "" && SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/Y").InnerText != "")
		{
            //Set players x and y to what its saved in the save data
            this.GetComponent<Transform>().position = new Vector3(float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/X").InnerText), float.Parse(SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/Y").InnerText));
        } 


        //--Other--
        Body = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        
	
    }

	private void FlashlightRotation()
	{
		int Direction = Anim.GetInteger("Direction");

		if (Direction == 1 && this.transform.GetChild (0).transform.rotation.z != 90) //Right
		{ 
			this.transform.GetChild (0).transform.position = new Vector3 (this.GetComponent<Transform> ().position.x + 2.2f, this.GetComponent<Transform> ().position.y - 0.3f, -5);
			this.transform.GetChild (0).transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 90);
			                                                               	
		} else if (Direction == 0 && this.transform.GetChild (0).transform.rotation.z != 0) //Down
		{
			this.transform.GetChild (0).transform.position = new Vector3 (this.GetComponent<Transform> ().position.x, this.GetComponent<Transform> ().position.y - 2.4f, -5);
			this.transform.GetChild (0).transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0);
		}
		else if(Direction == 2 && this.transform.GetChild(0).transform.rotation.z != 180) //Up 
		{
			this.transform.GetChild (0).transform.position = new Vector3 (this.GetComponent<Transform> ().position.x, this.GetComponent<Transform> ().position.y + 2.4f, -5);
			this.transform.GetChild (0).transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 180);
		}
		else if(Direction == 3 && this.transform.GetChild(0).transform.rotation.z != 270) //Left
		{
			this.transform.GetChild (0).transform.position = new Vector3 (this.GetComponent<Transform> ().position.x -2.2f , this.GetComponent<Transform> ().position.y - 0.3f , -5);
			this.transform.GetChild (0).transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 270);
		}
	}


    // Update is called once per frame
    void Update () {

		if(Input.GetButtonDown("Button 4") && InventoryScript.CheckItem(12))
		{
			if(!FlashLightToggle)
			{
				this.transform.GetChild (0).gameObject.SetActive(true);
				FlashLightToggle = true;
			}
			else
			{
				this.transform.GetChild (0).gameObject.SetActive(false);
				FlashLightToggle = false;
			}


		}

		if(FlashLightToggle)
			FlashlightRotation();

        

        if (Input.GetButtonDown("Button 0") == true) //A on controller, x on keyboard
        {
            
        }
        else if (Input.GetButtonDown("Button 3") == true) //If i is pressed down on keyboard or Y in controller, open inventory
        {
            //pause / unpause   <--------********Causes conflict if the canvas is not in the scene********** - canvas will be in every scene
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
            
            //InventoryScript.DisplayInventory();
        }
        else if (Input.GetButtonDown("Button 2") == true) //X on controller, q on keyboard
        {
           // InventoryScript.DisplayCrafting();
        }
        else if (Input.GetButtonDown("Button 6") == true) //Back on controller, p on keyboard
        {            
            //InventoryScript.PerformCraft(5);
        }


        //Check if you need to write player position to save data
        CounterPos++;
        if (CounterPos >= 150)
        {
            //Read in SaveGame.xml
            XmlDocument SaveGameDoc = new XmlDocument();
            SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

            SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/X").InnerText = transform.position.x.ToString();
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/Y").InnerText = transform.position.y.ToString();

            //Save XML
            DoSaveGame.UpdateSaveData(SaveGameDoc);

            CounterPos = 0;
        }



        //Movement
        float XMove = Input.GetAxis("Horizontal");
		float YMove = Input.GetAxis("Vertical");
        
        PlayerMovement.PlayerMove(Body,WalkSpeed,Anim,XMove, YMove);


	}

  
}
