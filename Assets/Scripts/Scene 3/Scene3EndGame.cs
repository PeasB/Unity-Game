using UnityEngine;
using System.Collections;

public class Scene3EndGame : MonoBehaviour {

    public float Timer = 5.0f;
    public AudioClip MaleScream;
    public AudioClip FemaleScream;
    private bool TimerActive;



    public void OnTriggerEnter2D(Collider2D Other)
    {
        //Disabling and Setting values in Player and possible AI.


        if(Other.tag == "Player")  //Player walked into Collider.
        {
            Other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Other.GetComponent<Animator>().SetBool("Moving", false);
            Other.GetComponent<WalkingSounds>().enabled = false;
            Other.GetComponent<Player>().enabled = false;
            Other.GetComponent<AudioSource>().clip = MaleScream;
            Other.GetComponent<AudioSource>().volume = 0.5f;
            Other.GetComponent<AudioSource>().loop = false;
            Other.GetComponent<AudioSource>().Play();

        }


        if(GameObject.Find("AI Kate") != null)    //Kate may or may not be in scene.
        {
            GameObject.Find("AI Kate").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("AI Kate").GetComponent<Animator>().SetBool("Moving", false);
            GameObject.Find("AI Kate").GetComponent<AI_Character>().enabled = false;
            GameObject.Find("AI Kate").GetComponent<WalkingSounds>().enabled = false;
            GameObject.Find("AI Kate").GetComponent<AudioSource>().clip = FemaleScream;
            GameObject.Find("AI Kate").GetComponent<AudioSource>().volume = 1;
            GameObject.Find("AI Kate").GetComponent<AudioSource>().loop = false;
            GameObject.Find("AI Kate").GetComponent<AudioSource>().Play();
        }
        

        TimerActive = true;
    }

    void FixedUpdate()
    {
        //Timer Decreacing every fixed frame.
        if(TimerActive)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            SceneTransition.DoSceneTransition("Credits", 0, 0);
        }

        if (Timer <= 5)
        {
            GameObject.Find("End Screen Canvas").transform.FindChild("Background").gameObject.SetActive(true);
            GameObject.Find("End Screen Canvas").transform.FindChild("To Be Continued...").gameObject.SetActive(true);
        }
        

    }


}
