using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamera : MonoBehaviour
{
    //Transform puzzle = null;

    Transform location;

    public Transform player;

    bool inPuzzle;
    bool isSet;

    public float speed = 50f;

    Vector3 reset;

    // Start is called before the first frame update
    void Start()
    {
        reset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!inPuzzle)
        //{
        //    reset = transform.position;
        //}
        //

        if (inPuzzle)
        {
            
            if (!isSet)
            {
                reset = player.position;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isSet = true;
            }
            transform.position = Vector3.Lerp(transform.position, location.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * speed);
        }

    }

    public void setPuzzle(Transform puzzleLocation)
    {
        location = puzzleLocation;
        inPuzzle = true;
    }

    public void disablePuzzle()
    {
        //location.position = reset;
        inPuzzle = false;
        isSet = false;
    }
}
