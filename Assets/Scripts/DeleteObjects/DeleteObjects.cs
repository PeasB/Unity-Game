//By: Ryan Dailey
using UnityEngine;
using System.Xml;

public class DeleteObjects : MonoBehaviour {

    public static void DeleteObject(string NameOfObject)
    {
        //--Destory in the future, so save this in savedata--
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

        XmlNode ObjectName = SaveGameDoc.CreateElement("ObjectName");
        ObjectName.InnerText = NameOfObject;

        bool DoBreak = false;

        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/DestroyObjects/Scene"))
        {
            if (DoBreak == false)
            {
                foreach (XmlNode Xnode in node.ChildNodes)
                {
                    if (Xnode.Name == "SceneName" && Application.loadedLevelName == Xnode.InnerText)
                    {
                        node.AppendChild(ObjectName);

                        DoBreak = true;
                        break;
                    }
                }


            }
            else
            {
                break;
            }
        }


        //Save XML
        DoSaveGame.UpdateSaveData(SaveGameDoc);
    }


    public static void CheckIfDeleted(string NameOfObject)
    {
        GameObject CheckObject = GameObject.Find(NameOfObject);


        //---Check if user already picked up item in save data. If so, destory object right away. 
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

        bool DoBreak = false;

        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/DestroyObjects/Scene"))
        {
            if (DoBreak == false)
            {
                foreach (XmlNode Xnode in node.ChildNodes)
                {
                    if (Xnode.Name == "SceneName" && Application.loadedLevelName == Xnode.InnerText)
                    {
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.InnerText == NameOfObject)
                            {
                                Destroy(CheckObject);
                                break;
                            }
                        }
                        DoBreak = true;
                        break;
                    }

                }
            }
            else
            {
                break;
            }
        }
    }
}
