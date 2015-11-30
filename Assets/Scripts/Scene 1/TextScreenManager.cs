using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextScreenManager : MonoBehaviour {

    public Canvas EntryCanvas;
    public GameObject CarObject;
    public float TextInOutSpeed = 0f;
    public float OnScreenTime = 0f;
    private float Timer;
    private Text ByWho;
    private Text GameDescription;
    private Text CurrentTextObject;
    private bool TextHasAppeared = false;
    private bool NextText = true;



    void Start()
    {

        //By Who Setup
        ByWho = EntryCanvas.transform.FindChild("By Who").GetComponent<Text>();
        ByWho.enabled = false;
        ByWho.color = new Color(255, 255, 255, 0);

        //Game Description
        GameDescription = EntryCanvas.transform.FindChild("Game Description").GetComponent<Text>();
        GameDescription.enabled = false;
        GameDescription.color = new Color(255, 255, 255, 0);

    }


    // Update is called once per frame
    void FixedUpdate ()
    {

        if(NextText)
        {
            if (CurrentTextObject == null)
                CurrentTextObject = ByWho;
            else if (CurrentTextObject == ByWho)
                CurrentTextObject = GameDescription;
            else
                EndEntryText();

                Timer = OnScreenTime;
                NextText = false;
        }

        if (!TextHasAppeared)
            IncreaceOpacity();
        else if(TextHasAppeared && Timer <= 0)
        {
            DecreaceOpacity();
        }
        else
            Timer -= Time.deltaTime;
	}


    void IncreaceOpacity() //Makes the Text Visable.
    {
        if (CurrentTextObject.color.a + TextInOutSpeed < 1)
        {
            CurrentTextObject.enabled = true;
            CurrentTextObject.color = new Color(255f, 255f, 255f, CurrentTextObject.color.a + TextInOutSpeed);
        }
        else
        {
            CurrentTextObject.color = new Color(255f, 255f, 255f, 255f);
            TextHasAppeared = true;

        }
    }

    void DecreaceOpacity()
    {
        //Note: When the value reaches 1 on the increace the value is converted to 255. Weird? but this works. floats vs int?
        if (CurrentTextObject.color.a == 255)
            CurrentTextObject.color = new Color(255f, 255f, 255f, 1f);


        if (CurrentTextObject.color.a - TextInOutSpeed > 0)
        {
            CurrentTextObject.enabled = true;
            CurrentTextObject.color = new Color(255f, 255f, 255f, CurrentTextObject.color.a - TextInOutSpeed);
        }
        else
        {
            CurrentTextObject.color = new Color(255f, 255f, 255f, 0f);
            TextHasAppeared = false;
            NextText = true;

        }
    }


    void EndEntryText()
    {
        CarObject.GetComponent<MoveCar>().CanDrive = true;
        EntryCanvas.enabled = false;

    }
}
