using UnityEngine;

public class InteractionsItem : MonoBehaviour {

    CircleCollider2D CircleCollition;
    bool EnableCollition = true;

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            print("On Enter");
        }
    }

    //void OnCollisionStay2D(Collision2D collisionInfo)
    //{
    //    if (collisionInfo.gameObject.tag == "Player")
    //    {
    //        CircleCollition.enabled = false;
    //    }
    //}


    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            print("Exit");
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
