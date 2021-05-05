using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemPuzzle : MonoBehaviour
{
    [SerializeField]
    Transform[] totems = null;

    [SerializeField]
    GameObject[] shapes = null;

    string[] sequence;
    string[] currentFace;

    public float height = 4f;

    bool puzzleStarted = true;
    bool puzzleComplete;

    public float speed;

    int currentTotem;

    public GameObject spinSound;

    // Start is called before the first frame update
    void Start()
    {
        sequence = new string[totems.Length];
        currentFace = new string[totems.Length];
        currentTotem = totems.Length;

        for (int i = 0; i < totems.Length; i++)
        {
            int rot = Random.Range(0, 4);
            totems[i].Rotate(0, 90 * rot, 0);
            GameObject obj = Instantiate(shapes[Random.Range(0, shapes.Length)],totems[i]);
            obj.transform.position = new Vector3(totems[i].position.x, totems[i].position.y + height, totems[i].position.z);
            sequence[i] = obj.tag;

            int direction = Mathf.RoundToInt(Quaternion.LookRotation(totems[i].forward).eulerAngles.y);

            //0 = square
            //90 = triangle
            //180 = circle
            //270 = x

            currentFace[i] = shapes[direction/90].tag;
        }


        for (int i = 0; i < totems.Length; i++)
        {
            int randPos = Random.Range(0, totems.Length);
            Vector3 temp = new Vector3(totems[i].position.x, totems[i].position.y, totems[i].position.z);
            Vector3 rand = new Vector3(totems[randPos].position.x, totems[randPos].position.y, totems[randPos].position.z);
            totems[i].position = rand;
            totems[randPos].position = temp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleStarted)
        {
            if(currentTotem < totems.Length)
            {
                //int temp = 100;
                for(int i = currentTotem; i < totems.Length; i++)
                {
                    Instantiate(spinSound, totems[i].transform);
                    totems[i].Rotate(0f, 90f, 0f);
                    int direction = Mathf.RoundToInt(Quaternion.LookRotation(totems[i].forward).eulerAngles.y);
                    currentFace[i] = shapes[direction / 90].tag;
                }
                currentTotem = totems.Length;
            }

            if(isComplete())
            {
                puzzleStarted = false;
                GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete();
                print("puzzle complete!");
            }
        }
    }

    bool isComplete()
    {
        for (int i = 0; i < currentFace.Length; i++)
        {
            if (sequence[i] != currentFace[i])
            {
                return false;
            }
        }
        return true;
    }

    public void RotatePillars(string totem)
    {
        currentTotem = int.Parse(totem.Substring(5));
    }

    void rotate(int totem)
    {
        float destination = totems[totem].rotation.eulerAngles.y - 90f;
        //totems[i].Rotate(0f, -90f, 0f);
        print(destination);
        print(totems[totem].rotation.eulerAngles.y);

        while (totems[totem].rotation.eulerAngles.y > destination)
        {
            totems[totem].RotateAround(totems[totem].position, totems[totem].up, Time.deltaTime * speed);
        }
    }
}
