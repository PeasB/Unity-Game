using UnityEngine;
using UnityEngine.EventSystems; 
using System.Collections;

public class InventoryNavigation : MonoBehaviour {

    public GameObject Inventory;
    public GameObject Crafting;

    public void ToInventory()
    {
        Inventory.gameObject.SetActive(true);
        Crafting.gameObject.SetActive(false);

    }
}
