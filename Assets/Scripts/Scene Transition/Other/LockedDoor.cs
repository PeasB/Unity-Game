using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class LockedDoor : MonoBehaviour {

    private Text TextMessage;



    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            if (InventoryScript.CheckItem(7) == false) //ItemID 7 is a key
            {
                TextMessage.text = "The door seems to be locked. Maybe I need to find the key";
                TextMessage.gameObject.SetActive(true);
            }
            else
            {
                //--Destory in the future, so save this in savedata--
                DeleteObjects.DeleteObject(this.gameObject.name);

                //Delete Item 7 (key)
                InventoryScript.DeleteItem(7);

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

        DeleteObjects.CheckIfDeleted(this.gameObject.name);
        

        //Get text canvas
        TextMessage = GameObject.Find("Message_Canvas").transform.FindChild("Text").gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
