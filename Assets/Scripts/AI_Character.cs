using UnityEngine;
using System.Collections;

public class AI_Character : MonoBehaviour {

	//Global Var
	Rigidbody2D Body;
	Animator Anim;
	public int WalkSpeed;



	// Use this for initialization
	void Start () {

		Body = GetComponent<Rigidbody2D>();
		Anim = GetComponent<Animator>();


	}
	
	// Update is called once per frame
	void Update () {
	
		float XMove = 0;
		float YMove = 0;

		PlayerMovement.PlayerMove (Body, WalkSpeed, Anim, XMove, YMove);

	}
}
