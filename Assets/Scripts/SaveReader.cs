using UnityEngine;
using System.Collections;
using System.Xml;

public class SaveReader {


	public void StraightValueFromSave(string SectionPath)
	{
		XmlDocument Save = new XmlDocument();
		Save.Load("Assets/Scripts/SaveGame.xml");




	}

	public void StoryPathFromSave(int ID)
	{
		XmlDocument Save = new XmlDocument();
		Save.Load("Assets/Scripts/SaveGame.xml");


		foreach(XmlNode Node in Save.SelectNodes("SaveData/StoryPath"))
		{



		}



	}


}
