using UnityEngine;
using System.Collections;

public class SlotsInteraction : MonoBehaviour {

    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //When user clicks on slot
        if (Input.GetMouseButtonDown(0))
        {
            print("Double Ouch!");
        }

    }
}
