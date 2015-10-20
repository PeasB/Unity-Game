﻿using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class InventoryScript : MonoBehaviour {

    


    public static void DisplayInventory()
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Read in Items.xml
        XmlDocument ItemsDoc = new XmlDocument();
        ItemsDoc.Load("Assets/Scripts/Items.xml");

        //Fetch current items ID in SaveGame.xml (SaveGameDoc) and store in a list. (Which is a dynamic array)
        List<int> InventoryItemsID = new List<int>();
        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
        {
            InventoryItemsID.Add(int.Parse(node.InnerText));
        }
        
        //Loop each item in array and compare with Items.xml (ItemsDoc) to get full info of Item. Then store in 2D Array
        string[,] InventoryBox = new string[32, 4]; //X value is 32, because the max amount of items you can carry is 32.  Y value is 4 (ID, Name, Description, Stacks).
        int InventoryCount = 0;

        for (int i = 0; i < InventoryItemsID.Count; i++)
        {
            int ItemID = InventoryItemsID[i];
            string ItemName = "?"; //Will soon be filled
            string ItemDescription = "?"; //Will soon be filled
            bool Stackable = false; //Could change to true

            foreach (XmlNode node in ItemsDoc.SelectNodes("Items/ItemsList/Item")) //Get info of ItemID
            {
                if (ItemID == int.Parse(node.SelectSingleNode("ID").InnerText)) //If it matches the item ID
                {
                    ItemName = node.SelectSingleNode("Name").InnerText;
                    ItemDescription = node.SelectSingleNode("Description").InnerText;
                    if (int.Parse(node.SelectSingleNode("Stackable").InnerText) == 1)
                    {
                        Stackable = true;
                    }
                }
            }

            if (i == 0 || Stackable == false) //First item. Does not need to check if it can stack on other items since it is the first item. OR if item has initial property that disables stacking.
            {
                InventoryBox[InventoryCount, 0] = ItemID.ToString();
                InventoryBox[InventoryCount, 1] = ItemName;
                InventoryBox[InventoryCount, 2] = ItemDescription;
                InventoryBox[InventoryCount, 3] = "1"; //Since it is unstackable, keep the stack at 1, which is the single item itself.

                InventoryCount++; //Go on to the next item slot
            }
            else //Check if it can stack on other items
            {
                bool FirstStackableItem = true; //If this stays true, a item that can be stackable will be created for the very first time. This can be changed to false, if the item already exists and stacks on the matching item

                for (int k = 0; k < InventoryBox.GetLength(0); i++) //Loop through 32 items to see if it can stack on to any items
                {         
                    if (InventoryBox[k, 0] == ItemID.ToString() && Stackable == true) //There is a stackable item that already exists that matches your item. Stack ontop of the item
                    {
                        InventoryBox[k, 3] = (int.Parse(InventoryBox[k, 3]) + 1).ToString(); //Add 1 to stack
                        FirstStackableItem = false; //This was not the First Stackable Item

                        break;
                    }
                }
                if (FirstStackableItem == true) //First time adding a stackable item (Stack will be 1, since it is the first item)
                {
                    InventoryBox[InventoryCount, 0] = ItemID.ToString();
                    InventoryBox[InventoryCount, 1] = ItemName;
                    InventoryBox[InventoryCount, 2] = ItemDescription;
                    InventoryBox[InventoryCount, 3] = "1"; //Since it is the first stackable item
                }
            }
        }

        
        //Display this info into the UI (Display the items in the players inventory)
        //Print it out until Andrew finishes inventory UI
        



    }

    public static void Crafting()
    {

    }





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
