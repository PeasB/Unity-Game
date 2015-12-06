using UnityEngine;
using System.Collections;

public class DefaultDirection : MonoBehaviour {

	public enum Direction //Direction Values.
	{
		Up = 2,
		Down = 0,
		Left = 3,
		Right = 1
	}
	
	public Direction ObjectDirection;

	// Use this for initialization
	void Start () {
	
		//Set the Direction in the Animator.
		this.GetComponent<Animator> ().SetInteger ("Direction", (int)ObjectDirection);


	}

}
