using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternsPuzzle : MonoBehaviour
{
    [SerializeField]
    GameObject[] cubes = null;

    [SerializeField]
    GameObject[] markers = null;

    public GameObject edge1;
    public GameObject edge2;

    Color[] colors;

    int currentCube = 0;

    int[] sequence;

    bool puzzleStarted;
    bool puzzleComplete;

    float baseHeight;
    public float selectHeight;

    float min;
    float max;

    public GameObject shiftSound;
    public GameObject flipSound;
    public GameObject placeSound;

    bool firstIsPicked;

    public float shiftRate = 0f;
    private float shiftrateTimeStamp = 0f;

    float lowest;

    //float fix; 

    //float distance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        lowest = cubes[0].transform.position.z;
        //fix = Mathf.Abs(cubes[0].transform.position.z - cubes[1].transform.position.z);
        min = cubes[0].transform.position.z + 0.1f;
        max = cubes[3].transform.position.z - 0.1f;

        sequence = new int[cubes.Length];
        for(int i = 0; i < sequence.Length; i++)
        {
            sequence[i] = i;
        }

        baseHeight = cubes[0].transform.position.y;

        colors = new Color[8];
        colors[0] = Color.red;
        colors[1] = Color.blue;
        colors[2] = Color.green;
        colors[3] = Color.yellow;
        colors[4] = Color.grey;
        colors[5] = Color.magenta;
        colors[6] = Color.cyan;
        colors[7] = Color.black;

        int select = Random.Range(0, colors.Length);
        edge1.GetComponent<Renderer>().material.SetColor("_Color",colors[select]);
        markers[0].GetComponent<Renderer>().material.SetColor("_Color", colors[select]);

        for (int i = 1; i < 7; i+=2)
        {
            select = Random.Range(0, colors.Length);
            markers[i].GetComponent<Renderer>().material.SetColor("_Color", colors[select]);
            markers[i + 1].GetComponent<Renderer>().material.SetColor("_Color", colors[select]);

        }

        select = Random.Range(0, colors.Length);
        edge2.GetComponent<Renderer>().material.SetColor("_Color", colors[select]);
        markers[7].GetComponent<Renderer>().material.SetColor("_Color", colors[select]);

        int select2 = Random.Range(0, colors.Length);

        for (int i = 8; i < 32; i+=2)
        {
            select = Random.Range(0, colors.Length);
            markers[i].GetComponent<Renderer>().material.SetColor("_Color", colors[select]);
            while(select == select2)
            {
                select2 = Random.Range(0, colors.Length);
            }
            markers[i + 1].GetComponent<Renderer>().material.SetColor("_Color", colors[select2]);
        }

        
        //Color mainColor1 = markers[i % 8].GetComponent<Renderer>().material.color;
        //Color mainColor2 = markers[(i + 1) % 8].GetComponent<Renderer>().material.color;

        //for (int i = 8; i < 32; i += 2)
        //{
        //    Color tempColor1 = colors[select];
        //    Color tempColor2 = colors[select2];
        //}

        //for positioning: the less the z, the closer it is to the front



        for (int i = 0; i < cubes.Length; i++)
        {
            int randPos = Random.Range(0, cubes.Length);
            Vector3 temp = new Vector3(cubes[i].transform.position.x, cubes[i].transform.position.y, cubes[i].transform.position.z);
            Vector3 rand = new Vector3(cubes[randPos].transform.position.x, cubes[randPos].transform.position.y, cubes[randPos].transform.position.z);
            cubes[i].transform.position = rand;
            cubes[randPos].transform.position = temp;
            cubes[i].transform.Rotate(0,0, 90*Random.Range(0, 4));

            //sequence[i] = randPos;
            //sequence[randPos] = i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleStarted)
        {
            //float lowest = cubes[0].transform.position.z;

            //if (cubes[0].transform.position.z < cubes[1].transform.position.z && cubes[1].transform.position.z < cubes[2].transform.position.z && cubes[2].transform.position.z < cubes[3].transform.position.z)
            //{
            //    print("phase 1");
            //    puzzleComplete = true;
            //    for (int i = 0; i < cubes.Length; i++)
            //    {

            //        if (cubes[i].transform.rotation.eulerAngles.z >= 1f || cubes[i].transform.rotation.eulerAngles.z <= -1f)
            //        {
            //            print(cubes[i].transform.rotation.eulerAngles.z);
            //            puzzleComplete = false;
            //        }
            //    }

            //}

            if (puzzleSolved())
            {
                puzzleComplete = true;
            }

            if (puzzleComplete)
            {
                setPuzzleExit();
                //puzzleStarted = false;
                GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete("PatternsActivator");
                print("puzzle complete!");
            }

            if (Input.GetKeyDown(KeyCode.W) && firstIsPicked)
            {
                cubes[currentCube].transform.Rotate(0, 0, 90f);
                Instantiate(flipSound, cubes[currentCube].transform);
            }

            if (Input.GetKeyDown(KeyCode.S) && firstIsPicked)
            {
                cubes[currentCube].transform.Rotate(0, 0, -90f);
                Instantiate(flipSound, cubes[currentCube].transform);
            }

            if (Input.GetKeyDown(KeyCode.A) && cubes[currentCube].transform.position.z > min && firstIsPicked && Time.time > shiftrateTimeStamp) //min -1.2
            {
                shiftrateTimeStamp = Time.time + shiftRate;
                Vector3 cube2 = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y, cubes[currentCube].transform.position.z - 0.5f);
                cubes[currentCube].transform.position = cube2;
                Instantiate(shiftSound, cubes[currentCube].transform);
            }

            if (Input.GetKeyDown(KeyCode.D) && cubes[currentCube].transform.position.z < max && firstIsPicked && Time.time > shiftrateTimeStamp) //max 0.2
            {
                shiftrateTimeStamp = Time.time + shiftRate;
                Vector3 cube2 = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y, cubes[currentCube].transform.position.z + 0.5f);
                cubes[currentCube].transform.position = cube2;
                Instantiate(shiftSound, cubes[currentCube].transform);
            }
        }
    }

    bool puzzleSolved()
    {
        for(int i = 0; i < cubes.Length - 1; i++)
        {
            for(int j = i; j < cubes.Length - i - 1; j++)
            {
                if (cubes[i].transform.position.z == cubes[i + 1].transform.position.z && j != i)
                {
                    return false;
                }
            }
            
        }

        float[] values = new float[] { cubes[0].transform.position.z, cubes[1].transform.position.z, cubes[2].transform.position.z, cubes[3].transform.position.z };
        int[] order = new int[] { 0, 1, 2, 3 };
        for(int i = 0; i < values.Length - 1; i++)
        {
            for(int j = 0; j < values.Length - i - 1; j++)
            {
                if(values[j] > values[j + 1])
                {
                    float temp1 = values[j];
                    values[j] = values[j + 1];
                    values[j + 1] = temp1;

                    int temp2 = order[j];
                    order[j] = order[j + 1];
                    order[j + 1] = temp2;
                }
            }
        }

        Color edgeColor1 = edge1.GetComponent<Renderer>().material.color;
        Color edgeColor2 = edge2.GetComponent<Renderer>().material.color;

        if (markers[2 * order[0] + (int)(cubes[order[0]].transform.localRotation.eulerAngles.z / 90f * 8)].GetComponent<Renderer>().material.color == edgeColor1 && markers[ 1 + 2 * order[3] + (int)(cubes[order[3]].transform.localRotation.eulerAngles.z / 90f * 8)].GetComponent<Renderer>().material.color == edgeColor2)
        {
            int counter = 0;
            print("this is now running");
            for (int i = 1; i < 7; i+=2)
            {

                if (markers[(i % 2) + 2 * order[i / 2] + ((int)(cubes[order[i / 2]].transform.localRotation.eulerAngles.z / 90f * 8))].GetComponent<Renderer>().material.color != markers[((i + 1) % 2) + 2 * order[(i + 1) / 2] + ((int)(cubes[order[(i + 1) / 2]].transform.localRotation.eulerAngles.z / 90f * 8))].GetComponent<Renderer>().material.color)
                {
                    //print("it has returned false");
                    return false;
                }

                //if (markers[(i % 2) + 2 * order[i / 2] + ((int)(cubes[order[i / 2]].transform.localRotation.eulerAngles.z / 90f * 8))].GetComponent<Renderer>().material.color == markers[((i + 1) % 2) + 2 * order[(i + 1) / 2] + ((int)(cubes[order[(i + 1) / 2]].transform.localRotation.eulerAngles.z / 90f * 8))].GetComponent<Renderer>().material.color)
                //{
                //    print(markers[(i % 2) + 2 * order[i / 2] + ((int)(cubes[order[i / 2]].transform.localRotation.eulerAngles.z / 90f * 8))].GetComponent<Renderer>().material.color);
                //    print(markers[((i + 1) % 2) + 2 * order[(i + 1) / 2] + ((int)(cubes[order[(i + 1) / 2]].transform.localRotation.eulerAngles.z / 90f * 8))].GetComponent<Renderer>().material.color);
                //    //return true;
                //    counter++;
                //}


            }

            print(counter);
        }
        else
        {
            print("this holds");
            return false;
        }

        return true;
        //return false;
    }

    public void selectCube(string cube)
    {

        bool[] stack = new bool[] { false, false, false, false };

        float[] values = new float[] { cubes[0].transform.position.z, cubes[1].transform.position.z, cubes[2].transform.position.z, cubes[3].transform.position.z };
        int[] order = new int[] { 0, 1, 2, 3 };
        for (int i = 0; i < values.Length - 1; i++)
        {
            for (int j = 0; j < values.Length - i - 1; j++)
            {
                if (values[j] > values[j + 1])
                {
                    float temp1 = values[j];
                    values[j] = values[j + 1];
                    values[j + 1] = temp1;

                    int temp2 = order[j];
                    order[j] = order[j + 1];
                    order[j + 1] = temp2;
                }

                //if(values[j] >= values[j + 1] - 0.1f && values[j] <= values[j+1] + 0.1f)
                //{
                //    stack[j] = true;
                //    stack[j + 1] = true;
                //}
            }
        }



        int val = int.Parse(cube.Substring(11)) - 1;
        if (!firstIsPicked)
        {
            firstIsPicked = true;
            cubes[val].transform.position = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y + selectHeight, cubes[val].transform.position.z);
        }
        else
        {
            bool isStacked = false;
            //int pos = 0;
            for(int i = 0; i < values.Length; i++)
            {
                if((cubes[currentCube].transform.position.z >= values[i] - 0.1f && cubes[currentCube].transform.position.z <= values[i] + 0.1f) && order[i] != currentCube)
                {
                    isStacked = true;
                    if (cubes[currentCube].transform.position.z >= cubes[val].transform.position.z - 0.1f && cubes[currentCube].transform.position.z <= cubes[val].transform.position.z + 0.1f)
                    {
                        cubes[currentCube].transform.position = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y - selectHeight, cubes[currentCube].transform.position.z);
                        cubes[val].transform.position = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y + selectHeight, cubes[val].transform.position.z);
                        //pos = i;
                        //break;
                    }
                    else
                    {
                        Vector3 temp1 = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y, cubes[currentCube].transform.position.z);
                        Vector3 temp2 = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y, cubes[val].transform.position.z);

                        cubes[currentCube].transform.position = temp2;
                        cubes[val].transform.position = temp1;
                    }
                    break;
                    //else
                    //{
                        //Vector3 temp1 = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y, cubes[currentCube].transform.position.z);
                        //Vector3 temp2 = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y, cubes[val].transform.position.z);

                        //cubes[currentCube].transform.position = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y - selectHeight, cubes[val].transform.position.z);
                        //cubes[val].transform.position = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y + selectHeight, cubes[val].transform.position.z);
                        //cubes[val].transform.position = temp1;
                        //cubes[currentCube].transform.position = temp2;
                        //break;
                    //}
                    
                }
            }

            if(!isStacked)
            {
                cubes[currentCube].transform.position = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y - selectHeight, cubes[currentCube].transform.position.z);
                cubes[val].transform.position = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y + selectHeight, cubes[val].transform.position.z);
            }
            //cubes[currentCube].transform.position = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y - selectHeight, cubes[currentCube].transform.position.z);
        }
        //cubes[val].transform.position = new Vector3(cubes[val].transform.position.x, cubes[val].transform.position.y + selectHeight, cubes[val].transform.position.z);


        currentCube = val;

        Instantiate(placeSound, cubes[currentCube].transform);

        //if (cubes[currentCube].transform.position.y > baseHeight)
        //{
        //    set = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y - selectHeight, cubes[currentCube].transform.position.z);
        //}

        //cubes[currentCube].transform.position = set;
        //currentCube = val;
        //set = new Vector3(cubes[currentCube].transform.position.x, cubes[currentCube].transform.position.y + selectHeight, cubes[currentCube].transform.position.z);
        //cubes[currentCube].transform.position = set;
        //Instantiate(placeSound, cubes[currentCube].transform);
    }

    public void setPuzzleStart()
    {
        puzzleStarted = true;
    }

    public void setPuzzleExit()
    {
        float[] values = new float[] { cubes[0].transform.position.z, cubes[1].transform.position.z, cubes[2].transform.position.z, cubes[3].transform.position.z };
        int[] order = new int[] { 0, 1, 2, 3 };
        for (int i = 0; i < values.Length - 1; i++)
        {
            for (int j = 0; j < values.Length - i - 1; j++)
            {
                if (values[j] > values[j + 1])
                {
                    float temp1 = values[j];
                    values[j] = values[j + 1];
                    values[j + 1] = temp1;

                    int temp2 = order[j];
                    order[j] = order[j + 1];
                    order[j + 1] = temp2;
                }
            }
        }

        firstIsPicked = false;
        print("test");
        float beginning = min - 0.1f;
        float height = 1.262f;
        for(int i = 0; i < cubes.Length; i++)
        {
            //cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, height, lowest);
            cubes[order[i]].transform.position = new Vector3(cubes[order[i]].transform.position.x, height, lowest);
            lowest += 0.5f;
            //beginning += 0.5f;
        }

        lowest -= 2f;
        puzzleStarted = false;
        currentCube = 0;
        //Start();
    }

    public bool getPuzzleStarted()
    {
        return puzzleStarted;
    }
}
