using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTrigger : MonoBehaviour
{
    public GameObject player;

    public GameObject speedPuzzle;
    public GameObject sliderPuzzle;
    public GameObject safePuzzle;
    public GameObject patternsPuzzle;
    public GameObject memoryPuzzle;

    public GameObject ballPlane;
    public GameObject ballObj;

    public GameObject cam;

    public Transform speedCam;
    public Transform sliderCam;
    public Transform ballCam;
    public Transform safeCam;
    public Transform patternsCam;
    public Transform memoryCam;

    public GameObject startTextObj;
    public Text startText;

    bool speed;
    bool slider;
    bool ball;
    bool safe;
    bool patterns;
    bool memory;

    // Start is called before the first frame update
    void Start()
    {
        startTextObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown("space") && speed)
        {
            player.GetComponent<CharacterController>().enabled = false;
            cam.GetComponent<FocusCamera>().setPuzzle(speedCam);
            speedPuzzle.GetComponent<SpeedPuzzle>().setPuzzleStart();
            startText.text = "Press Q to exit.";
        }

        if (Input.GetKeyDown("space") && slider)
        {
            if(GameObject.Find("GameManager").GetComponent<Gamemanager>().isTutorial())
            {
                GameObject.Find("TutorialManager").GetComponent<TutorialManager>().setOverRide();
            }
            player.GetComponent<CharacterController>().enabled = false;
            cam.GetComponent<FocusCamera>().setPuzzle(sliderCam);
            sliderPuzzle.GetComponent<Slider>().setPuzzleStart();
            startText.text = "Press Q to exit.";
        }

        if (Input.GetKeyDown("space") && ball)
        {
            player.GetComponent<CharacterController>().enabled = false;
            cam.GetComponent<FocusCamera>().setPuzzle(ballCam);
            ballPlane.GetComponent<MoveBoard>().setPuzzleStart();
            ballObj.GetComponent<Ball>().setPuzzleStart();
            startText.text = "Press Q to exit.";
        }

        if (Input.GetKeyDown("space") && safe)
        {
            player.GetComponent<CharacterController>().enabled = false;
            cam.GetComponent<FocusCamera>().setPuzzle(safeCam);
            safePuzzle.GetComponent<SafePuzzle>().setPuzzleStart();
            startText.text = "Press Q to exit.";
        }

        if (Input.GetKeyDown("space") && patterns)
        {
            player.GetComponent<CharacterController>().enabled = false;
            cam.GetComponent<FocusCamera>().setPuzzle(patternsCam);
            patternsPuzzle.GetComponent<PatternsPuzzle>().setPuzzleStart();
            startText.text = "Press Q to exit.";
        }

        if (Input.GetKeyDown("space") && memory)
        {
            player.GetComponent<CharacterController>().enabled = false;
            cam.GetComponent<FocusCamera>().setPuzzle(memoryCam);
            memoryPuzzle.GetComponent<MemoryPuzzle>().setPuzzleStart();
            startText.text = "Press Q to exit.";
        }


        if (Input.GetKeyDown("q") && speed)
        {
            speedPuzzle.GetComponent<SpeedPuzzle>().setPuzzleExit();
            cam.GetComponent<FocusCamera>().disablePuzzle();
            player.GetComponent<CharacterController>().enabled = true;
        }
        if (Input.GetKeyDown("q") && slider)
        {
            sliderPuzzle.GetComponent<Slider>().setPuzzleExit();
            cam.GetComponent<FocusCamera>().disablePuzzle();
            player.GetComponent<CharacterController>().enabled = true;
        }
        if (Input.GetKeyDown("q") && ball)
        {
            ballPlane.GetComponent<MoveBoard>().setPuzzleExit();
            ballObj.GetComponent<Ball>().setPuzzleExit();
            cam.GetComponent<FocusCamera>().disablePuzzle();
            player.GetComponent<CharacterController>().enabled = true;
        }
        if (Input.GetKeyDown("q") && safe)
        {
            safePuzzle.GetComponent<SafePuzzle>().setPuzzleExit();
            cam.GetComponent<FocusCamera>().disablePuzzle();
            //camera.GetComponent<FocusCamera>().disablePuzzle();
            player.GetComponent<CharacterController>().enabled = true;
        }

        if (Input.GetKeyDown("q") && patterns)
        {
            patternsPuzzle.GetComponent<PatternsPuzzle>().setPuzzleExit();
            cam.GetComponent<FocusCamera>().disablePuzzle();
            //camera.GetComponent<FocusCamera>().disablePuzzle();
            player.GetComponent<CharacterController>().enabled = true;
        }

        if (Input.GetKeyDown("q") && memory)
        {
            memoryPuzzle.GetComponent<MemoryPuzzle>().setPuzzleExit();
            cam.GetComponent<FocusCamera>().disablePuzzle();
            //camera.GetComponent<FocusCamera>().disablePuzzle();
            player.GetComponent<CharacterController>().enabled = true;
        }


    }

    void OnTriggerEnter(Collider collider)
    {
        if (gameObject.tag == "Speed")
        {
            startText.text = "Start Speed Puzzle? (SPACE)";
            startTextObj.SetActive(true);
            speed = true;
        }
        else if (gameObject.tag == "Slider")
        {
            startText.text = "Start Slider Puzzle? (SPACE)";
            startTextObj.SetActive(true);
            slider = true;
        }
        else if (gameObject.tag == "Ball" && collider.tag == "Player")
        {
            startText.text = "Start Ball Maze? (SPACE)";
            startTextObj.SetActive(true);
            ball = true;
        }
        else if (gameObject.tag == "Safe")
        {
            startText.text = "Start Safe Puzzle? (SPACE)";
            startTextObj.SetActive(true);
            safe = true;
        }
        else if (gameObject.tag == "Patterns")
        {
            startText.text = "Start Patterns Puzzle? (SPACE)";
            startTextObj.SetActive(true);
            patterns = true;
        }

        else if (gameObject.tag == "Memory")
        {
            startText.text = "Start Memory Puzzle? (SPACE)";
            startTextObj.SetActive(true);
            memory = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        startTextObj.SetActive(false);
        if (gameObject.tag == "Speed")
        {
            speed = false;
        }
        else if (gameObject.tag == "Slider")
        {
            slider = false;
        }
        else if (gameObject.tag == "Ball" && collider.tag == "Player")
        {
            ball = false;
        }
        else if (gameObject.tag == "Safe")
        {
            safe = false;
        }
        else if (gameObject.tag == "Patterns")
        {
            patterns = false;
        }
        else if (gameObject.tag == "Memory")
        {
            memory = false;
        }
    }

    public void returnMovement()
    {
        cam.GetComponent<FocusCamera>().disablePuzzle();
        player.GetComponent<CharacterController>().enabled = true;
    }
}
