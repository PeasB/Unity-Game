//By: Ryan Dailey
using UnityEngine;
using System.Xml;

public class AI_Events : MonoBehaviour {

    private double X_Cutscene;
    private double Y_Cutscene;

    public double Get_X_Cutscene
    {
        get
        {
            return X_Cutscene;
        }
    }

    public double Get_Y_Cutscene
    {
        get
        {
            return Y_Cutscene;
        }
    }

    public void GetLocation(Rigidbody2D Body, int EventNumber, int CaseStep)
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.LoadXml(Resources.Load("CutScenes/CutSceneEvents").ToString());
        //Find current scene then find ScenePart. 
        foreach (XmlNode node in SaveGameDoc.SelectNodes("CutSceneEvents/Scenes/Scene"))
        {
            if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
            {
                if (Body.name == "AI Josh")
                {
                    if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/Y").InnerText != "")
                    {
                        X_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/X").InnerText);
                        Y_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/JoshAI/Y").InnerText);
                    }
                    else
                    {
                        X_Cutscene = 0;
                        Y_Cutscene = 0;
                    }
                }
                else if (Body.name == "AI Matt")
                {
                    if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/Y").InnerText != "")
                    {
                        X_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/X").InnerText);
                        Y_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/MattAI/Y").InnerText);
                    }
                    else
                    {
                        X_Cutscene = 0;
                        Y_Cutscene = 0;
                    }
                }
                else if (Body.name == "AI Kate")
                {
                    if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/Y").InnerText != "")
                    {
                        X_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/X").InnerText);
                        Y_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/KateAI/Y").InnerText);
                    }
                    else
                    {
                        X_Cutscene = 0;
                        Y_Cutscene = 0;
                    }
                }
                else if (Body.name == "AI April")
                {
                    if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/Y").InnerText != "")
                    {
                        X_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/X").InnerText);
                        Y_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/AprilAI/Y").InnerText);
                    }
                    else
                    {
                        X_Cutscene = 0;
                        Y_Cutscene = 0;
                    }
                }
                else if (Body.name == "AI Ethan")
                {
                    if (node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/X").InnerText != "" || node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/Y").InnerText != "")
                    {
                        X_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/X").InnerText);
                        Y_Cutscene = double.Parse(node.SelectSingleNode("Part" + EventNumber + "/Case" + CaseStep + "/EthanAI/Y").InnerText);
                    }
                    else
                    {
                        X_Cutscene = 0;
                        Y_Cutscene = 0;
                    }
                }

            }
        }

    }

    private void SaveInfo(Rigidbody2D Body, int EventNumber, int CaseStep)
    {
        //---Set values for save data---
        //Read in SaveGame.xml
        XmlDocument xSaveGameDoc = new XmlDocument();
        xSaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

        //Find current scene then find ScenePart. 
        foreach (XmlNode Xnode in xSaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
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

                //Save XML
                DoSaveGame.UpdateSaveData(xSaveGameDoc);

                break;

            }                        
        }
    }

    public int PreformEvent (Rigidbody2D Body, int Speed, Animator Anim, int EventNumber, double X_Original, double Y_Original, int CaseStep)
    {
        //SavePosition(Body.gameObject.name);  //<---- For saving an AI's location
        
        //Var for AI x and y
        float XMove = 0;
        float YMove = 0;


        if (X_Cutscene != 0 && Y_Cutscene != 0)
        {
            XMove = Find_X_Distance(Body, X_Cutscene);
            YMove = Find_Y_Distance(Body, Y_Cutscene);

            if (XMove == 0 && YMove == 0)
            {
                CaseStep++;
                GetLocation(Body, EventNumber, CaseStep);
                SaveInfo(Body, EventNumber, CaseStep);
            }

        }
        else
        {
            #region CutScene Finished
            Body.GetComponent<AI_Character>().Action = AI_Character.AI_Action.StationaryWithDir;
            Body.GetComponent<CircleCollider2D>().enabled = true;

            //---Check for teleport---
            //Read in SaveGame.xml
            XmlDocument SaveGameDoc = new XmlDocument();
            SaveGameDoc.LoadXml(Resources.Load("CutScenes/CutSceneEvents").ToString());

            foreach (XmlNode node in SaveGameDoc.SelectNodes("CutSceneEvents/Scenes/Scene"))
            {
                if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
                {
                    if (Body.name == "AI Josh")
                    {
                        #region AI Josh
                        //Check for teleport
                        if (node.SelectSingleNode("Part" + EventNumber + "/Teleport/JoshAI/X").InnerText != "" && node.SelectSingleNode("Part" + EventNumber + "/Teleport/JoshAI/Y").InnerText != "")
                        {
                            Body.GetComponent<Transform>().position = new Vector3(float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/JoshAI/X").InnerText), float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/JoshAI/Y").InnerText));
                        }

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
                        DoSaveGame.UpdateSaveData(xSaveGameDoc);

                        #endregion
                    }
                    else if (Body.name == "AI Matt")
                    {
                        #region AI Matt

                        //Check for teleport
                        if (node.SelectSingleNode("Part" + EventNumber + "/Teleport/MattAI/X").InnerText != "" && node.SelectSingleNode("Part" + EventNumber + "/Teleport/MattAI/Y").InnerText != "")
                        {
                            Body.GetComponent<Transform>().position = new Vector3(float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/MattAI/X").InnerText), float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/MattAI/Y").InnerText));
                        }

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
                        DoSaveGame.UpdateSaveData(xSaveGameDoc);

                        #endregion
                    }
                    else if (Body.name == "AI Kate")
                    {
                        #region AI Kate

                        //Check for teleport
                        if (node.SelectSingleNode("Part" + EventNumber + "/Teleport/KateAI/X").InnerText != "" && node.SelectSingleNode("Part" + EventNumber + "/Teleport/KateAI/Y").InnerText != "")
                        {
                            Body.GetComponent<Transform>().position = new Vector3(float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/KateAI/X").InnerText), float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/KateAI/Y").InnerText));
                        }

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
                        DoSaveGame.UpdateSaveData(xSaveGameDoc);

                        #endregion
                    }
                    else if (Body.name == "AI April")
                    {
                        #region AI April

                        //Check for teleport
                        if (node.SelectSingleNode("Part" + EventNumber + "/Teleport/AprilAI/X").InnerText != "" && node.SelectSingleNode("Part" + EventNumber + "/Teleport/AprilAI/Y").InnerText != "")
                        {
                            Body.GetComponent<Transform>().position = new Vector3(float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/AprilAI/X").InnerText), float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/AprilAI/Y").InnerText));
                        }

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
                        DoSaveGame.UpdateSaveData(xSaveGameDoc);

                        #endregion
                    }
                    else if (Body.name == "AI Ethan")
                    {
                        #region AI Ethan

                        //Check for teleport
                        if (node.SelectSingleNode("Part" + EventNumber + "/Teleport/EthanAI/X").InnerText != "" && node.SelectSingleNode("Part" + EventNumber + "/Teleport/EthanAI/Y").InnerText != "")
                        {
                            Body.GetComponent<Transform>().position = new Vector3(float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/EthanAI/X").InnerText), float.Parse(node.SelectSingleNode("Part" + EventNumber + "/Teleport/EthanAI/Y").InnerText));
                        }

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
                        DoSaveGame.UpdateSaveData(xSaveGameDoc);

                        #endregion
                    }

                }                
            }
            #endregion
        }

        
            

            //Execute Movement
            PlayerMovement.PlayerMove(Body, Speed, Anim, XMove, YMove);

            return CaseStep;

        }

    private int Find_X_Distance(Rigidbody2D Body, double xPosition)
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

    private int Find_Y_Distance(Rigidbody2D Body, double yPosition)
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

    
}
