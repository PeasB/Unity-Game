using UnityEngine;
using System.Collections;

public class WhichOutdoors : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
	    if(this.GetComponent<StoryPathChecker>().ValueCheck())
        {
            this.GetComponent<SceneTransition>().WhatScene = "Scene 3 Outside";
        }



	}
}
