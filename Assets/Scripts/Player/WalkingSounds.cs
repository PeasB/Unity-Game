using UnityEngine;
using System.Collections;

public class WalkingSounds : MonoBehaviour {

	string CurrentSurface;


	void OnTriggerEnter2D(Collider2D Other)
	{
		/*
		if(CurrentSurface != Other.tag)

		AudioClip Audio;

		switch (Other.tag) {
		
		case "Ground Dirt":
			
		
		
		} */


	}

	private bool IsFloor(string Tag)
	{
		/*
		//Ground
		if (Tag.Substring(0, 6)) 
		{
			
		
		} */

		return true;


	}
	



}
