﻿//By: Ryan Dailey
using UnityEngine;
using System.Xml;

public class AI_Events : MonoBehaviour {

    

    public static int PreformEvent (Rigidbody2D Body, int Speed, Animator Anim, int EventNumber, double X_Original, double Y_Original, int CaseStep)
    {
        //SavePosition(Body.gameObject.name);  //<---- For saving an AI's location

        //Var for AI x and y
        float XMove = 0;
        float YMove = 0;

        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/AI/CutSceneEvents.xml");

        //Find current scene then find ScenePart. 
        foreach (XmlNode node in SaveGameDoc.SelectNodes("CutSceneEvents/Scenes/Scene"))
        {
            if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
            {
               
                    if (Body.name == "AI Josh")
                    {
                        if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/Y").InnerText != "")
                        {
                            XMove = Find_X_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/X").InnerText));
                            YMove = Find_Y_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/Y").InnerText));
                        }
                        else
                        {
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Stationary;
                            print("Done");
                        }
                                                                        
                    }
                    else if (Body.name == "AI Matt")
                    {
                        if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/Y").InnerText != "")
                        {
                            XMove = Find_X_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/X").InnerText));
                            YMove = Find_Y_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/Y").InnerText));
                        }
                        else
                        {
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Stationary;
                            print("Done");
                        }                                                

                    }
                    else if (Body.name == "AI Kate")
                    {
                        if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/Y").InnerText != "")
                        {
                            XMove = Find_X_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/X").InnerText));
                            YMove = Find_Y_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/Y").InnerText));
                        }
                        else
                        {
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Stationary;
                            print("Done");
                        }
                                                
                    }
                    else if (Body.name == "AI April")
                    {
                        if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/Y").InnerText != "")
                        {
                            XMove = Find_X_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/X").InnerText));
                            YMove = Find_Y_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/Y").InnerText));
                        }
                        else
                        {
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Stationary;
                            print("Done");
                        }
                        
                    }
                    else if (Body.name == "AI Ethan")
                    {
                        if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/Y").InnerText != "")
                        {
                            XMove = Find_X_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/X").InnerText));
                            YMove = Find_Y_Distance(Body, double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/Y").InnerText));
                        }
                        else
                        {
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Stationary;
                            print("Done");
                        }
                                                
                    }


                    if (XMove == 0 && YMove == 0)
                    {
                        CaseStep++;
                    }

                

                break;
            }
        }

        

        //Execute Movement
        PlayerMovement.PlayerMove(Body, Speed, Anim, XMove, YMove);

        return CaseStep;

    }

    private static int Find_X_Distance(Rigidbody2D Body, double xPosition)
    {
        double X_Distance;

        X_Distance = xPosition - Body.position.x; 


        int X_Direction = 0;

        if (X_Distance < -0.05)
        {
            X_Direction = -1;
        }
        else if (X_Distance > 0.05)
        {
            X_Direction = 1;
        }

        return X_Direction;
    }

    private static int Find_Y_Distance(Rigidbody2D Body, double yPosition)
    {
        double Y_Distance;

        Y_Distance = yPosition - Body.position.y; 


        int Y_Direction = 0;

        if (Y_Distance < -0.05)
        {
            Y_Direction = -1;
        }
        else if (Y_Distance > 0.05)
        {
            Y_Direction = 1;
        }

        return Y_Direction;
    }




    private static void SavePosition(string AI_Name)
    {

    }

}
