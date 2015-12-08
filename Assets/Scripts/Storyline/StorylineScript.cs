//By: Ryan Dailey
//This Script and prefab is currently not being used!
using UnityEngine;
using System.Xml;

public class StorylineScript : MonoBehaviour {

    private static GameObject AI_Matt;
    private static GameObject AI_Josh;
    private static GameObject AI_Kate;
    private static GameObject AI_April;
    private static GameObject AI_Ethan;

    private int PartNumber = 0;

    private void SetUpScene(int ScenePart)
    {
        if (Application.loadedLevelName == "Walking Sounds Test Scene")
        {
            #region Walking Sound Test Scene
            if (ScenePart == 0)
            {
                #region ScenePart 0
                //---AI---
                //AI Josh
                AI_Josh.GetComponent<Transform>().position = new Vector3(1,1); //Position
                AI_Josh.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event; //Action
                AI_Josh.GetComponent<AI_Character>().EventType = 3; //Event Type


                #endregion
            }
            else if (ScenePart == 1)
            {
                #region ScenePart 1

                //---AI---
                //AI Josh
                AI_Josh.GetComponent<Transform>().position = new Vector3(3, 3); //Position
                AI_Josh.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event; //Action
                AI_Josh.GetComponent<AI_Character>().EventType = 2; //Event Type

                //AI April
                AI_April.GetComponent<Transform>().position = new Vector3(-1, 4); //Position
                AI_April.GetComponent<AI_Character>().Action = AI_Character.AI_Action.FollowPlayer; 


                #endregion
            }
            #endregion
        }
        else if (Application.loadedLevelName == "Ryans Test Scene")
        {

        }
        else if (Application.loadedLevelName == "")
        {

        }
        else if (Application.loadedLevelName == "")
        {

        }
        else if (Application.loadedLevelName == "")
        {

        }
        else if (Application.loadedLevelName == "")
        {

        }

    }

    public static void DoActionForScene(int ScenePart)
    {
        if (Application.loadedLevelName == "Scene 2")
        {
            #region Scene 2
            if (ScenePart == 1)
            {
                #region Part 1
                //AI Josh and AI Kate runs to the split path, saying they both wanna race you.
                //AI Josh and AI Kate then takes off

                AI_Josh.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Josh.GetComponent<AI_Character>().EventType = ScenePart;

                AI_Kate.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Kate.GetComponent<AI_Character>().EventType = ScenePart;

                AI_April.GetComponent<AI_Character>().Action = AI_Character.AI_Action.FollowPlayer;

                AI_Ethan.GetComponent<AI_Character>().Action = AI_Character.AI_Action.FollowPlayer;

                #endregion
            }
            else if (ScenePart == 2)
            {
                #region Part 2
                //This is the part where they either split up or go together.
                //Josh, Kate and Ethan always go to the left path
                //Make Ethan not follow you anymore
                //April continues to follow you
                
                
                AI_Josh.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Josh.GetComponent<AI_Character>().EventType = ScenePart;
                AI_Josh.GetComponent<CircleCollider2D>().enabled = false;
                                
                AI_Kate.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Kate.GetComponent<AI_Character>().EventType = ScenePart;
                AI_Kate.GetComponent<CircleCollider2D>().enabled = false;
                                
                AI_Ethan.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Ethan.GetComponent<AI_Character>().EventType = ScenePart;
                AI_Ethan.GetComponent<CircleCollider2D>().enabled = false;


                #endregion
            }
            else if (ScenePart == 3)
            {
                #region Part 3
                //Just run ScenePart 2
                //There will be no part 3 in CutScenesEvents
                DoActionForScene(ScenePart - 1);
                #endregion
            }
            else if (ScenePart == 4)
            {
                #region Part 4
                //All four AI's walk up to the cabin
                //AI_April gets disabled so that she can walk right through you, since she is behind you


                AI_April.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_April.GetComponent<AI_Character>().EventType = ScenePart;
                AI_April.GetComponent<CircleCollider2D>().enabled = false;

                AI_Josh.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Josh.GetComponent<AI_Character>().EventType = ScenePart;

                AI_Kate.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Kate.GetComponent<AI_Character>().EventType = ScenePart;

                AI_Ethan.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Ethan.GetComponent<AI_Character>().EventType = ScenePart;


                #endregion
            }
            else if (ScenePart == 5)
            {
                print("part 5");
                #region Part 5
                //April, Kate and Josh walk inside the Cabin
                //Player Matt and Ethan stay outside

                AI_Josh.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Josh.GetComponent<AI_Character>().EventType = ScenePart;

                AI_Kate.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Kate.GetComponent<AI_Character>().EventType = ScenePart;

                AI_April.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_April.GetComponent<AI_Character>().EventType = ScenePart;


                #endregion
            }
            else if (ScenePart == 6)
            {
                print("PART 6!!!");
            }


            #endregion
        }
        else if (Application.loadedLevelName == "Scene 3")
        {
            #region Scene 3
            if (ScenePart == 1)
            {
                #region Part 1
                //You go to the cabin by yourself
                //AI kate moves out of the way

                AI_Kate.GetComponent<AI_Character>().Action = AI_Character.AI_Action.Event;
                AI_Kate.GetComponent<AI_Character>().EventType = ScenePart;

                #endregion
            }
            else if (ScenePart == 2)
            {
                #region Part 2
                //Kate follows you to the shed
                //There is no part 2 in CutSceneEvents since there is no cutscene

                AI_Kate.GetComponent<AI_Character>().Action = AI_Character.AI_Action.FollowPlayer;
                              

                #endregion
            }
            else if (ScenePart == 3)
            {

            }
            else if (ScenePart == 4)
            {

            }
            #endregion
        }
        else if (Application.loadedLevelName == "Scene 3 Outside")
        {
            #region Scene 3 Outside
            if (ScenePart == 1)
            {

            }
            else if (ScenePart == 2)
            {

            }
            else if (ScenePart == 3)
            {

            }
            else if (ScenePart == 4)
            {

            }
            #endregion
        }
        else if (Application.loadedLevelName == "Scene 3 Shed")
        {
            #region Scene 3 Shed
            if (ScenePart == 1)
            {

            }
            else if (ScenePart == 2)
            {

            }
            else if (ScenePart == 3)
            {

            }
            else if (ScenePart == 4)
            {

            }
            #endregion
        }


    }
    
	// Use this for initialization
	void Start () {

        //Read in SaveGame.xml
        XmlDocument SaveGameDoc = new XmlDocument();
        SaveGameDoc.Load("Assets/Scripts/SaveGame.xml");

        AI_Matt = GameObject.Find("AI Matt");
        AI_Josh = GameObject.Find("AI Josh");
        AI_Kate = GameObject.Find("AI Kate");
        AI_April = GameObject.Find("AI April");
        AI_Ethan = GameObject.Find("AI Ethan");

        //Find current scene then find ScenePart. 
        foreach (XmlNode node in SaveGameDoc.SelectNodes("SaveData/SaveState/Scenes/Scene"))
        {
            if (Application.loadedLevelName == node.SelectSingleNode("SceneName").InnerText)
            {
                //SetUpScene(int.Parse(node.SelectSingleNode("ScenePart").InnerText));
                break;
            }
        }

   }
	
	// Update is called once per frame
	void Update () {
	


	}
}
