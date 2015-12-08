using UnityEngine;
using System.Collections;

public class Scene3EndGame : MonoBehaviour {

    public float Timer = 5.0f;
    public AudioClip MaleScream;
    public AudioClip FemaleScream;
    private bool TimerActive;



    public void OnTriggerEnter2D(Collider2D Other)
    {
        Other.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Other.GetComponent<Player>().enabled = false;
        Other.GetComponent<AudioSource>().clip = MaleScream;

        if(GameObject.Find("AI Kate") != null)
        {
            GameObject.Find("AI Kate").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("AI Kate").GetComponent<AI_Character>().enabled = false;
            GameObject.Find("AI Kate").GetComponent<AudioSource>().clip = FemaleScream;
        }

        TimerActive = true;
    }

    void Update()
    {
        if(TimerActive)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            SceneTransition.DoSceneTransition("Credits", 0, 0);
        }

        else if (Timer <= 5)
        {
            GameObject.Find("End Screen Canvas").transform.FindChild("Background").gameObject.SetActive(true);
            GameObject.Find("End Screen Canvas").transform.FindChild("To Be Continued...").gameObject.SetActive(true);
        }
        

    }


}
