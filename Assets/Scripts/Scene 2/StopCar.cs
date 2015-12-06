using UnityEngine;
using System.Collections;

public class StopCar : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Car")
        {
            Other.GetComponent<MoveCar>().CanDrive = false;
        }
    }


}
