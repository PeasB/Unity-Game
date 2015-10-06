using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

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

		float XMove = Input.GetAxis("Horizontal");
		float YMove = Input.GetAxis("Vertical");


        PlayerMovement.PlayerMove(Body,WalkSpeed,Anim,XMove, YMove);




	}
}
