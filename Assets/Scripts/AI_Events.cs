using UnityEngine;

public class AI_Events : MonoBehaviour {



    public static void PreformEvent (Rigidbody2D Body, int Speed, Animator Anim, int EventNumber, int X_Original, int Y_Original)
    {
        //Var for AI x and y
        float XMove = 0;
        float YMove = 0;
        

        if (EventNumber == 0) //Do nothing, lets the player push around the AI like a ragdoll
        {
            
        }
        else if (EventNumber == 1) //Walking up and down (down, up)
        {
            //Add cases

            //if (Body.position.y > Y_Original) //Down
            //{
            //    YMove = -1;
            //}
            //else if (Body.position.y < Y_Original - 60) // Up
            //{
            //    YMove = 1;
            //}            
        }
        else if (EventNumber == 2) //Walking in a L shape (left, up, down, right)
        {

        }
        else if (EventNumber == 3) //Walking in a square (right, down, left, up)
        {

        }
        else if (EventNumber == 4)
        {

        }
        else if (EventNumber == 5)
        {

        }
        else if (EventNumber == 6)
        {

        }



        //Execute Movement
        PlayerMovement.PlayerMove(Body, Speed, Anim, XMove, YMove);

    }

}
