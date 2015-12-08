//By: Ryan Dailey
using UnityEngine;
using UnityEngine.UI;

public class NavigationTabs : MonoBehaviour {
    


    public void GoPauseGame()
    {
        //Show pause menu
        print("Pause");
    }

    public void GoCrafting()
    {
        //Show crafting menu
        print("Crafting");
        InventoryScript.DisplayCrafting();
        InventoryScript.DisplayInventoryInCraft();
        if (UI_Inventory.SelectedCraftingItem_ID != 0)
            InventoryScript.DisplayItemFormula(UI_Inventory.SelectedCraftingItem_ID);
    }

    public void GoInventory()
    {
        //Show inventory menu
        print("Inventory");
        InventoryScript.DisplayInventory();
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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
