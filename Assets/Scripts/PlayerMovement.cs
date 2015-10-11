﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    public static void PlayerMove (Rigidbody2D Body, int Speed, Animator Anim, float XMove,float YMove)
    {
		//Var Movements

		//Execute Movement of Person
        Body.velocity = new Vector2(XMove* Speed, YMove*Speed);

        //Animation of Movement
        if (XMove != 0 || YMove != 0)
        {
            if (XMove < 0)
            {
                Anim.SetInteger("Direction", 3);
                Anim.SetBool("Moving", true);
            }
            else if (XMove > 0)
            {
                Anim.SetInteger("Direction", 1);
                Anim.SetBool("Moving", true);
            }
            else if (YMove > 0)
            {
                Anim.SetInteger("Direction", 2);
                Anim.SetBool("Moving", true);
            }
            else if (YMove < 0)
            {
                Anim.SetInteger("Direction", 0);
                Anim.SetBool("Moving", true);
            }

        }
        else
        {
            Anim.SetBool("Moving", false);
        }




    }

}
