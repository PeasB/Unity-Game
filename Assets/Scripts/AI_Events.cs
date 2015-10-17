using UnityEngine;

public class AI_Events : MonoBehaviour {

    

    public static int PreformEvent (Rigidbody2D Body, int Speed, Animator Anim, int EventNumber, int X_Original, int Y_Original, int CaseStep)
    {
        //Var for AI x and y
        float XMove = 0;
        float YMove = 0;
        
        //Think about making this an Enumerator
        if (EventNumber == 0) //Do nothing and stay still
        {
            
        }
        else if (EventNumber == 1) //Walking up and down (down, up). Has 2 cases
        {
            #region Event 1: Up and Down
            if (CaseStep == 0) //Case going down
            {
                if (Body.position.y > Y_Original - 60) //Down
                {
                    YMove = -1;
                }
                else
                    CaseStep = 1; //Switch Cases
            }
            else if (CaseStep == 1) //Case going up
            {
                if (Body.position.y < Y_Original) // Up
                {
                    YMove = 1;
                }
                else
                    CaseStep = 0; //Switch Cases
            }
            #endregion
        }
        else if (EventNumber == 2) //Walking in a L shape (left, up, down, right). Has 4 cases
        {
            #region Event 2: L-Shape

            if (CaseStep == 0) //Case going down
            {
                if (Body.position.y > Y_Original - 60) //Down
                {
                    YMove = -1;
                }
                else
                    CaseStep = 1; //Swicth Cases
            }
            else if (CaseStep == 1) //Case going right
            {
                if (Body.position.x < X_Original + 30) //Right
                {
                    XMove = 1;
                }
                else
                    CaseStep = 2; //Swicth Cases
            }
            else if (CaseStep == 2) //Case going left
            {
                if (Body.position.x > X_Original) //Left
                {
                    XMove = -1;
                }
                else
                    CaseStep = 3; //Swicth Cases
            }
            else if (CaseStep == 3) //Case going up
            {
                if (Body.position.y < Y_Original) // Up
                {
                    YMove = 1;
                }
                else
                    CaseStep = 0; //Switch Cases
            }

            #endregion
        }
        else if (EventNumber == 3) //Walking in a square Clock wise (right, down, left, up). Has 4 cases
        {
            #region Event 3: Square CW

            if (CaseStep == 0) //Case going right
            {
                if (Body.position.x < X_Original + 30) //Right
                {
                    XMove = 1;
                }
                else
                    CaseStep = 1;
            }
            else if (CaseStep == 1) //Case going down
            {
                if (Body.position.y > Y_Original - 30) //Down
                {
                    YMove = -1;
                }
                else
                    CaseStep = 2;
            }
            else if (CaseStep == 2) //Case going left
            {
                if (Body.position.x > X_Original) //Left
                {
                    XMove = -1;
                }
                else
                    CaseStep = 3;
            }
            else if (CaseStep == 3) //Case going up
            {
                if (Body.position.y < Y_Original) // Up
                {
                    YMove = 1;
                }
                else
                    CaseStep = 0;
            }
            
            #endregion
        }
        else if (EventNumber == 4)
        {
            #region Event 4: Sqaure CCW

            if (CaseStep == 0) //Case going Down
            {
                if (Body.position.y > Y_Original - 30) //Down
                {
                    YMove = -1;
                }
                else
                    CaseStep = 1;
            }
            else if (CaseStep == 1) //Case going right
            {                
                if (Body.position.x < X_Original + 30) //Right
                {
                    XMove = 1;
                }
                else
                    CaseStep = 2;
            }
            else if (CaseStep == 2) //Case going up
            {
                if (Body.position.y < Y_Original) // Up
                {
                    YMove = 1;
                }
                else
                    CaseStep = 3;
            }
            else if (CaseStep == 3) //Case going left
            {               
                if (Body.position.x > X_Original) //Left
                {
                    XMove = -1;
                }
                else
                    CaseStep = 0;
            }

            #endregion
        }
        else if (EventNumber == 5)
        {
            #region Event 5: 

            

            #endregion
        }
        else if (EventNumber == 6)
        {

        }
        else if (EventNumber == 7)
        {

        }
        else if (EventNumber == 8)
        {

        }
        else if (EventNumber == 9)
        {

        }
        else if (EventNumber == 10)
        {

        }



        //Execute Movement
        PlayerMovement.PlayerMove(Body, Speed, Anim, XMove, YMove);

        return CaseStep;

    }

}
