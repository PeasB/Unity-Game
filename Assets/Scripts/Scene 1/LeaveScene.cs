using UnityEngine;
using System.Collections;
using System.Xml;

public class LeaveScene : MonoBehaviour {
	
    public CameraFollow MainCamera;
    public MoveCar Car;
    private bool TimerActive = false;
    public float Timer = 0f;
	
    // Update is called once per frame
    void Update () {
	

		if(GameObject.Find("Area Event Opening Conversation") == null)
        {
            MainCamera.Smoothing = new Vector2(0, 0);
            TimerActive = true;
        }

        if (TimerActive)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Car.CanDrive = false;
			SceneTransition.DoSceneTransition("Scene 2", 350, -983);
        }
            



	}
}
