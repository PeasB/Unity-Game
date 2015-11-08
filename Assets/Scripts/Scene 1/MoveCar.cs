using UnityEngine;

public class MoveCar : MonoBehaviour {

	public Vector2 MoveSpeed = new Vector2(); //Speed of object
    public bool CanDrive = false;
	
	// Update is called once per frame
	void Update () 
	{	
        if (CanDrive)
		    this.GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed.x,MoveSpeed.y); //move up by speed
	}



    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Respawn")
            this.GetComponent<Transform>().position = new Vector3(-5, -135);




    }
}
