//By: Ryan Dailey
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
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.StationaryWithDir;
                        Body.GetComponent<CircleCollider2D>().enabled = true;

                        //Read in SaveGame.xml
                        XmlDocument xSaveGameDoc = new XmlDocument();
                        xSaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                        //Find current scene then find ScenePart. 
                        foreach (XmlNode Xnode in xSaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
                        {
                            if (Application.loadedLevelName == Xnode.SelectSingleNode("SceneName").InnerText)
                            {
                                Xnode.SelectSingleNode("AI/JoshAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/JoshAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/JoshAI/Action").InnerText = "";
                                Xnode.SelectSingleNode("AI/JoshAI/EventType").InnerText = "";
                                Xnode.SelectSingleNode("AI/JoshAI/CaseStep").InnerText = "";

                                break;
                            }
                        }

                        //Save XML
                        xSaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

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
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.StationaryWithDir;
                        Body.GetComponent<CircleCollider2D>().enabled = true;

                        //Read in SaveGame.xml
                        XmlDocument xSaveGameDoc = new XmlDocument();
                        xSaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                        //Find current scene then find ScenePart. 
                        foreach (XmlNode Xnode in xSaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
                        {
                            if (Application.loadedLevelName == Xnode.SelectSingleNode("SceneName").InnerText)
                            {
                                Xnode.SelectSingleNode("AI/MattAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/MattAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/MattAI/Action").InnerText = "";
                                Xnode.SelectSingleNode("AI/MattAI/EventType").InnerText = "";
                                Xnode.SelectSingleNode("AI/MattAI/CaseStep").InnerText = "";
                            }
                        }

                        //Save XML
                        xSaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

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
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.StationaryWithDir;
                        Body.GetComponent<CircleCollider2D>().enabled = true;

                        //Read in SaveGame.xml
                        XmlDocument xSaveGameDoc = new XmlDocument();
                        xSaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                        //Find current scene then find ScenePart. 
                        foreach (XmlNode Xnode in xSaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
                        {
                            if (Application.loadedLevelName == Xnode.SelectSingleNode("SceneName").InnerText)
                            {
                                Xnode.SelectSingleNode("AI/KateAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/KateAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/KateAI/Action").InnerText = "";
                                Xnode.SelectSingleNode("AI/KateAI/EventType").InnerText = "";
                                Xnode.SelectSingleNode("AI/KateAI/CaseStep").InnerText = "";
                            }
                        }

                        //Save XML
                        xSaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

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
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.StationaryWithDir;
                        Body.GetComponent<CircleCollider2D>().enabled = true;

                        //Read in SaveGame.xml
                        XmlDocument xSaveGameDoc = new XmlDocument();
                        xSaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                        //Find current scene then find ScenePart. 
                        foreach (XmlNode Xnode in xSaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
                        {
                            if (Application.loadedLevelName == Xnode.SelectSingleNode("SceneName").InnerText)
                            {
                                Xnode.SelectSingleNode("AI/AprilAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/AprilAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/AprilAI/Action").InnerText = "";
                                Xnode.SelectSingleNode("AI/AprilAI/EventType").InnerText = "";
                                Xnode.SelectSingleNode("AI/AprilAI/CaseStep").InnerText = "";
                            }
                        }

                        //Save XML
                        xSaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

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
                            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.StationaryWithDir;
                        Body.GetComponent<CircleCollider2D>().enabled = true;

                        //Read in SaveGame.xml
                        XmlDocument xSaveGameDoc = new XmlDocument();
                        xSaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                        //Find current scene then find ScenePart. 
                        foreach (XmlNode Xnode in xSaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
                        {
                            if (Application.loadedLevelName == Xnode.SelectSingleNode("SceneName").InnerText)
                            {
                                Xnode.SelectSingleNode("AI/EthanAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/EthanAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/EthanAI/Action").InnerText = "";
                                Xnode.SelectSingleNode("AI/EthanAI/EventType").InnerText = "";
                                Xnode.SelectSingleNode("AI/EthanAI/CaseStep").InnerText = "";
                            }
                        }

                        //Save XML
                        xSaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

                    }
                                                
                    }


                    if (XMove == 0 && YMove == 0)
                    {
                        CaseStep++;

                    //---Set values for save data---
                    //Read in SaveGame.xml
                    XmlDocument xSaveGameDoc = new XmlDocument();
                    xSaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                    //Find current scene then find ScenePart. 
                    foreach (XmlNode Xnode in SaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
                    {
                        if (Application.loadedLevelName == Xnode.SelectSingleNode("SceneName").InnerText)
                        {

                            //Update xml
                            if (Body.name == "AI Josh")
                            {
                                Xnode.SelectSingleNode("AI/JoshAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/JoshAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/JoshAI/Action").InnerText = "Event";
                                Xnode.SelectSingleNode("AI/JoshAI/EventType").InnerText = EventNumber.ToString();
                                Xnode.SelectSingleNode("AI/JoshAI/CaseStep").InnerText = CaseStep.ToString();
                            }
                            else if (Body.name == "AI Matt")
                            {
                                Xnode.SelectSingleNode("AI/MattAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/MattAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/MattAI/Action").InnerText = "Event";
                                Xnode.SelectSingleNode("AI/MattAI/EventType").InnerText = EventNumber.ToString();
                                Xnode.SelectSingleNode("AI/MattAI/CaseStep").InnerText = CaseStep.ToString();
                            }
                            else if (Body.name == "AI Kate")
                            {
                                Xnode.SelectSingleNode("AI/KateAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/KateAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/KateAI/Action").InnerText = "Event";
                                Xnode.SelectSingleNode("AI/KateAI/EventType").InnerText = EventNumber.ToString();
                                Xnode.SelectSingleNode("AI/KateAI/CaseStep").InnerText = CaseStep.ToString();
                            }
                            else if (Body.name == "AI April")
                            {
                                Xnode.SelectSingleNode("AI/AprilAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/AprilAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/AprilAI/Action").InnerText = "Event";
                                Xnode.SelectSingleNode("AI/AprilAI/EventType").InnerText = EventNumber.ToString();
                                Xnode.SelectSingleNode("AI/AprilAI/CaseStep").InnerText = CaseStep.ToString();
                            }
                            else if (Body.name == "AI Ethan")
                            {
                                Xnode.SelectSingleNode("AI/EthanAI/X").InnerText = Body.position.x.ToString();
                                Xnode.SelectSingleNode("AI/EthanAI/Y").InnerText = Body.position.y.ToString();
                                Xnode.SelectSingleNode("AI/EthanAI/Action").InnerText = "Event";
                                Xnode.SelectSingleNode("AI/EthanAI/EventType").InnerText = EventNumber.ToString();
                                Xnode.SelectSingleNode("AI/EthanAI/CaseStep").InnerText = CaseStep.ToString();
                            }
                        }
                    }               

                    //Save XML
                    xSaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

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
