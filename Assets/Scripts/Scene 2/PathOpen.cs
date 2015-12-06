using UnityEngine;
using System.Collections;

public class PathOpen : MonoBehaviour {

	// Update is called once per frame
	void Update () 
	{
		if (this.GetComponent<StoryPathChecker> ().ValueCheck ()) 
		{
			transform.GetComponent<BoxCollider2D>().enabled = false;
			Destroy(this.gameObject);
		}
	}
}
