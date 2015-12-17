//By: Ryan Dailey
using UnityEngine;
using UnityEngine.UI;

public class NavigationTabs : MonoBehaviour {

    private GameObject Inventory;
    private GameObject Crafting;
    private GameObject Pause;

    public void GoPauseGame()
    {
        //Show pause menu
        print("Pause");


        //Show
        Pause.SetActive(true);
        Inventory.SetActive(false);
        Crafting.SetActive(false);

    }

    public void GoCrafting()
    {
        //Show crafting menu
        print("Crafting");
        InventoryScript.DisplayCrafting();
        InventoryScript.DisplayInventoryInCraft();
        if (UI_Inventory.SelectedCraftingItem_ID != 0)
            InventoryScript.DisplayItemFormula(UI_Inventory.SelectedCraftingItem_ID);

        //Show
        Pause.SetActive(false);
        Inventory.SetActive(false);
        Crafting.SetActive(true);

    }

    public void GoInventory()
    {
        //Show inventory menu
        print("Inventory");
        InventoryScript.DisplayInventory();


        
        //Show
        Pause.SetActive(false);
        Inventory.SetActive(true);
        Crafting.SetActive(false);

    }

    public void DoCraftButton()
    {
        //Perform Craft
        print("Craft");
        if (UI_Inventory.SelectedCraftingItem_ID != 0 && UI_Inventory.CanCraft == true)
            InventoryScript.PerformCraft(UI_Inventory.SelectedCraftingItem_ID);
    }


    // Use this for initialization
    void Start () {
        Pause = GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Pause").gameObject;
        Crafting = GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Crafting").gameObject;
        Inventory = GameObject.Find("Inventory Canvas").transform.FindChild("Menu_Inventory").gameObject;


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
