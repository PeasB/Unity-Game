using UnityEngine;

public class SceneTransition : MonoBehaviour {


    public static string WhatScene = "";

    //Check for collition    
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            Application.LoadLevel("Scene 1");
        }
    }

    public static void GoToScene(string SceneName)
    {
        Application.LoadLevel(SceneName);
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
