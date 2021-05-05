using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoard : MonoBehaviour
{

    public float speed = 50f;

    private float x;
    private float z;

    bool puzzleStarted;

    // Start is called before the first frame update
    void Start()
    {
        x = 1f;
        z = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(puzzleStarted)
        {
            if (Input.GetKey(KeyCode.A))
            {
                x -= speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                x += speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.W))
            {
                z -= speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                z += speed * Time.deltaTime;
            }

            x = Mathf.Clamp(x, -20, 20);
            z = Mathf.Clamp(z, -20, 20);

            transform.localEulerAngles = new Vector3(-z, 0, -x);
        }
    }

    public void setPuzzleStart()
    {
        puzzleStarted = true;
    }

    public void setPuzzleExit()
    {
        puzzleStarted = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        Start();
    }

}
