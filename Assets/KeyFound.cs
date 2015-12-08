using UnityEngine;
using System.Collections;

public class KeyFound : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

        if(this.GetComponent<StoryPathChecker>().ValueCheck())
        {
            GameObject.Find("Area Event find key").GetComponent<AreaEvent>().CanReactivate = false;
        }

	
	}
}
