using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Raycast : MonoBehaviour
{

    public float distance = 10f;

    GameObject obj;

    public GameObject sliderPuzzle;
    public GameObject speedPuzzle;
    public GameObject totemPuzzle;
    public GameObject patternsPuzzle;
    public GameObject memoryPuzzle;

    string[] speedButtons;

    bool tutorialFinished;

    bool memoryPuzzleCoroutine;

    public float selectRate = 0f;
    private float selectrateTimeStamp = 0f;

    // Start is called before the first frame update
    void Start()
    {
        speedButtons = new string[10] { "SButton0", "SButton1", "SButton2", "SButton3", "SButton4", "SButton5", "SButton6", "SButton7", "SButton8", "SButton9"};
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, distance) && Input.GetMouseButtonDown(0))
        {
            obj = hit.transform.gameObject;
            //print(obj.name);

            try
            {
                //if (obj.name == "SpeedActivator" || obj.name == "SliderActivator" || obj.name == "BallActivator" || obj.name == "SafeActivator" || obj.name == "MemoryActivator" || obj.name == "PatternsActivator")
                //{
                //    print("Activator: " + obj.name);
                //}

                if ((obj.name == "Up" || obj.name == "Down" || obj.name == "Left" || obj.name == "Right") && sliderPuzzle.GetComponent<Slider>().getPuzzleStarted())
                {
                    if (GameObject.Find("GameManager").GetComponent<Gamemanager>().isTutorial() && !tutorialFinished)
                    {
                        tutorialFinished = true;
                        GameObject.Find("TutorialManager").GetComponent<TutorialManager>().setOverRide();
                    }
                    sliderPuzzle.GetComponent<Slider>().Move(obj.name);
                }

                if (((IList)speedButtons).Contains(obj.name))
                {
                    speedPuzzle.GetComponent<SpeedPuzzle>().Activate(obj.name);
                }

                if (obj.GetComponent<Transform>().parent.name == "totem0" || obj.GetComponent<Transform>().parent.name == "totem1" || obj.GetComponent<Transform>().parent.name == "totem2" || obj.GetComponent<Transform>().parent.name == "totem3" || obj.GetComponent<Transform>().parent.name == "totem4")
                {
                    totemPuzzle.GetComponent<TotemPuzzle>().RotatePillars(obj.GetComponent<Transform>().parent.name);
                }

                if ((obj.GetComponent<Transform>().parent.name == "PatternCube1" || obj.GetComponent<Transform>().parent.name == "PatternCube2" || obj.GetComponent<Transform>().parent.name == "PatternCube3" || obj.GetComponent<Transform>().parent.name == "PatternCube4") && patternsPuzzle.GetComponent<PatternsPuzzle>().getPuzzleStarted() && Time.time > selectrateTimeStamp)
                {
                    selectrateTimeStamp = Time.time + selectRate;
                    patternsPuzzle.GetComponent<PatternsPuzzle>().selectCube(obj.GetComponent<Transform>().parent.name);
                }

                if ((obj.name == "PatternCube1" || obj.name == "PatternCube2" || obj.name == "PatternCube3" || obj.name == "PatternCube4") && patternsPuzzle.GetComponent<PatternsPuzzle>().getPuzzleStarted() && Time.time > selectrateTimeStamp)
                {
                    selectrateTimeStamp = Time.time + selectRate;
                    patternsPuzzle.GetComponent<PatternsPuzzle>().selectCube(obj.name);
                }

                if ((obj.name == "Button1" || obj.name == "Button2" || obj.name == "Button3" || obj.name == "Button4" || obj.name == "Button5" || obj.name == "Button6" || obj.name == "Button7") && memoryPuzzleCoroutine)
                {
                    memoryPuzzle.GetComponent<MemoryPuzzle>().clickButton(obj.name);
                }

                if ((obj.GetComponent<Transform>().parent.name == "Button1" || obj.GetComponent<Transform>().parent.name == "Button2" || obj.GetComponent<Transform>().parent.name == "Button3" || obj.GetComponent<Transform>().parent.name == "Button4" || obj.GetComponent<Transform>().parent.name == "Button5" || obj.GetComponent<Transform>().parent.name == "Button6" || obj.GetComponent<Transform>().parent.name == "Button7") && memoryPuzzleCoroutine)
                {
                    memoryPuzzle.GetComponent<MemoryPuzzle>().clickButton(obj.GetComponent<Transform>().parent.name);
                }

                if (obj.name == "ReturnButton")
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            catch
            {
                //print("NullReferenceException detected! Ignoring");
                print(obj.name);
            }
            
        }
    }

    public void setMemoryPuzzleCoroutine(bool set)
    {
        if (set)
        {
            memoryPuzzleCoroutine = true;
        }
        else
        {
            memoryPuzzleCoroutine = false;
        }
    }
}
