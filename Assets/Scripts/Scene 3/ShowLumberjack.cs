using UnityEngine;
using System.Collections;

public class ShowLumberjack : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
	    if(this.GetComponent<StoryPathChecker>().ValueCheck())
        {
            this.transform.FindChild("Ai Lumberjack").gameObject.SetActive(true);
            this.transform.FindChild("EndTrigger").gameObject.SetActive(true);
        }



	}
}
