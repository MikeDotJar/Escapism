using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField]
    Transform[] tiles = null; // tile at pos 0 is the activator, tile at pos 15 is the empty space

    [SerializeField]
    Transform[] buttons = null;

    private int currentEmptyTile = 15;
    private int currentActivator = 0;

    public float moveRate = 0f;
    private float moverateTimeStamp = 0f;

    bool puzzleStarted;
    bool randomize;

    public GameObject buttonSound;
    public GameObject slideSound;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            int rand = Random.Range(0, buttons.Length);
            Vector3 temp1 = new Vector3(buttons[i].position.x, buttons[i].position.y, buttons[i].position.z);
            Vector3 temp2 = new Vector3(buttons[rand].position.x, buttons[rand].position.y, buttons[rand].position.z);

            buttons[i].position = temp2;
            buttons[rand].position = temp1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if(puzzleStarted)
        {

            if (!randomize)
            {
                RandomizePositions(tiles[0], tiles[15]);
                randomize = true;
            }

            if (GetActivator() == 3)
            {
                GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete("SliderActivator");
                UnityEngine.Debug.Log("Complete");
                puzzleStarted = false;
            }
        }
        
    }

    void Display(int[] arr)
    {
        for(int i = 0; i < arr.Length; i++)
        {
            UnityEngine.Debug.Log(arr[i]);
        }
    }

    void RandomizePositions(Transform tile1, Transform tile2)
    {

        //UnityEngine.Debug.Log(tilePositions[0]);

        int val1 = 0;
        int val2 = 15;

        while(val1 == 0 || val1 == 3 || val1 == 15)
            val1 = Random.Range(1, 15);

        while(val2 == 15 || val2 == val1 || val1 == 0)
            val2 = Random.Range(1, 15);

        SwitchTiles(tiles[0], tiles[val1]);
        SwitchTiles(tiles[15], tiles[val2]);

        //print("Val1: " + val1 + " Val2: " + val2);
        //print(val2);

        Swap(val1, 0, tiles);
        Swap(val2, 15, tiles);

        StartEmptyTile(val2);
        StartActivator(val1);

        //UnityEngine.Debug.Log(tilePositions[0]);
    }

    void SwitchTiles(Transform tile1, Transform tile2)
    {
        Vector3 temp1 = tile1.position;
        Vector3 temp2 = tile2.position;

        tile1.position = temp2;
        tile2.position = temp1;

    }

    void Swap(int val1, int val2, Transform[] tiles)
    {
        Transform temp = tiles[val1];
        tiles[val1] = tiles[val2];
        tiles[val2] = temp;
        Instantiate(slideSound, temp);
    }

    public void Move(string direction)
    {
        int emptyTile = GetEmptyTile();

        if (direction == "Up")
        {

            if (Time.time > moverateTimeStamp && Check(2))
            {
                if (emptyTile + 4 == GetActivator())
                {
                    SetActivator(-4);
                }
                Instantiate(buttonSound,buttons[0].transform);
                SwitchTiles(tiles[emptyTile], tiles[emptyTile + 4]);
                Swap(emptyTile, emptyTile + 4, tiles);
                //emptyTile += 4;
                SetEmptyTile(4);

                moverateTimeStamp = Time.time + moveRate;
            }

        }

        if (direction == "Down")
        {
            if (Time.time > moverateTimeStamp && Check(1))
            {
                if (emptyTile - 4 == GetActivator())
                {
                    SetActivator(4);
                }
                Instantiate(buttonSound, buttons[0].transform);
                SwitchTiles(tiles[emptyTile], tiles[emptyTile - 4]);
                Swap(emptyTile, emptyTile - 4, tiles);
                //emptyTile -= 4;
                SetEmptyTile(-4);
                moverateTimeStamp = Time.time + moveRate;
            }
        }

        if (direction == "Left")
        {
            if (Time.time > moverateTimeStamp && emptyTile != 3 && emptyTile != 7 && emptyTile != 11 && emptyTile != 15 && Check(3))
            {
                if (emptyTile + 1 == GetActivator())
                {
                    SetActivator(-1);
                }
                Instantiate(buttonSound, buttons[0].transform);
                SwitchTiles(tiles[emptyTile], tiles[emptyTile + 1]);
                Swap(emptyTile, emptyTile + 1, tiles);
                //emptyTile++;
                SetEmptyTile(1);
                moverateTimeStamp = Time.time + moveRate;
            }
        }

        if (direction == "Right")
        {
            if (Time.time > moverateTimeStamp && emptyTile != 0 && emptyTile != 4 && emptyTile != 8 && emptyTile != 12 && Check(4))
            {
                if (emptyTile - 1 == GetActivator())
                {
                    SetActivator(1);
                }
                Instantiate(buttonSound, buttons[0].transform);
                SwitchTiles(tiles[emptyTile], tiles[emptyTile - 1]);
                Swap(emptyTile, emptyTile - 1, tiles);
                //emptyTile--;
                SetEmptyTile(-1);
                moverateTimeStamp = Time.time + moveRate;
            }
        }

        print(emptyTile);
    }

    bool Check(int direction)
    {
        int checker;

        switch(direction)
        {
            case 1:
                checker = GetEmptyTile() - 4; // down
                break;
            case 2:
                checker = GetEmptyTile() + 4; // up
                break;
            case 3:
                checker = GetEmptyTile() + 1; // right
                break;
            default:
                checker = GetEmptyTile() - 1; // left
                break;
        }

        //UnityEngine.Debug.Log(checker);

        if (checker <= 15 && checker >= 0)
            return true;
        else
            return false;
    }

    int GetEmptyTile()
    {
        return currentEmptyTile;
    }

    void SetEmptyTile(int val)
    {
        currentEmptyTile += val;
    }

    void StartEmptyTile(int val)
    {
        currentEmptyTile = val;
    }

    int GetActivator()
    {
        return currentActivator;
    }

    void SetActivator(int val)
    {
        currentActivator += val;
    }

    void StartActivator(int val)
    {
        currentActivator = val;
    }

    bool IsCompleted()
    {
        return false;
    }

    public void setPuzzleStart()
    {
        puzzleStarted = true;
    }

    public void setPuzzleExit()
    {
        puzzleStarted = false;

        SwitchTiles(tiles[0], tiles[GetActivator()]);
        SwitchTiles(tiles[15], tiles[GetEmptyTile()]);

        Swap(GetActivator(), 0, tiles);
        Swap(GetEmptyTile(), 15, tiles);

        StartEmptyTile(GetActivator());
        StartActivator(GetEmptyTile());

        currentEmptyTile = 15;
        currentActivator = 0;

        UnityEngine.Debug.Log(puzzleStarted);
        randomize = false;
        Start();
    }

    public bool getPuzzleStarted()
    {
        return puzzleStarted;
    }
}
