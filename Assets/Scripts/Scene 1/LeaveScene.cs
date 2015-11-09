using UnityEngine;
using System.Collections;

public class LeaveScene : MonoBehaviour {


    public CameraFollow MainCamera;
    public MoveCar Car;
    private bool TimerActive = false;
    public float Timer = 0f;

    // Update is called once per frame
    void Update () {
	
        if(!this.GetComponent<AreaEvent>().ConversationInstance.IsActive && this.GetComponent<AreaEvent>().ConversationInstance.HasRun)
        {
            MainCamera.Smoothing = new Vector2(0, 0);
            TimerActive = true;
        }

        if (TimerActive)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Car.CanDrive = false;
            Application.LoadLevel("Scene 2");
        }
            



	}
}
