using UnityEngine;
using System.Collections;

public class MainMenuManger : MonoBehaviour {


	//Called when Player Clicks Start New Game.
	public void StartNewGame()
	{
		print("Hello Start New");

	}

	//Called When Player Clicks Continue game.
	public void ResumeGame()
	{
		print ("Hello Resume");


	}

	//Called when Player Clicks Quit Game.
	public void QuitGame()
	{
		print ("Hello Quit Game");

		Application.Quit ();

	}

}
