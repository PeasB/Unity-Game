using UnityEngine;
using System.Collections;

public class LeaveScene : MonoBehaviour {


    public CameraFollow MainCamera;
    public MoveCar Car;

	// Update is called once per frame
	void Update () {
	
        if(!this.GetComponent<AreaEvent>().ConversationInstance.IsActive && this.GetComponent<AreaEvent>().ConversationInstance.HasRun)
        {
            MainCamera.Smoothing = new Vector2(0, 0);
            Application.LoadLevel("Scene 2");
        }


	}
}
