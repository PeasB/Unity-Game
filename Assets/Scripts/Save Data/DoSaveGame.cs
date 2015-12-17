//By: Ryan Dailey
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;
using System;

public class DoSaveGame : MonoBehaviour
{

    //.Load("Assets/Scripts/SaveGame.xml")

    ////---For encrypted one---
    ////Read in SaveGame.xml
    //XmlDocument SaveGameDoc = new XmlDocument();
    //SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());



    ////---For encrypted one---
    ////Save XML
    //DoSaveGame.UpdateSaveData(SaveGameDoc); 

    //.LoadXml(Resources.Load("Defult Save/DefultSaveGame.xml").ToString()

    //.LoadXml(Resources.Load("CutScenes/CutSceneEvents.xml").ToString()

    //.LoadXml(Resources.Load("ItemFile/Items.xml").ToString()

    public static void NewGame()
    {
        //Read in DefultSaveGame.xml
        XmlDocument DefultSaveGameDoc = new XmlDocument();
        DefultSaveGameDoc.LoadXml(Resources.Load("Defult Save/DefultSaveGame").ToString());

        //Getting GameVersion and inputting it into SaveGameData
        StreamReader SR = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Delirium/Version.txt");        
        DefultSaveGameDoc.SelectSingleNode("SaveData/Settings/GameVersion").InnerText = SR.ReadLine();
        DefultSaveGameDoc.SelectSingleNode("SaveData/Settings/DateTimeStarted").InnerText = System.DateTime.Now.ToString();
        SR.Close();

        //Update/Save Data
        UpdateSaveData(DefultSaveGameDoc);
        
    }
      
    public static string FetchSaveData()
    {
        ////Get encrypted SaveGameData
        //StringBuilder StringXML = new StringBuilder();
        //StreamReader SR = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Delirium/SaveGameData.txt");

        //StringXML.Append(SR.ReadLine());       
        //SR.Dispose();

        ////return decrypted StringXml   
        //return AES_Crypto.DecryptText(StringXML.ToString());

        //==Unencrypted version==
        try
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Delirium/SaveGameData.xml");
            return XmlDoc.OuterXml;
        }
        catch (Exception)
        {
            return null;
        }
        
        
        
        
    }

    public static void UpdateSaveData(XmlDocument XmlDoc)
    {
        //XmlDoc.SelectSingleNode("SaveData/Settings/DateTimeOfSave").InnerText = System.DateTime.Now.ToString(); //Saves current time

        //StreamWriter SW = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Delirium/SaveGameData.txt");        
        //SW.Write(AES_Crypto.EncryptText(XmlDoc.OuterXml));
        //SW.Close();       

        //==Unencrypted version==
        XmlDoc.SelectSingleNode("SaveData/Settings/DateTimeOfSave").InnerText = System.DateTime.Now.ToString(); //Saves current time
        XmlDoc.Save(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Delirium/SaveGameData.xml");

        

    }



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
