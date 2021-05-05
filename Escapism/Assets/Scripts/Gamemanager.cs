using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    GameObject[] scriptObjects = null;

    [SerializeField]
    GameObject[] objectsToDisable = null;

    MonoBehaviour cast;

    bool isPaused;

    int puzzles = 0;

    int totalPuzzles;

    public Text timerDisplay;

	public Text puzzleCountDisplay; 

    float time;

    public GameObject tickSound;

    float update;

    public Transform player;

    public GameObject menu;
    public GameObject pauseMenu;
    public GameObject reticle;
    public GameObject playSound;
    public GameObject tenSecLeft;
    public GameObject puzzleCompleteSound;
    public GameObject puzzleFailedSound;

    bool gameComplete;

    public GameObject endPuzzleTextObj;
    public GameObject puzzledFailedTextObj;

    IEnumerator coroutine;
    public float msgTime;
    public float returnTime = 10f;

    bool tutorial;
    bool tutorialEnded = false;

    bool tenSecondsHasplayed;
    bool endingHasPlayed;

    float finalMinutes;
    float finalSeconds;

    public GameObject door;

    bool timesUpdated;
    bool gameLost;
    bool returning;

    public GameObject lost;


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            time = 60f * menu.GetComponent<Menu>().getTime();
        }
        catch
        {
            time = 600f;
        }

        //time = 2f;

        if (tutorial)
        {
            cast = GameObject.Find("Main Camera").GetComponent<Raycast>();
            cast.enabled = false;

            foreach (GameObject obj in scriptObjects)
            {
                MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
                foreach(MonoBehaviour script in scripts)
                {
                    script.enabled = false;
                }

            }

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            //GameObject.Find("TutorialManager").SetActive(true);
            try
            {
                //GameObject.Find("TutorialManager").SetActive(true);
                MonoBehaviour tut = GameObject.Find("TutorialManager").GetComponent<MonoBehaviour>();
                tut.enabled = true;
            }
            catch
            {
                print("null");
            }
        }

        if(tutorialEnded)
        {
            foreach (GameObject obj in scriptObjects)
            {
                MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    script.enabled = true;
                }

            }
        }
        totalPuzzles = GameObject.Find("Puzzles").transform.childCount;
        Instantiate(playSound,transform);
        //timer.text = minutes.ToString() + ":" + seconds.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                resumeGame();
                isPaused = false;
            }
            else
            {
                pauseGame();
                isPaused = true;
            }
        }



		float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);

        if (!gameComplete)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if(!gameLost)
            {
                if (!timesUpdated)
                {
                    timesUpdated = true;
                    finalMinutes = minutes;
                    finalSeconds = seconds;
                    Destroy(door);
                }

                timerDisplay.color = Color.green;
                puzzleCountDisplay.color = Color.green;
            }
            
            

        }

		puzzleCountDisplay.text = "Puzzles done: " + puzzles.ToString() + "/" + totalPuzzles.ToString();



		if (update != seconds && !gameComplete && !gameLost)
        {
            Instantiate(tickSound, player);
        }

        if(!gameComplete && !gameLost)
        {
            if (seconds >= 59.5 && seconds <= 60.5)
            {
                
                if (minutes < 9)
                {
                    timerDisplay.text = "0" + (minutes + 1).ToString() + ":00";
                }
                else
                {
                    timerDisplay.text = (minutes + 1).ToString() + ":00";
                }
            }
            else
            {
                if (minutes < 10 && seconds < 10)
                {
                    //minutes = "0" + minutes.ToString();
                    timerDisplay.text = "0" + minutes.ToString() + ":" + "0" + seconds.ToString();
                }
                else if (minutes < 10)
                {
                    timerDisplay.text = "0" + minutes.ToString() + ":" + seconds.ToString();
                }
                else if (seconds < 10)
                {
                    timerDisplay.text = minutes.ToString() + ":" + "0" + seconds.ToString();
                }
                else
                {
                    timerDisplay.text = minutes.ToString() + ":" + seconds.ToString();
                }
            }
            

            update = seconds;
        }
        else if(!gameLost)
        {
            if (minutes < 10 && seconds < 10)
            {
                //minutes = "0" + minutes.ToString();
                timerDisplay.text = "0" + finalMinutes.ToString() + ":" + "0" + finalSeconds.ToString();
            }
            else if (minutes < 10)
            {
                timerDisplay.text = "0" + finalMinutes.ToString() + ":" + finalSeconds.ToString();
            }
            else if (seconds < 10)
            {
                timerDisplay.text = finalMinutes.ToString() + ":" + "0" + finalSeconds.ToString();
            }
            else
            {
                timerDisplay.text = finalMinutes.ToString() + ":" + finalSeconds.ToString();
            }
        }
        else
        {
            print("rip");
            timerDisplay.text = "00:00";
        }

        if (puzzles >= totalPuzzles)
        {
            gameComplete = true;
        }

        if (seconds <= 10 && minutes <= 0 && !tenSecondsHasplayed && !gameComplete)
        {
            tenSecondsHasplayed = true;
            Instantiate(tenSecLeft, transform);
        }

        if (seconds <= 0 && minutes <= 0 && !endingHasPlayed)
        {
            print("test");
            endingHasPlayed = true;
            gameFailed();
        }

        //individualPuzzleFail - time ran out

        if(gameLost && !returning)
        {
            returning = true;
            coroutine = LostReturnToMenu(returnTime);
            lost.SetActive(true);
            StartCoroutine(coroutine);
        }

    }

    public void AddPuzzles()
    {
        puzzles++;
    }

    void pauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        reticle.SetActive(false);
    }

    void resumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        reticle.SetActive(true);
        Time.timeScale = 1;
    }

    public void puzzleComplete()
    {
        puzzles++;
        endPuzzleTextObj.SetActive(true);
        coroutine = WaitForComplete(msgTime);
        StartCoroutine(coroutine);
        Instantiate(puzzleCompleteSound, player);

	}

    public void puzzleComplete(string activator)
    {
        GameObject.Find("HiveActivator").GetComponent<PuzzleTrigger>().returnMovement();
        GameObject.Find(activator).SetActive(false);
        GameObject.Find("HiveActivator").GetComponent<PuzzleTrigger>().startTextObj.SetActive(false);
        puzzles++;
        endPuzzleTextObj.SetActive(true);
        coroutine = WaitForComplete(msgTime);
        StartCoroutine(coroutine);
        Instantiate(puzzleCompleteSound, player);
		//EMELIApuzzleCountDisplay++; 
    }

    public void gameFailed()
    {
        gameLost = true;
        Instantiate(puzzleFailedSound,transform);
    }

    public void puzzleFailed()
    {
        puzzledFailedTextObj.SetActive(true);
        coroutine = WaitForComplete(msgTime);
        StartCoroutine(coroutine);
    }

    IEnumerator WaitForComplete(float msgTime)
    {
        yield return new WaitForSeconds(msgTime);
        endPuzzleTextObj.SetActive(false);
        puzzledFailedTextObj.SetActive(false);
    }

    IEnumerator LostReturnToMenu(float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        returnToMenu();
    }

    public bool isTutorial()
    {
        return tutorial;
    }

    public void returnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool getIspaused()
    {
        return isPaused;
    }
}
