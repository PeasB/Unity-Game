using UnityEngine;
using System.Collections;

public class WayBack : MonoBehaviour {
	
	// Update is called once per frame
	void Update () 
	{
	
		if (this.GetComponent<StoryPathChecker> ().ValueCheck ()) 
		{
			transform.FindChild("Wall").gameObject.SetActive(true);
			transform.FindChild("Noises Heard Conversation").gameObject.SetActive(true);
			transform.FindChild("Wall Blocking").gameObject.SetActive(false);
			this.gameObject.GetComponent<WayBack>().enabled = true;
		}


	}
}
