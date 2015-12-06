using UnityEngine;
using System.Xml;

public class MainMenuManger : MonoBehaviour {
    

    void Start ()
    {
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Check if user has not started a game yet
        if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/CurrentScene").InnerText == "") //Make it the Date Started instead
        {
            GameObject.Find("Start Game").SetActive(false);
        }


    }

	//Called when Player Clicks Start New Game.
	public void StartNewGame()
	{
        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        //Check if user has already started a game, and warn them
        if (SaveGameDoc.SelectSingleNode("SaveData/SaveState/CurrentScene").InnerText != "") //Make it the Date Started instead
        {
            //Show "Are you sure you would like to erase your current save file? This cannot be undone" with a Yes or No button
			GameObject.Find ("Canvas").transform.Find ("Erase Save").gameObject.SetActive (true);
        }
        else
        {
            //No game has been created, so just start the game
            //Start new game
            DoSaveGame.NewGame();

            Application.LoadLevel("Scene 1");
        }
        

    }

	//Called When Player Clicks Continue game.
	public void ResumeGame()
	{
        ////Check if Save File needs to be patched
        //PatchSaveData.CheckForPatch(); 

        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        Application.LoadLevel(SaveGameDoc.SelectSingleNode("SaveData/SaveState/CurrentScene").InnerText);
    }


	public void StartNewGameConfirmed()
	{

		GameObject.Find ("Canvas").transform.Find ("Erase Save").gameObject.SetActive (false);

		//Start new game.
		DoSaveGame.NewGame();

		//Load the Level.
		Application.LoadLevel("Scene 1");
	}

	public void StartNewGameNotConfirmed()
	{
		GameObject.Find ("Canvas").transform.Find ("Erase Save").gameObject.SetActive (false);
	}
	
	
	//Called when Player Clicks Quit Game.
	public void QuitGame()
	{
        //PatchSaveData.CheckForPatch(); //<--Currently here only for testing purposes since this button isn't being used at the moment

		Application.Quit ();

	}

}
