using UnityEngine;
using System.Collections;

public class UI_Inventory : MonoBehaviour {

    //crafting menu
    public GameObject Canvas;
    public GameObject Inventory;
    public GameObject Crafting;
    public GameObject Map;
    public GameObject Dialogue;
    bool Paused = false;


    public GameObject[] InventorySlots = new GameObject[30];

    public GameObject[] InventoryInCraftingSlots = new GameObject[30];

    public GameObject[] CraftingSlots = new GameObject[28];

   
    // Use this for initialization
    void Start ()
    {
        Canvas = GameObject.Find("Inventory Canvas");
        //Make if statement
        Canvas.gameObject.SetActive(false);
        //Inventory.gameObject.SetActive(false);
        //Dialogue.gameObject.SetActive(false);

        
        int Row = 1;
        int Coloum = 1;

        for (int i = 0; i < 30; i++)
        {

            //InventorySlots[i] = GameObject.Find("Inventory Canvas/Menu_Crafting/Slot " + Row + "," + Coloum);
            InventorySlots[i] = GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Crafting").gameObject.transform.FindChild("Slot " + Row + "," + Coloum).gameObject;
            Coloum++;
            if (Coloum == 5)
            {
                Coloum = 1;
                Row++;
            }
        }

              
        if (InventorySlots[29] == null)
        {
            print("WTF");
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
                Time.timeScale = 0.0f;
                Canvas.gameObject.SetActive(true);
                Paused = true;
            }
        }



    }
}
