//By: Ryan Dailey
using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class InventoryScript : MonoBehaviour {

    //Y value is 4 (ID, Name, Description, Stacks, picturePath).
    static string[,] InventoryBox;    

    //2D array for items to craft (Find X base on how much craftable items there are. Y value is 5 (ID_Main, Name, Description, PicPath, CanCraft)  (ID1, ID2, ID3, ID4, ID5 was removed from array to make array size smaller)
    static string[,] CraftingTable;



    public static void DisplayInventory(bool CraftInventory) //<--Make so it either displays in Items menu or crafting menu. False = show in inventory
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Read in Items.xml
        XmlDocument ItemsDoc = new XmlDocument();
        ItemsDoc.Load("Assets/Scripts/ItemsPlus/Items.xml");

        //Fetch current items ID in SaveGame.xml (SaveGameDoc) and store in a list. (Which is a dynamic array)
        List<int> InventoryItemsID = new List<int>();
        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
        {
            InventoryItemsID.Add(int.Parse(node.InnerText));
        }
        
        //Loop each item in array and compare with Items.xml (ItemsDoc) to get full info of Item. Then store in 2D Array
        InventoryBox = new string[36, 5]; //X value is 36, because the max amount of items you can carry is 36.  Y value is 4 (ID, Name, Description, Stacks, picturePath).
        int InventoryCount = 0;

        for (int i = 0; i < InventoryItemsID.Count; i++)
        {
            int ItemID = InventoryItemsID[i];
            string ItemName = "?"; //Will soon be filled
            string ItemDescription = "?"; //Will soon be filled
            string PicturePath = "?"; //Will soon be filled
            bool Stackable = false; //Could change to true
            
            foreach (XmlNode node in ItemsDoc.SelectNodes("Items/ItemsList/Item")) //Get info of ItemID
            {
                if (ItemID == int.Parse(node.SelectSingleNode("ID").InnerText)) //If it matches the item ID
                {
                    ItemName = node.SelectSingleNode("Name").InnerText;
                    ItemDescription = node.SelectSingleNode("Description").InnerText;
                    PicturePath = node.SelectSingleNode("PicPath").InnerText;
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
                InventoryBox[InventoryCount, 4] = PicturePath;

                InventoryCount++; //Go on to the next item slot
            }
            else //Check if it can stack on other items
            {
                
                bool FirstStackableItem = true; //If this stays true, a item that can be stackable will be created for the very first time. This can be changed to false, if the item already exists and stacks on the matching item
                
                for (int k = 0; k < InventoryBox.GetLength(0); k++) //Loop through 32 items to see if it can stack on to any items
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
                    InventoryBox[InventoryCount, 4] = PicturePath;

                    InventoryCount++; //Go on to the next item slot
                }
                
            }
            
        }


        //------------------------Display into UI---------------------------
        //Display array info into the UI (Display the items in the players inventory)

        if (CraftInventory == false) //Display inventory in the main inventory tab
        {

        }
        else if (CraftInventory == true) //Display inventory in the inventory section in the crafting tab
        {

        }
        //Print it out until Andrew finishes inventory UI
        for (int i = 0; i < InventoryBox.GetLength(0); i++)
        {
            if (InventoryBox[i, 1] != null) //Makes it only print the items that are in the players inventory
            {
                print(InventoryBox[i, 1] + " " + InventoryBox[i, 3]);
            }
        }
        print("----------------------");


    }

    public static void DisplayCrafting() 
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Read in Items.xml
        XmlDocument ItemsDoc = new XmlDocument();
        ItemsDoc.Load("Assets/Scripts/ItemsPlus/Items.xml");

        //Fetch current items ID in SaveGame.xml (SaveGameDoc) and store in a list. (Which is a dynamic array)
        List<int> InventoryItemsID = new List<int>();
        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
        {
            InventoryItemsID.Add(int.Parse(node.InnerText));
        }


        //Loop through how many craftable items and make this the x value for the 2D array
        int CraftableItemsCount = 0;
        foreach (XmlNode node in ItemsDoc.SelectNodes("Items/Crafting/Craft"))
        {
            CraftableItemsCount++;
        }

        //2D array for items to craft (Find X base on how much craftable items there are. Y value is 9 (ID_Main, Name, Description, PicPath, CanCraft)  (ID1, ID2, ID3, ID4, ID5 was removed from array to make array size smaller)
        CraftingTable = new string[CraftableItemsCount, 5];

        int iCounter = 0;

        foreach (XmlNode node in ItemsDoc.SelectNodes("Items/Crafting/Craft"))
        {
            
            //Var
            int Main_ID = int.Parse(node.SelectSingleNode("NewID").InnerText); //Main ID
            string Main_Name = "?"; //Will soon be filled
            string Main_Description = "?"; //Will soon be filled
            string Main_PicturePath = "?"; //Will soon be filled
            bool CanCraft = true; //Can change to false if the player does not have the required items to craft

            //------------------------Gathering Data------------------------------------

            //Up to this point, the New_ID is found and stored    

            //Now find the Item info based on the Main_ID and store it
            foreach (XmlNode x_Node in ItemsDoc.SelectNodes("Items/ItemsList/Item")) //Get info of ItemID
            {
                if (Main_ID == int.Parse(x_Node.SelectSingleNode("ID").InnerText)) //If it matches the item ID
                {
                    //Store info into array
                    Main_Name = x_Node.SelectSingleNode("Name").InnerText; //Store Name
                    Main_Description = x_Node.SelectSingleNode("Description").InnerText; //Store Description
                    Main_PicturePath = x_Node.SelectSingleNode("PicPath").InnerText; //Store Picture Path
                    break;
                }
            }

            //-------------------------Ingredients-------------------------------------


            //make int, -1 for default, can change to another number. -1 means don't use
            //these var are used to see if you have the required items to craft new item.
            int Ingredient_ID1 = -1;
            int Ingredient_ID2 = -1;
            int Ingredient_ID3 = -1;
            int Ingredient_ID4 = -1;
            int Ingredient_ID5 = -1;

            //Check how many ingredients there are
            int IngredientID = 1;

            //Get Ingredient ID's
            foreach (XmlNode subnode in node.ChildNodes)
            {
                if (subnode.Name != "NewID") //Check if its not the last child
                {    

                    if (IngredientID == 1)
                    {
                        Ingredient_ID1 = int.Parse(subnode.InnerText); //First Ingredient ID
                    }
                    else if (IngredientID == 2)
                    {
                        Ingredient_ID2 = int.Parse(subnode.InnerText); //Second Ingredient ID
                    }
                    else if (IngredientID == 3)
                    {
                        Ingredient_ID3 = int.Parse(subnode.InnerText); //Third Ingredient ID
                    }
                    else if (IngredientID == 4)
                    {
                        Ingredient_ID4 = int.Parse(subnode.InnerText); //Fourth Ingredient ID
                    }
                    else if (IngredientID == 5)
                    {
                        Ingredient_ID5 = int.Parse(subnode.InnerText); //Fifth Ingredient ID
                    }

                    IngredientID++;                    
                }
            }

            //Find if you have the required ingredients to craft (Compare each ingredient with your inventory). Overall, this will set CanCraft to either true or false

            //Loop through your current items

            bool IngredientOne_Exists = false;
            bool IngredientTwo_Exists = false;
            bool IngredientThree_Exists = false;
            bool IngredientFour_Exists = false;
            bool IngredientFive_Exists = false;

            foreach (XmlNode nodeItem in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
            {
                if (Ingredient_ID1 != -1 && int.Parse(nodeItem.InnerText) == Ingredient_ID1 && IngredientOne_Exists == false)
                    IngredientOne_Exists = true;

                else if (Ingredient_ID2 != -1 && int.Parse(nodeItem.InnerText) == Ingredient_ID2 && IngredientTwo_Exists == false)
                    IngredientTwo_Exists = true;

                else if (Ingredient_ID3 != -1 && int.Parse(nodeItem.InnerText) == Ingredient_ID3 && IngredientThree_Exists == false)
                    IngredientThree_Exists = true;

                else if (Ingredient_ID4 != -1 && int.Parse(nodeItem.InnerText) == Ingredient_ID4 && IngredientFour_Exists == false)
                    IngredientFour_Exists = true;

                else if (Ingredient_ID5 != -1 && int.Parse(nodeItem.InnerText) == Ingredient_ID5 && IngredientFive_Exists == false)
                    IngredientFive_Exists = true;
            }
            
            

            //Checks if you don't have the required items to craft
            if (Ingredient_ID1 != -1 && IngredientOne_Exists == false || Ingredient_ID2 != -1 && IngredientTwo_Exists == false || Ingredient_ID3 != -1 && IngredientThree_Exists == false || Ingredient_ID4 != -1 && IngredientFour_Exists == false || Ingredient_ID5 != -1 && IngredientFive_Exists == false)
            {
                CanCraft = false;
            }
            


            //------------------------Input Data into Array---------------------------------------


            //Input var into array
            CraftingTable[iCounter, 1] = Main_Name;
            CraftingTable[iCounter, 2] = Main_Description;
            CraftingTable[iCounter, 3] = Main_PicturePath;
            if (CanCraft == true)
                CraftingTable[iCounter, 4] = "1";
            else
                CraftingTable[iCounter, 4] = "0";


            //-----------------------------------------------------------------------

            

            //Increment Counter---------------------------
            iCounter++;
        }

        //========================Display into UI==============================

        //Loop finnished and array is complete. Now input data from the array into the UI

        //Print until UI is finished
        for (int i = 0; i < CraftingTable.GetLength(0); i++)
        {
            print("Item: " + CraftingTable[i, 1] + "         " + "Can you Craft it?: " + CraftingTable[i, 4]);
        }
        print("----------------------");








    }
        
    public static void DisplayItemFormula(int SelectedCraftID) 
    {
        //Use the array that DisplayCrafting nicely constructed and donated to us :)

        //var
        string[,] IngredientIDs = new string[5, 5]; //X: 5 is max number of ingredients(which item). Y is 5 (ID, Name, Description, PicPath, HasItem)
        int NumOfIngredients = 0; //Start at defult 0 (Dont change)
        //var of New_ID (ID to be crafted)
        string SC_Name = "?";
        string SC_Description = "?";
        string SC_PicPath = "?";
        bool SC_CanCraft = false;

        //Match info from SC_ID (SelectedCraftID)
        for (int i = 0; i < CraftingTable.GetLength(0); i++)
        {
            if (CraftingTable[i, 0] == SelectedCraftID.ToString())
            {
                SC_Name = CraftingTable[i, 1];
                SC_Description = CraftingTable[i, 2];
                SC_PicPath = CraftingTable[i, 3];
                if (CraftingTable[i, 4] == "1")
                {
                    SC_CanCraft = true;
                }
                break;
            }
        }

        //--------------------------------------------------------------------------

        //Find out how many ingredients there are for this Craft Item so you know how many ingredient boxs to display on UI
        //Do a count on how many ingredients in item, then Store IngredientID's in array.
               
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Read in Items.xml
        XmlDocument ItemsDoc = new XmlDocument();
        ItemsDoc.Load("Assets/Scripts/ItemsPlus/Items.xml");

        foreach (XmlNode node in ItemsDoc.SelectNodes("Items/Crafting/Craft"))
        {
            if (node.SelectSingleNode("NewID").InnerText == SelectedCraftID.ToString()) // IF Matched ID
            {
                //Get Ingredient ID's
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Name != "NewID") //Check if its not the last child
                    {
                        NumOfIngredients++;
                        if (NumOfIngredients == 1)
                        {
                            IngredientIDs[NumOfIngredients - 1, 0] = subnode.InnerText; //First Ingredient ID
                        }
                        else if (NumOfIngredients == 2)
                        {
                            IngredientIDs[NumOfIngredients - 1, 0] = subnode.InnerText; //Second Ingredient ID
                        }
                        else if (NumOfIngredients == 3)
                        {
                            IngredientIDs[NumOfIngredients - 1, 0] = subnode.InnerText; //Third Ingredient ID
                        }
                        else if (NumOfIngredients == 4)
                        {
                            IngredientIDs[NumOfIngredients - 1, 0] = subnode.InnerText; //Fourth Ingredient ID
                        }
                        else if (NumOfIngredients == 5)
                        {
                            IngredientIDs[NumOfIngredients - 1, 0] = subnode.InnerText; //Fifth Ingredient ID
                        }                        
                    }
                }
                break;
            }
        }


        //--------------------------------------------------------------------------

        //If SC_CanCraft == false, Check what items are missing so you darken the missing item on the ui.
        //If SC_CanCraft == true, skip all since no item pics need to be darken since you have all the required items.
        //Also Find the rest of the info of ingredients (like name, description, picpath, ect)

        
            for (int i = 0; i < NumOfIngredients; i++) //Find what ingredient is missing
            {

                //Find other info
                foreach (XmlNode node in ItemsDoc.SelectNodes("Items/ItemsList/Item")) //Get info of ItemID
                {
                    if (IngredientIDs[i, 0] == node.SelectSingleNode("ID").InnerText)
                    {
                        IngredientIDs[i, 1] = node.SelectSingleNode("Name").InnerText; //Name 
                        IngredientIDs[i, 2] = node.SelectSingleNode("Description").InnerText; //Descripition
                        IngredientIDs[i, 3] = node.SelectSingleNode("PicPath").InnerText; //PicPath
                    }
                }

                if (SC_CanCraft == false) //you dont have all required items, so see if you own this selected ingredient
                {
                    foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
                    {
                        if (IngredientIDs[i, 0] == node.InnerText)
                        {
                            IngredientIDs[i, 4] = "1"; //Has item
                            break;
                        }
                    }

                    if (IngredientIDs[i, 4] != "1") //If no item match 
                        IngredientIDs[i, 4] = "0"; 

                }
                else //true (you already have all items
                {
                    IngredientIDs[i, 4] = "1"; //HasItem
                }
           }

       //--------------------------------------------------------------------------

       //========================Display into UI==============================
       print("Babatunde");

       //Given: SelectedCraft_ID, SC_CanCraft, SC_Description, SC_PicPath, and all that for each ingredient in the IngredientID array




    }

    public static void PerformCraft(int SelectedCraftID) //Do craft
    {

        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Read in Items.xml
        XmlDocument ItemsDoc = new XmlDocument();
        ItemsDoc.Load("Assets/Scripts/ItemsPlus/Items.xml");

        //Var
        int[] IngredientIDs = new int[5];
        int NumOfIngredients = 0; //Start at defult 0 (Dont change)

        //Get all the IngredientIDs and store into array
        foreach (XmlNode node in ItemsDoc.SelectNodes("Items/Crafting/Craft"))
        {
            if (node.SelectSingleNode("NewID").InnerText == SelectedCraftID.ToString()) // IF Matched ID
            {
                //Get Ingredient ID's
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Name != "NewID") //Check if its not the last child
                    {
                        NumOfIngredients++;
                        if (NumOfIngredients == 1)
                        {
                            IngredientIDs[NumOfIngredients - 1] = int.Parse(subnode.InnerText); //First Ingredient ID
                        }
                        else if (NumOfIngredients == 2)
                        {
                            IngredientIDs[NumOfIngredients - 1] = int.Parse(subnode.InnerText); //Second Ingredient ID
                        }
                        else if (NumOfIngredients == 3)
                        {
                            IngredientIDs[NumOfIngredients - 1] = int.Parse(subnode.InnerText); //Third Ingredient ID
                        }
                        else if (NumOfIngredients == 4)
                        {
                            IngredientIDs[NumOfIngredients - 1] = int.Parse(subnode.InnerText); //Fourth Ingredient ID
                        }
                        else if (NumOfIngredients == 5)
                        {
                            IngredientIDs[NumOfIngredients - 1] = int.Parse(subnode.InnerText); //Fifth Ingredient ID
                        }                        
                    }
                }
                break;
            }
        }

        int NumOfCanCraft = 0; //The number of items that can be crafted. Check if it compares to the NumOfIngredients. If yes, preform craft

        //Do a check if user has the required ingredient items
        for (int i = 0; i < NumOfIngredients; i++) //Find what ingredient is missing
        {
            foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
            {
                if (IngredientIDs[i].ToString() == node.InnerText)
                {
                    NumOfCanCraft++;                    
                    break;
                }
            }
        }
        
        //if pass, craft!!!
        if (NumOfIngredients == NumOfCanCraft)
        {
            //Create Item
            CreateItem(SelectedCraftID);

            //Delete ingredient items from the player inventory
            for (int i = 0; i < NumOfIngredients; i++)
            {
                DeleteItem(IngredientIDs[i]); //<-Fix this so 0 doesn't get deleted
            }
            
            //refresh inventory by calling in a DisplayInventory(true)
            DisplayInventory(true);
            ////refresh display crafting by calling it
            //DisplayCrafting();
            ////refresh itemformula by calling it
            //DisplayItemFormula(SelectedCraftID);
        }
        else
        {
            //Error!!! Cannot craft because player does not have required items
            print("Error: Cannot craft! Player does not have the required ingredients");
        }

       

    }

    public static void CreateItem(int SelectedItemID)
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Create new item in the players inventory
        XmlNode Item = SaveGameDoc.CreateElement("ItemID");
        Item.InnerText = SelectedItemID.ToString();
        SaveGameDoc.DocumentElement.SelectSingleNode("Inventory").AppendChild(Item);
        //Save XML
        SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");
    }

    public static void DeleteItem(int SelectedItemID)
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Check if Item exists / find item
        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/Inventory/ItemID"))
        {
            if (SelectedItemID.ToString() == node.InnerText)
            {
                //Item found. Now delete item. Goodbye :)
                node.ParentNode.RemoveChild(node);
                //Save XML
                SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");
                break;
            }
        }

    }

    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
