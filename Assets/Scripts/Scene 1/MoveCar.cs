using UnityEngine;

public class MoveCar : MonoBehaviour {

	public Vector2 MoveSpeed; //Speed of object

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{	
		var X = transform.position.x; // get truck x
		var Y = transform.position.y; // get truck y	
		 

		transform.position = new Vector3 (X, Y + MoveSpeed.y,transform.position.z); //move up by speed
	}
}
