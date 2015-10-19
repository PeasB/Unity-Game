using UnityEngine;
using System.Collections;
//Generic Camer to follow anything
public class CameraFollow: MonoBehaviour {
	
	public Transform Player;

	public Vector2 
		Margin, //used for deadspace
		Smoothing; //how fast the camera moves

	public BoxCollider2D Bound;

	private Vector3 
		_Min, // border min (bottom left)
		_Max; // border max (top right)

	public bool IsFollowing { get; set;}

	public void Start()
	{
		_Min = Bound.bounds.min; // bottom left corner of border
		_Max = Bound.bounds.max; //bottom right corner of border
		IsFollowing = true; //set to false if you don't want the camera to follow
	}

	public void Update()
	{
		var X = transform.position.x; //get x value of camera
		var Y = transform.position.y; //get y value of camera

		if (IsFollowing)
		{
			if(Mathf.Abs(X - Player.position.x) > Margin.x) // if current object is outside of the x deadspace, set a line to follow over time			 			 
				X = Mathf.Lerp(X, Player.position.x, Smoothing.x * Time.deltaTime);

			if(Mathf.Abs( Y - Player.position.y) > Margin.y)// if current object is outside of the y deadspace, set a line to follow over tme
				Y = Mathf.Lerp(Y, Player.position.y, Smoothing.y * Time.deltaTime);		
		}

		var CameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height); //half of Camera length(x)
		X = Mathf.Clamp(X, _Min.x + CameraHalfWidth, _Max.x - CameraHalfWidth);//keep x values inside the border 
		Y = Mathf.Clamp(Y, _Min.y + GetComponent<Camera>().orthographicSize, _Max.y - GetComponent<Camera>().orthographicSize); //keep y values in border
		transform.position = new Vector3(X,Y,transform.position.z); //move the camera to object			
	}

}
