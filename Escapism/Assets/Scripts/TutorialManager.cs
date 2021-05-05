using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    [SerializeField]
    GameObject[] tutorialPanels = null;

    [SerializeField]
    Transform[] puzzleCameras = null;

    [SerializeField]
    GameObject[] puzzlePanels = null;

    MonoBehaviour cast;

    public GameObject continueDisplay;

    public CharacterController controller;

    public GameObject sliderMarker;
    public GameObject buttonsMarker;

    public GameObject cam;

    IEnumerator coroutine;
    float timer = 3f;

    int currentPanel = 0;
    bool coroutineStarted;
    bool isAllowed;
    bool uniqueCase;
    bool wasCalled;
    bool overRide;
    bool basicTutorial = true;
    bool isNew;
    bool firstAdvTutorial = true;
    bool begin;

    // Start is called before the first frame update
    void Start()
    {
        cast = GameObject.Find("Main Camera").GetComponent<Raycast>();
        cast.enabled = false;
        tutorialPanels[currentPanel].SetActive(true);
        coroutineStarted = true;
        coroutine = cont(timer);
        StartCoroutine(coroutine);

        //currentPanel = puzzlePanels.Length - 1;
        //basicTutorial = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return) && isAllowed)
        //{
        //    if (tutorialPanels[currentPanel].name == "ApproachPuzzle")
        //    {
        //        sliderMarker.SetActive(true);
        //    }
        //    else if (tutorialPanels[currentPanel].name == "SliderBasic")
        //    {
        //        sliderMarker.SetActive(false);
        //        buttonsMarker.SetActive(true);
        //    }
        //    else if()
        //}

        print(currentPanel);

        if (basicTutorial)
        {
            if (tutorialPanels[currentPanel].name == "ApproachPuzzle" || tutorialPanels[currentPanel].name == "SliderBasic" && !wasCalled)
            {
                if (tutorialPanels[currentPanel].name == "ApproachPuzzle")
                {
                    sliderMarker.SetActive(true);
                    //if (!coroutineStarted)
                    //{
                    //    coroutineStarted = true;
                    //    isAllowed = false;
                    //    coroutine = cont(timer);
                    //    StartCoroutine(coroutine);
                    //}
                }

                if (tutorialPanels[currentPanel].name == "SliderBasic")
                {
                    cast.enabled = true;
                    sliderMarker.SetActive(false);
                    buttonsMarker.SetActive(true);
                }
                uniqueCase = true;
                isAllowed = false;
                wasCalled = true;
            }

            if (tutorialPanels[currentPanel].name == "SliderBasic2")
            {
                buttonsMarker.SetActive(false);
            }

            if ((Input.GetKeyDown(KeyCode.Return) && isAllowed) || overRide)
            {
                coroutineStarted = false;

                overRide = false;
                uniqueCase = false;
                wasCalled = false;
                continueDisplay.SetActive(false);
                if (!coroutineStarted)
                {
                    coroutineStarted = true;
                    isAllowed = false;
                    coroutine = cont(timer);
                    StartCoroutine(coroutine);
                    tutorialPanels[currentPanel].SetActive(false);
                    currentPanel++;
                    //tutorialPanels[currentPanel].SetActive(true);
                    if (currentPanel == tutorialPanels.Length)
                    {
                        basicTutorial = false;
                        isNew = true;
                    }
                    else
                    {
                        tutorialPanels[currentPanel].SetActive(true);
                    }

                }

            }
        }
        else
        {
            controller.enabled = false;
            cast.enabled = false;
            if (begin)
            {
                //MonoBehaviour tut = GameObject.Find("HouseTutorialOption").GetComponent<MonoBehaviour>();
                //tut.enabled = false;
                //GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTutorial(false);
                //DontDestroyOnLoad(GameObject.Find("HouseTutorialOption"));
                //print("new scene");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //using UnityEngine.SceneManagement;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                try
                {
                    //Destroy(GameObject.Find("HouseTutorialOption"));
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
                catch
                {
                    print("lmao nope!");
                }

            }
            if (isNew)
            {
                currentPanel = 0;
                isNew = false;
            }
            if (Input.GetKeyDown(KeyCode.Return) && isAllowed)
            {
                print(currentPanel);
                continueDisplay.SetActive(false);
                if (!coroutineStarted)
                {

                    coroutineStarted = true;
                    isAllowed = false;
                    coroutine = cont(timer);
                    StartCoroutine(coroutine);
                    if (firstAdvTutorial)
                    {
                        puzzlePanels[currentPanel].SetActive(true);
                        //currentPanel++;
                        cam.GetComponent<FocusCamera>().setPuzzle(puzzleCameras[currentPanel]);
                        firstAdvTutorial = false;
                    }
                    else
                    {
                        puzzlePanels[currentPanel].SetActive(false);
                        currentPanel++;
                        if (currentPanel == puzzlePanels.Length)
                        {
                            begin = true;
                            //currentPanel--;
                            print("test");
                        }
                        else
                        {
                            cam.GetComponent<FocusCamera>().setPuzzle(puzzleCameras[currentPanel]);
                            puzzlePanels[currentPanel].SetActive(true);
                        }

                    }

                }
            }

        }
       

        if(currentPanel > tutorialPanels.Length)
        {
            basicTutorial = false;
            //tutorial is finished
        }
    }

    IEnumerator cont(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(!uniqueCase)
        {
            continueDisplay.SetActive(true);
        }
        coroutineStarted = false;
        isAllowed = true;
        
    }

    public void allow()
    {
        isAllowed = true;
    }

    public void setOverRide()
    {
        overRide = true;
    }
}
