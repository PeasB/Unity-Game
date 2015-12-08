using UnityEngine;
using System.Collections;

public class KeyFound : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

        if(this.GetComponent<StoryPathChecker>().ValueCheck())
        {
            if(GameObject.Find("Area Event find key") != null)
            GameObject.Find("Area Event find key").GetComponent<AreaEvent>().CanReactivate = false;
        }

	
	}
}
