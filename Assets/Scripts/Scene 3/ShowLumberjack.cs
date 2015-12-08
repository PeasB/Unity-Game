using UnityEngine;
using System.Collections;

public class ShowLumberjack : MonoBehaviour {
	
    public AudioClip SuspenseAudio;



	// Update is called once per frame
	void Update ()
    {
	    if(this.GetComponent<StoryPathChecker>().ValueCheck())
        {
            this.transform.FindChild("AI Lumberjack").gameObject.SetActive(true);
            this.transform.FindChild("EndTrigger").gameObject.SetActive(true);
            if(GameObject.Find("Main Camera").GetComponent<AudioSource>().clip != SuspenseAudio)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = SuspenseAudio;
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            }

        }



	}
}
