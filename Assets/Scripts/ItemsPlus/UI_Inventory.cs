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



    // Use this for initialization
    void Start ()
    {
        Canvas.gameObject.SetActive(false); 
        //Inventory.gameObject.SetActive(false);
        //Dialogue.gameObject.SetActive(false);
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
