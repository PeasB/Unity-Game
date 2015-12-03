//By: Ryan Dailey
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;

public class DoSaveGame : MonoBehaviour {


    ////---For encrypted one---
    ////Read in SaveGame.xml
    //XmlDocument SaveGameDoc = new XmlDocument();
    //SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());



    ////---For encrypted one---
    ////Save XML
    //DoSaveGame.UpdateSaveData(SaveGameDoc); 

        

    public static void NewGame()
    {
        //Read in DefultSaveGame.xml
        XmlDocument DefultSaveGameDoc = new XmlDocument();
        DefultSaveGameDoc.Load("Assets/Scripts/Save Data/Defult Save/DefultSaveGame.xml");

        UpdateSaveData(DefultSaveGameDoc);
        
    }

    public static string FetchSaveData()
    {
        //Get encrypted SaveGameData
        StringBuilder StringXML = new StringBuilder();
        StreamReader SR = new StreamReader("Assets/Scripts/Save Data/SaveGameData.txt");
        
        StringXML.Append(SR.ReadLine());       
        SR.Dispose();
        
        //return decrypted StringXml   
        return AES_Crypto.DecryptText(StringXML.ToString());
    }

    public static void UpdateSaveData(XmlDocument XmlDoc)
    {
        XmlDoc.SelectSingleNode("SaveData/Settings/DateTimeOfSave").InnerText = System.DateTime.Now.ToString(); //Saves current time

        StreamWriter SW = new StreamWriter("Assets/Scripts/Save Data/SaveGameData.txt");        
        SW.Write(AES_Crypto.EncryptText(XmlDoc.OuterXml));
        SW.Close();       
    }



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
