using UnityEngine;
using System.Collections;

public class LowerMusic : MonoBehaviour {

    public AudioSource MainCameraAudio;
    private bool CarEntered = false;
    public float AudioLowerSpeed = 0f;

    void OnTriggerEnter2D()
    {
        CarEntered = true;
                                  
    }

    void Update()
    {
        if (CarEntered && MainCameraAudio.volume > 0.4)
            MainCameraAudio.volume -= AudioLowerSpeed;
    }



}
