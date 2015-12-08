using UnityEngine;
using System.Collections;

public class CallCanvas : MonoBehaviour {

    public float Timer;
    private bool TimerActive;


    public void OnTriggerEnter2D(Collider2D Other)
    {
        Other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Other.GetComponent<Player>().enabled = false;
        TimerActive = true;
    }

    void Update()
    {
        Timer -= Time.deltaTime;

        if(Timer <= 0)
        {

        }

    }


}
