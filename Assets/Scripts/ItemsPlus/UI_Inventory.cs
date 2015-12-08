﻿//By: Ryan Dailey
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inventory : MonoBehaviour {


    //crafting menu
    private GameObject Canvas;
    public GameObject Inventory;
    public GameObject Crafting;
    //public GameObject Map;
    //public GameObject Dialogue;
    bool Paused = false;

    //Selected Item
    public static int SelectedInventoryItem_ID = 0; //0 = no item selected
    public static int SelectedCraftingItem_ID = 0; //0 = no item selected
    public static bool CanCraft = false;

    //Do a get set
    private static GameObject[] InventorySlots = new GameObject[30];

    private static GameObject[] InventoryInCraftingSlots = new GameObject[30];

    private static GameObject[] CraftingSlots = new GameObject[28];

    private static GameObject[] FormluaSlots = new GameObject[5];

    public static GameObject[] GetCraftingSlots
    {
        get
        {
            return CraftingSlots;
        }
    }

    public static GameObject[] GetInventorySlots
    {
        get
        {
            return InventorySlots;
        }
    }

    public static GameObject[] GetInventoryInCraftingSlots
    {
        get
        {
            return InventoryInCraftingSlots;
        }
    }

    public static GameObject[] GetFormulaSlots
    {
        get { return FormluaSlots; }
    }


    // Use this for initialization
    void Start()
    {



        Canvas = GameObject.Find("Inventory Canvas");
        //Make if statement
        Canvas.gameObject.SetActive(false);
        //Inventory.gameObject.SetActive(false);
        //Dialogue.gameObject.SetActive(false);


        //---Crafting Cells---
        #region Crafting cells
        int Row = 1;
        int Column = 1;

        for (int i = 0; i < 28; i++)
        {
            CraftingSlots[i] = Canvas.transform.Find("Menu_Crafting").gameObject.transform.Find("Slot " + Row + "," + Column).gameObject;
            Column++;
            if (Column == 5)
            {
                Column = 1;
                Row++;
            }
        }
        #endregion

        //---Inventory in crafting cells---
        #region Inventory in crafting cells
        Row = 1;
        Column = 1;

        for (int i = 0; i < 30; i++)
        {
            InventoryInCraftingSlots[i] = Canvas.transform.Find("Menu_Crafting").gameObject.transform.Find("ItemSlot " + Row + "," + Column).gameObject;
            Column++;
            if (Column == 7)
            {
                Column = 1;
                Row++;
            }
        }
        #endregion

        //---Inventory cells---
        #region Inventory Cells
        Row = 1;
        Column = 1;

        for (int i = 0; i < 30; i++)
        {
            InventorySlots[i] = Canvas.transform.Find("Menu_Inventory").gameObject.transform.Find("iSlot " + Row + "," + Column).gameObject;
            Column++;
            if (Column == 6)
            {
                Column = 1;
                Row++;
            }
        }
        #endregion

        //---Formula cells---
        #region Formula cells

        for (int i = 0; i < 5; i++)
        {
            FormluaSlots[i] = Canvas.transform.Find("Menu_Crafting").gameObject.transform.Find("Formula" + (i + 1)).gameObject;
        }

        #endregion


    }

    // Update is called once per frame
    void Update()
    {
        //===
        //if (Input.GetButtonDown("Button 3") == true) //If i is pressed down on keyboard or Y in controller, open inventory
        //{
        //    if (Paused == true)
        //    {
        //        Time.timeScale = 1.0f;
        //        Canvas.gameObject.SetActive(false);
        //        Paused = false;
        //    }
        //    else
        //    {
        //        InventoryScript.DisplayInventory();
        //        InventoryScript.DisplayCrafting();
        //        InventoryScript.DisplayInventoryInCraft();
        //        InventoryScript.DisplayItemFormula(9);
        //        Time.timeScale = 0.0f;
        //        Canvas.gameObject.SetActive(true);
        //        Paused = true;
        //    }
        //}
        //else if (Input.GetButtonDown("Button 2") == true) //X on controller, q on keyboard
        //{
        //    //InventoryScript.DisplayCrafting();
        //}




    }
}
