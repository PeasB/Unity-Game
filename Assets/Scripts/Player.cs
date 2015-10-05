using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Rigidbody2D Body;
    Transform Position;
    Animator Anim;
    public int WalkSpeed;


	// Use this for initialization
	void Start () {

        Body = GetComponent<Rigidbody2D>();
        Position = GetComponent<Transform>();
        Anim = GetComponent<Animator>();
                                                         

	}
	
	// Update is called once per frame
	void Update () {

        PlayerMovement.PlayerMove(Body,WalkSpeed,Anim);


	}
}
