using UnityEngine;
using System.Collections;

public class InputTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        string Output = " ";

        if(Input.GetButtonDown("Button 0"))
        {
            Output = "Button 0";
        }                   
        else if (Input.GetButtonDown("Button 1"))
        {
            Output = "Button 1";
        }
        else if (Input.GetButtonDown("Button 2"))
        {
            Output = "Button 2";
        }
        else if (Input.GetButtonDown("Button 3"))
        {
            Output = "Button 3";
        }
        else if (Input.GetButtonDown("Button 6"))
        {
            Output = "Button 6";
        }
        else if (Input.GetButtonDown("Button 7"))
        {
            Output = "Button 7";
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            Output = "Vertical Axis";
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            Output = "Horizontal Axis";
        }

        if (Output != " ")
        {
            print(Output + " Moved/Pressed");
        }
    }
}
