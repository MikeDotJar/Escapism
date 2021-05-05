using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpeedPuzzle : MonoBehaviour
{

    [SerializeField]
    Renderer[] lights = null;


    Renderer[] order;

    private IEnumerator coroutine;
    private IEnumerator wait;
    bool puzzleStarted;
    bool newRoutine;

    public float time;
    public float delay;
    public float delayRate;
    public int size;
    int counter = 0;

    int choice = 0;

    public GameObject buttonSound;

    // Start is called before the first frame update
    void Start()
    {

        order = new Renderer[size];

        for (int i = 0; i < size; i++)
        {
            order[i] = lights[Random.Range(0, lights.Length)];
        }

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].material.color = Color.red;
        }

        newRoutine = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == size && puzzleStarted)
        {
            GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete("SpeedActivator");
            UnityEngine.Debug.Log("Puzzle complete!");
            puzzleStarted = false;
        }

        if (puzzleStarted && newRoutine)
        {
            newRoutine = false;
            order[counter].material.color = Color.green;
            coroutine = ResetPuzzle(time);
            StartCoroutine(coroutine);
        }

        //if (Input.GetKeyDown("space"))
        //{
        //    puzzleStarted = true;
        //}

        try
        {
            if(puzzleStarted)
            {
                //int keyPressed = int.Parse(Input.inputString);
                int keyPressed = choice;
                if (order[counter].material.color == Color.green && order[counter] == lights[keyPressed - 1])
                {
                    StopCoroutine(coroutine);
                    order[counter].material.color = Color.red;

                    //newRoutine = true;
                    counter++;
                    wait = Wait(delay);
                    delay -= delay * delayRate;
                    time -= time * delayRate;
                    StartCoroutine(wait);
                }
            }
            
        }
        catch
        {
            //UnityEngine.Debug.Log("Nothing is being pressed");
        }

    }

    IEnumerator ResetPuzzle(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleFailed();
        puzzleStarted = false;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        newRoutine = true;
    }

    public void Activate(string button)
    {
        choice = int.Parse(button.Substring(7)) + 1;
        print(choice);
        //choice = int.Parse(button.Substring(7));
        Instantiate(buttonSound, lights[0].transform);
    }

    public void setPuzzleStart()
    {
        puzzleStarted = true;
    }

    public void setPuzzleExit()
    {
        StopCoroutine(coroutine);
        puzzleStarted = false;
        newRoutine = false;
        counter = 0;
        Start();
    }
}
