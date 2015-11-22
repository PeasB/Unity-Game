using UnityEngine;

public class InteractionsItem : MonoBehaviour {

    CircleCollider2D CircleCollition;
    bool EnableCollition = true;

    int ItemID = 0;
    string ItemName = "";



    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "Player")
        {
            //Get Item info



            //Show "press x to pick up item"


        }
    }
    

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "Player")
        {
            //Stop showing "press x to pick up item"


        }

    }




    // Use this for initialization
    void Start () {

        CircleCollition = GetComponent<CircleCollider2D>();

    }
	
	// Update is called once per frame
	void Update () {
        CircleCollition.enabled = true;
    }
}
