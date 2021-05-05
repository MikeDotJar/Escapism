using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzle : MonoBehaviour
{
    public Transform knob;

    public float[] correctVals = new float[3];
    public float time;
    public float tempRange;
    int currentTic = 0;
    public float range = 1f;

    public int i = 1;

    private float speed = 50f;
    private float x;

    bool puzzleStarted;
    bool coroutineStarted;
    bool newValue;

    private IEnumerator coroutine;
    private IEnumerator wait;

    public GameObject turnSound;
    public GameObject tickUpSound;
    public GameObject crackSound;

    // Start is called before the first frame update
    void Start()
    {
        x = 1f;

        correctVals[0] = Random.Range(10, 360);
        
        // to give the safe the generic r > l > r rotation pattern
        while (i < 3)
        {
            tempRange = Random.Range(10, 360);

            if (i == 1 && tempRange < correctVals[i - 1])
            {
                correctVals[i] = tempRange;
                i++;
                
            }

            if (i == 2 && tempRange > correctVals[i - 1])
            {
                correctVals[i] = tempRange;
                i++;
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleStarted)
        {
            if (Input.GetKey(KeyCode.A))
            {
                x -= speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                x += speed * Time.deltaTime;
            }

            // for when the inspector goes negative, simply convert value since it is actually 0-360
            if (x < 0)
            {
                x = 360 + x;  
            }

            // if rotation ever goes over 360, reset to 0
            if (x >= 360)
            {
                x = 0;
            }

            transform.localEulerAngles = new Vector3(x, 0, -90);

            // puzzle complete
            if (currentTic >= correctVals.Length)
            {
                GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete("SafeActivator");
                UnityEngine.Debug.Log("Game Complete.");
                this.enabled = false;
                setPuzzleExit();
            }

            if (!coroutineStarted && x <= correctVals[currentTic] + range && x >= correctVals[currentTic] - range)
            {
                Instantiate(crackSound, transform);
                coroutine = WaitForTick(time);
                newValue = false;
                coroutineStarted = true;
                UnityEngine.Debug.Log("This is a clue!");
                UnityEngine.Debug.Log("Coroutine started " + Time.time);
                StartCoroutine(coroutine);
            }

            // Exited range too fast
            if (coroutineStarted && !newValue && x > correctVals[currentTic] + range || coroutineStarted && !newValue && x < correctVals[currentTic] - range)
            {
                StopCoroutine(coroutine);
                coroutineStarted = false;
                UnityEngine.Debug.Log("You exited too soon.");
            }

            
            // player solves first tick, so range changes and user gets booted
        }

    }

    public void setPuzzleStart()
    {
        puzzleStarted = true;
    }

    public void setPuzzleExit()
    {
        puzzleStarted = false;
        currentTic = 0;
        Start();
    }

    IEnumerator WaitForTick(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(tickUpSound, transform);
        UnityEngine.Debug.Log("Coroutine ended " + Time.time);
        currentTic++;
        newValue = true;
        UnityEngine.Debug.Log("Current tic is " + currentTic);
        coroutineStarted = false;
    }
}
