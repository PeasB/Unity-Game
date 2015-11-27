//By: Ryan Dailey
using UnityEngine;
using System.Xml;
using System.IO;
using System.Text;

public class DoSaveGame : MonoBehaviour {


    public static void NewGame()
    {

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

    public static void UpdateSaveData(string XmlDoc)
    {
        StreamWriter SW = new StreamWriter("Assets/Scripts/Save Data/SaveGameData.txt");        
        SW.Write(AES_Crypto.EncryptText(XmlDoc));
        SW.Close();       
    }





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
