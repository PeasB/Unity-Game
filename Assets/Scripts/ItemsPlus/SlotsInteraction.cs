using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotsInteraction : MonoBehaviour {
       
    
    public void InventorySlot(GameObject WhatSlot)
    {
       
        #region Inventory Slot

        //Get Item Info
        for (int i = 0; i < UI_Inventory.GetInventorySlots.Length; i++)
        {
            if (UI_Inventory.GetInventorySlots[i] == WhatSlot) //this.gameObject
            {
                int Item_ID = int.Parse(InventoryScript.GetInventoryBox[i, 0]);
                string Item_Name = InventoryScript.GetInventoryBox[i, 1];
                string Item_Description = InventoryScript.GetInventoryBox[i, 2];
                int Item_Stacks = int.Parse(InventoryScript.GetInventoryBox[i, 3]);

                //Selected Item
                UI_Inventory.SelectedInventoryItem_ID = Item_ID;

                //Display Item Name, Description, bigger picture, and Stack (count)
                //-Item Name-
                GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Inventory").transform.FindChild("Text_ItemName").gameObject.GetComponent<Text>().text = Item_Name; //===NEW
                //-Description-
                GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Inventory").transform.FindChild("Text_ItemDescription").gameObject.GetComponent<Text>().text = Item_Description; //===NEW
                //-Bigger Picture-
                GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Inventory").transform.FindChild("Item Slot").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(InventoryScript.GetInventoryBox[i, 4]); //===NEW




                break;
            }
        }

        #endregion

    }
    
    public void CraftingSlot(GameObject WhatSlot)
    {
       
        #region Crafting Slot

        //Get Item Info
        for (int i = 0; i < UI_Inventory.GetCraftingSlots.Length; i++)
        {
            if (UI_Inventory.GetCraftingSlots[i] == WhatSlot) //this.gameObject
            {
                int Item_ID = int.Parse(InventoryScript.GetCraftingTable[i, 0]);
                string Item_Name = InventoryScript.GetCraftingTable[i, 1];
                string Item_Description = InventoryScript.GetCraftingTable[i, 2];
                int Item_CanCraft = int.Parse(InventoryScript.GetCraftingTable[i, 4]);
                
                
                //Selected Item
                UI_Inventory.SelectedCraftingItem_ID = Item_ID;

                //Show Formula
                InventoryScript.DisplayItemFormula(Item_ID);

                //Show text under slot (for a future update)


                //If CanCraft is false, disable craft button
                if (Item_CanCraft == 0)
                    GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Crafting").transform.FindChild("btnCraft").gameObject.SetActive(false); //====NEW
                else
                    GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Crafting").transform.FindChild("btnCraft").gameObject.SetActive(true); //====NEW


                //Get item formula (which will display the images of the items in the formula slots)
                InventoryScript.DisplayItemFormula(Item_ID);


                break;
            }
        }

        #endregion

    }



    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {


    //    //When user clicks on slot
    //    if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == true)
    //    {


    //        if (TheSlotType == SlotType.Inventory)
    //        {
    //            #region Inventory Slot

    //            //Get Item Info
    //            for (int i = 0; i < UI_Inventory.GetInventorySlots.Length; i++)
    //            {
    //                if (UI_Inventory.GetInventorySlots[i] == this.gameObject)
    //                {
    //                    int Item_ID = int.Parse(InventoryScript.GetInventoryBox[i, 0]);
    //                    string Item_Name = InventoryScript.GetInventoryBox[i, 1];
    //                    string Item_Description = InventoryScript.GetInventoryBox[i, 2];
    //                    int Item_Stacks = int.Parse(InventoryScript.GetInventoryBox[i, 3]);

    //                    //Selected Item
    //                    UI_Inventory.SelectedInventoryItem_ID = Item_ID;

    //                    //Display Item Name, Description, bigger picture, and Stack (count)




    //                    break;
    //                }
    //            }

    //            #endregion
    //        }
    //        else if (TheSlotType == SlotType.InventoryInCrafting)
    //        {
    //            #region InventoryInCrafting Slot

    //            #endregion
    //        }
    //        else if (TheSlotType == SlotType.Crafting)
    //        {
    //            #region Crafting Slot

    //            //Get Item Info
    //            for (int i = 0; i < UI_Inventory.GetCraftingSlots.Length; i++)
    //            {
    //                if (UI_Inventory.GetCraftingSlots[i] == this.gameObject)
    //                {
    //                    int Item_ID = int.Parse(InventoryScript.GetCraftingTable[i, 0]);
    //                    string Item_Name = InventoryScript.GetCraftingTable[i, 1];
    //                    string Item_Description = InventoryScript.GetCraftingTable[i, 2];
    //                    int Item_CanCraft = int.Parse(InventoryScript.GetCraftingTable[i, 4]);

    //                    //Selected Item
    //                    UI_Inventory.SelectedCraftingItem_ID = Item_ID;

    //                    //Show text under slot (for a future update)


    //                    //If CanCraft is false, disable craft button


    //                    //Get item formula (which will display the images of the items in the formula slots)
    //                    InventoryScript.DisplayItemFormula(Item_ID);


    //                    break;
    //                }
    //            }



    //            #endregion
    //        }
    //        else if (TheSlotType == SlotType.Formula)
    //        {
    //            #region Formula Slot

    //            #endregion
    //        }




    //    }

    }
}
