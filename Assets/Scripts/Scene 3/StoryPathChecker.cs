using UnityEngine;
using System.Collections;
using System.Xml;

public class StoryPathChecker : MonoBehaviour {

	public int SceneID;
	public int ConversationID;
	public int Level;
	public int Value;

	 private XmlDocument Save;

	public bool ValueCheck()
	{
        Save = new XmlDocument();
        Save.Load("Assets/Scripts/SaveGame.xml");

        foreach (XmlNode Child in Save.SelectSingleNode("SaveData/StoryPaths"))
		{
			if(Child.SelectSingleNode("ID").InnerText == SceneID + "." + ConversationID + "." + Level && Child.SelectSingleNode("Outcome").InnerText == Value.ToString())
			{
				return true;
			}
		}

		return false;
	}
	
}
