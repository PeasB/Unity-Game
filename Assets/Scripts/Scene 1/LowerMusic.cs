using UnityEngine;
using System.Collections;

public class LowerMusic : MonoBehaviour {

    public AudioSource MainCameraAudio;
    private bool Triggered = false;
    public float Min;
    public float AudioLowerSpeed = 0f;

    void OnTriggerEnter2D()
    {
        Triggered = true;
                                  
    }

    void Update()
    {
        if (Triggered && MainCameraAudio.volume > Min)
            MainCameraAudio.volume -= AudioLowerSpeed;
    }

}
