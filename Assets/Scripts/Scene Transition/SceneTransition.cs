using UnityEngine;
using System.Xml;

public class SceneTransition : MonoBehaviour {

    private GameObject AI_Matt;
    private GameObject AI_Josh;
    private GameObject AI_Kate;
    private GameObject AI_April;
    private GameObject AI_Ethan;


    public string WhatScene = "";
    public double PlayerX = 0;
    public double PlayerY = 0;

    //Check for collition    
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            
            //Read in SaveGame.xml
            XmlDocument SaveGameDoc = new XmlDocument();
            SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

            SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/X").InnerText = PlayerX.ToString();
            SaveGameDoc.SelectSingleNode("SaveData/SaveState/PlayerPosition/Y").InnerText = PlayerY.ToString();

            if (AI_Josh != null)
            {
                if (AI_Josh.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/X").InnerText = (PlayerX - 0.001).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Y").InnerText = (PlayerY - 0.001).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Action").InnerText = "FollowPlayer";
                }
                else
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/X").InnerText = "";
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/JoshAI/Y").InnerText = "";
                }
            }

            if (AI_Matt != null)
            {
                if (AI_Matt.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/X").InnerText = (PlayerX - 0.002).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Y").InnerText = (PlayerY - 0.002).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Action").InnerText = "FollowPlayer";
                }
                else
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/X").InnerText = "";
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/MattAI/Y").InnerText = "";
                }
            }

            if (AI_Kate != null)
            {
                if (AI_Kate.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/X").InnerText = (PlayerX - 0.003).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Y").InnerText = (PlayerY - 0.003).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Action").InnerText = "FollowPlayer";
                }
                else
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/X").InnerText = "";
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/KateAI/Y").InnerText = "";
                }
            }

            if (AI_April != null)
            {
                if (AI_April.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/X").InnerText = (PlayerX - 0.004).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Y").InnerText = (PlayerY - 0.004).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Action").InnerText = "FollowPlayer";
                }
                else
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/X").InnerText = "";
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/AprilAI/Y").InnerText = "";
                }
            }

            if (AI_Ethan != null)
            {
                if (AI_Ethan.GetComponent<AI_Character>().Action == AI_Character.AI_Action.FollowPlayer)
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/X").InnerText = (PlayerX - 0.005).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Y").InnerText = (PlayerY - 0.005).ToString();
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Action").InnerText = "FollowPlayer";
                }
                else
                {
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/X").InnerText = "";
                    SaveGameDoc.SelectSingleNode("SaveData/SaveState/EthanAI/Y").InnerText = "";
                }
            }            

            //Save XML
            SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

            //Load Level
            Application.LoadLevel(WhatScene);

        }
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
