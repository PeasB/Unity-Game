//By: Ryan Dailey
using UnityEngine;
using System.Xml;

public class SceneTransition : MonoBehaviour {

    private static GameObject AI_Matt;
    private static GameObject AI_Josh;
    private static GameObject AI_Kate;
    private static GameObject AI_April;
    private static GameObject AI_Ethan;


    public string WhatScene = "";
    public double PlayerX = 0;
    public double PlayerY = 0;

    //Check for collition    
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            DoSceneTransition(WhatScene, PlayerX, PlayerY);
        }
    }

    
    public static void DoSceneTransition(string SceneName, double XPlayer, double YPlayer)
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/X").InnerText = XPlayer.ToString();
        SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/Y").InnerText = YPlayer.ToString();


        //Find scene (WhatScene) then update the AI's position in that scene. 
        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
        {
            if (SceneName == node.SelectSingleNode("SceneName").InnerText)
            {
                #region Setting up Next Scene
                if (AI_Josh != null)
                {
                    if (AI_Josh.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/JoshAI/X").InnerText = (XPlayer - 0.001).ToString();
                        node.SelectSingleNode("AI/JoshAI/Y").InnerText = (YPlayer - 0.001).ToString();
                        node.SelectSingleNode("AI/JoshAI/Action").InnerText = "FollowPlayer";
                    }
                }
                if (AI_Matt != null)
                {
                    if (AI_Matt.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/MattAI/X").InnerText = (XPlayer - 0.002).ToString();
                        node.SelectSingleNode("AI/MattAI/Y").InnerText = (YPlayer - 0.002).ToString();
                        node.SelectSingleNode("AI/MattAI/Action").InnerText = "FollowPlayer";
                    }
                }
                if (AI_Kate != null)
                {
                    if (AI_Kate.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/KateAI/X").InnerText = (XPlayer - 0.003).ToString();
                        node.SelectSingleNode("AI/KateAI/Y").InnerText = (YPlayer - 0.003).ToString();
                        node.SelectSingleNode("AI/KateAI/Action").InnerText = "FollowPlayer";
                    }
                }
                if (AI_April != null)
                {
                    if (AI_April.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/AprilAI/X").InnerText = (XPlayer - 0.004).ToString();
                        node.SelectSingleNode("AI/AprilAI/Y").InnerText = (YPlayer - 0.004).ToString();
                        node.SelectSingleNode("AI/AprilAI/Action").InnerText = "FollowPlayer";
                    }
                }
                if (AI_Ethan != null)
                {
                    if (AI_Ethan.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/EthanAI/X").InnerText = (XPlayer - 0.005).ToString();
                        node.SelectSingleNode("AI/EthanAI/Y").InnerText = (YPlayer - 0.005).ToString();
                        node.SelectSingleNode("AI/EthanAI/Action").InnerText = "FollowPlayer";
                    }
                }

                #endregion
            }
            else if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
            {
                #region Cleaning up Current Scene

                if (AI_Josh != null)
                {
                    if (AI_Josh.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/JoshAI/X").InnerText = "";
                        node.SelectSingleNode("AI/JoshAI/Y").InnerText = "";
                        node.SelectSingleNode("AI/JoshAI/Action").InnerText = "";
                        node.SelectSingleNode("AI/JoshAI/EventType").InnerText = "";
                        node.SelectSingleNode("AI/JoshAI/CaseStep").InnerText = "";
                    }
                }
                if (AI_Matt != null)
                {
                    if (AI_Matt.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/MattAI/X").InnerText = "";
                        node.SelectSingleNode("AI/MattAI/Y").InnerText = "";
                        node.SelectSingleNode("AI/MattAI/Action").InnerText = "";
                        node.SelectSingleNode("AI/MattAI/EventType").InnerText = "";
                        node.SelectSingleNode("AI/MattAI/CaseStep").InnerText = "";
                    }
                }
                if (AI_Kate != null)
                {
                    if (AI_Kate.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/KateAI/X").InnerText = "";
                        node.SelectSingleNode("AI/KateAI/Y").InnerText = "";
                        node.SelectSingleNode("AI/KateAI/Action").InnerText = "";
                        node.SelectSingleNode("AI/KateAI/EventType").InnerText = "";
                        node.SelectSingleNode("AI/KateAI/CaseStep").InnerText = "";
                    }
                }
                if (AI_April != null)
                {
                    if (AI_April.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/AprilAI/X").InnerText = "";
                        node.SelectSingleNode("AI/AprilAI/Y").InnerText = "";
                        node.SelectSingleNode("AI/AprilAI/Action").InnerText = "";
                        node.SelectSingleNode("AI/AprilAI/EventType").InnerText = "";
                        node.SelectSingleNode("AI/AprilAI/CaseStep").InnerText = "";
                    }
                }
                if (AI_Ethan != null)
                {
                    if (AI_Ethan.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                    {
                        node.SelectSingleNode("AI/EthanAI/X").InnerText = "";
                        node.SelectSingleNode("AI/EthanAI/Y").InnerText = "";
                        node.SelectSingleNode("AI/EthanAI/Action").InnerText = "";
                        node.SelectSingleNode("AI/EthanAI/EventType").InnerText = "";
                        node.SelectSingleNode("AI/EthanAI/CaseStep").InnerText = "";
                    }
                }

                #endregion
            }
        }


        //Update Current level
        SaveGameDoc.SelectSingleNode("SaveData/SaveState/CurrentScene").InnerText = SceneName;

        //Save XML
        SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

        //Load Level
        Application.LoadLevel(SceneName);
    }


    // Use this for initialization
    void Start () {

        AI_Matt = GameObject.Find("AI Matt");
        AI_Josh = GameObject.Find("AI Josh");
        AI_Kate = GameObject.Find("AI Kate");
        AI_April = GameObject.Find("AI April");
        AI_Ethan = GameObject.Find("AI Ethan");
    }
	
	// Update is called once per frame
	void Update () {
	
        
	}
}
