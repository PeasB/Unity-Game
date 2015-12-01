//By: Ryan Dailey
using UnityEngine;
using UnityEngine.UI;

public class BrokenDoor : MonoBehaviour {

    private Text TextMessage;


    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            if (InventoryScript.CheckItem(6) == false) //ItemID 6 is a hammer
            {
                TextMessage.text = "The door seems to be broken. I need a hammer to break it down";
                TextMessage.gameObject.SetActive(true);
            }
            else
            {
                //--Destory in the future, so save this in savedata--
                DeleteObjects.DeleteObject(this.gameObject.name);

                //Delete Item 6 (hammer)
                InventoryScript.DeleteItem(6);

                //Destroy Object
                Destroy(this.gameObject);
            }

        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            TextMessage.text = "";
            TextMessage.gameObject.SetActive(false);
        }
    }

        

    // Use this for initialization
    void Start () {

        //---Check if user already opened door in save data. If so, destory object right away. 
        DeleteObjects.CheckIfDeleted(this.gameObject.name);

        //Get text canvas
        TextMessage = GameObject.Find("Message_Canvas").transform.FindChild("Text").gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
