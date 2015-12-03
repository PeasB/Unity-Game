﻿//By: Ryan Dailey
using UnityEngine;

public class UI_Inventory : MonoBehaviour {

    //crafting menu
    public GameObject Canvas;
    public GameObject Inventory;
    public GameObject Crafting;
    public GameObject Map;
    public GameObject Dialogue;
    bool Paused = false;

    //Do a get set
    private static GameObject[] InventorySlots = new GameObject[30];

    private static GameObject[] InventoryInCraftingSlots = new GameObject[30];

    private static GameObject[] CraftingSlots = new GameObject[28];

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



    // Use this for initialization
    void Start ()
    {
        

        Canvas = GameObject.Find("Inventory Canvas");
        //Make if statement
        Canvas.gameObject.SetActive(false);
        //Inventory.gameObject.SetActive(false);
        //Dialogue.gameObject.SetActive(false);


        //---Crafting Cells---
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
        
      

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown("Button 3") == true) //If i is pressed down on keyboard or Y in controller, open inventory
        {
            if (Paused == true)
            {
                Time.timeScale = 1.0f;
                Canvas.gameObject.SetActive(false);
                Paused = false;
            }
            else
            {
                InventoryScript.DisplayCrafting();
                Time.timeScale = 0.0f;
                Canvas.gameObject.SetActive(true);
                Paused = true;
            }
        }
        else if (Input.GetButtonDown("Button 2") == true) //X on controller, q on keyboard
        {
            //InventoryScript.DisplayCrafting();
        }



    }
}
