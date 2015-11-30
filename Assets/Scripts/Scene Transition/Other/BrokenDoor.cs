using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class BrokenDoor : MonoBehaviour {

    private Text TextMessage;


    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            if (InventoryScript.CheckItem(6) == false) //ItemID 6 is a hammer
            {
                TextMessage.text = "The door seems to be broken. Maybe I need a hammer to break it down";
                TextMessage.gameObject.SetActive(true);
            }
            else
            {
                //--Destory in the future, so save this in savedata--
                //Read in SaveGame.xml
                XmlDocument SaveGameDoc = new XmlDocument();
                SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

                //Create new item in the players inventory
                XmlNode ObjectName = SaveGameDoc.CreateElement("ObjectName");
                ObjectName.InnerText = this.gameObject.name;

                foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/DestroyObjects/Scene"))
                {
                    if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
                    {
                        node.AppendChild(ObjectName);
                        break;
                    }
                }

                //Save XML
                SaveGameDoc.Save("Assets/Scripts/SaveGame.xml");

                //Delete Item 6 (hammer)
                InventoryScript.DeleteItem(6);

                //Destroy Object
                Destroy(this.gameObject);
            }

        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            TextMessage.text = "";
            TextMessage.gameObject.SetActive(false);
        }
    }










    // Use this for initialization
    void Start () {

        //---Check if user already opened door in save data. If so, destory object right away. 
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/DestroyObjects/Scene"))
        {
            if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
            {
                foreach (XmlNode node1 in node.ChildNodes)
                {
                    if (node1.InnerText == this.gameObject.name)
                    {
                        Destroy(this.gameObject);
                        break;
                    }
                }
                break;
            }
        }


        //Get text canvas
        TextMessage = GameObject.Find("Message_Canvas").transform.FindChild("Text").gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
