using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform plane;
    public Transform spawn;
    bool puzzleStarted;

    public MonoBehaviour board;

    public float dist = 3;

    // Start is called before the first frame update
    void Start()
    {
        SetRigidBody(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleStarted)
        {
            SetRigidBody(true);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "BallGameEnd")
        {
            plane.localEulerAngles = new Vector3(0, 0, 0);
            Destroy(gameObject);
            GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete("BallActivator");
            board.enabled = false;
            UnityEngine.Debug.Log("End");
        }

    }

    public void SetRigidBody(bool setting)
    {
        if(setting)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void setPuzzleStart()
    {
        puzzleStarted = true;
    }

    public void setPuzzleExit()
    {
        puzzleStarted = false;
        transform.position = new Vector3(spawn.position.x, spawn.position.y + dist, spawn.position.z);
        Start();
    }

    public bool getPuzzleStarted()
    {
        return puzzleStarted;
    }
}