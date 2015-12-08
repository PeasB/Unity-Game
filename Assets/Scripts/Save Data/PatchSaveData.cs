//By: Ryan Dailey
using UnityEngine;
using System.Xml;

public class PatchSaveData : MonoBehaviour {

    public static void CheckForPatch()
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

        //If savedata gameversion does not match the current game version, run patch
        if (SaveGameDoc.SelectSingleNode("SaveData/Settings/GameVersion").InnerText != Application.version)
        {
            RunPatch();
        }
    }

    private static void RunPatch()
    {
        print("Running Patch");

        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.LoadXml(DoSaveGame.FetchSaveData());

        //if (SaveGameDoc.SelectSingleNode("SaveData/Settings") == null)
        //    print("I dont exist");        
        //else        
        //    print("I exist");

        
        //if (SaveGameDoc.SelectSingleNode("SaveData/Settings") == null)
        //{

        //}


        //Update save data to latest game version
        SaveGameDoc.SelectSingleNode("SaveData/Settings/GameVersion").InnerText = Application.version;

        //Save XML
        DoSaveGame.UpdateSaveData(SaveGameDoc);
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
