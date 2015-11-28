﻿//By: Ryan Dailey
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class InteractionsItem : MonoBehaviour {

    
    private bool PressToPickUp = false;
    private int ItemID = 0;
    private string ItemName = "";
    private Text TextMessage;


    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "Player")
        {
            //---Get Item info---
            //Read in Items.xml
            XmlDocument ItemsDoc = new XmlDocument();
            ItemsDoc.Load("Assets/Scripts/ItemsPlus/Items.xml");

            foreach (XmlNode node in ItemsDoc.SelectNodes("Items/ItemsList/Item")) //Get info of ItemID
            {
                if (this.name.Contains(node.SelectSingleNode("ObjectName").InnerText) == true) //if (this.name == node.SelectSingleNode("ObjectName").InnerText)
                {
                    ItemID = int.Parse(node.SelectSingleNode("ID").InnerText);
                    ItemName = node.SelectSingleNode("Name").InnerText;
                    break;
                }
            }

            //---Show "press x to pick up item"---            
            TextMessage.text = "Press X to pick up " + ItemName; //<- Do Keyboard or xbox controller button
            TextMessage.gameObject.SetActive(true);
            
            

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
            TextMessage.gameObject.SetActive(false);
            TextMessage.text = "";

            //Reset Item info
            ItemID = 0;
            ItemName = "";

        }

    }




    // Use this for initialization
    void Start () {

        //---Check if user already picked up item in save data. If so, destory object right away. 
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/ItemPickUp/Scene"))
        {
            if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
            {
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    if (node1.InnerText == this.gameObject.name)
                    {
                        Destroy(this.gameObject);
                        break;
                    }
                }
                break;
            }
        }
        

        //Get text canvas
        TextMessage = GameObject.Find("Message_Canvas").transform.FindChild("Text").gameObject.GetComponent<Text>();
        

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown("Button 0") == true && PressToPickUp == true) //A on controller, x on keyboard
        {
            //---Pick up item---
            
            //Check if you have enough space in your inventory
            if (InventoryScript.InventorySpace() == true) //If there is enough space
            {
                //--disable pickup--
                PressToPickUp = false;

                //--Destroy object--
                Destroy(this.gameObject); 
                     
                //--Create item in inventory--
                InventoryScript.CreateItem(ItemID);

                //--Put into savedata not to pickup again--
                //Read in SaveGame.xml
                XmlDocument SaveGameDoc = new XmlDocument();
                SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                //Create new item in the players inventory
                XmlNode ObjectName = SaveGameDoc.CreateElement("ObjectName");
                ObjectName.InnerText = this.gameObject.name;
                                
                foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/ItemPickUp/Scene"))
                {
                    if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
                    {
                        node.AppendChild(ObjectName);
                        break;
                    }
                }

                //Save XML
                SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");


                //--Stop Showing pickup message--
                TextMessage.gameObject.SetActive(false);
                TextMessage.text = "";

                //--Reset item info--
                ItemID = 0;
                ItemName = "";                
                
                //--Play picked up item sound--             


            }
            else //Not Enough space
            {
                print("Error: Not enough space in inventory to pickup item");
                TextMessage.text = "Inventory is full too full!";
            }
        }

        


    }
}
