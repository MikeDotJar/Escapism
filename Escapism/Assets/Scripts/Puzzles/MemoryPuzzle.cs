using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPuzzle : MonoBehaviour
{
	[SerializeField]
	GameObject[] shapes = null; // prefab of shapes that will be used

	[SerializeField]
	GameObject[] buttons = null;

	public Transform beginning;
	public Transform display;
	public int length = 10; // length of puzzle
	public float time = 4f; 
	GameObject[] sequence; // order of shapes they present themselves
	bool puzzleStarted;
	IEnumerator coroutine; // coroutine = timer
	public float difference = 5f;
	float subDifference;

	Color[] colors;

	string[] shapesPattern;
	Color[] colorsPattern;

	int[] selected;
	GameObject[] displayShapes;
	int index = 0;

	int isShapes;

	GameObject[] buttonShapes;

	//int button;

	bool coroutineStarted = true;

	public GameObject buttonSound;

	public Material defaultMaterial;

	bool canQuit;

	// Start is called before the first frame update
	void Start()
	{
		buttonShapes = new GameObject[4];
		displayShapes = new GameObject[length];
		subDifference = difference;

		selected = new int[length];
		isShapes = Random.Range(0, 2);
		//isShapes = 1;

		colors = new Color[4];
		colors[0] = Color.red;
		colors[1] = Color.blue;
		colors[2] = Color.green;
		colors[3] = Color.yellow;

		sequence = new GameObject[length]; // empty sequence array
										   // need to assign prefabs to it
										   // run a loop for length
										   //colors = new Color{ Color.red, Color.blue, Color.green, Color.yellow }; 

		shapesPattern = new string[sequence.Length];

		Transform temp = transform; // object script is attached to

		for (int i = 0; i < length; i++)
		{

			// .Length prevents hardcoding -> get rand pos in shapes array
			sequence[i] = Instantiate(shapes[Random.Range(0, shapes.Length)], beginning);

			sequence[i].transform.position = sequence[i].transform.position + new Vector3(difference,0,-1);
			difference -= .25f;
			sequence[i].GetComponent<Renderer>().material.SetColor("_Color", colors[Random.Range(0, colors.Length)]);
		}

        for (int p = 0; p < length; p++)
		{
			//if(sequence[p].GetComponent<Renderer>().material.GetColor
			shapesPattern[p] = sequence[p].tag;
			//colorsPattern[p] = sequence[p].GetComponent<Renderer>().material.GetColor("_Color");
			//print(colorsPattern[p]);
		}
		
	}
    // Update is called once per frame
    void Update()
    {
        if(puzzleStarted)
        {
			if(coroutineStarted)
            {
				canQuit = false;
				for (int i = 0; i < sequence.Length; i++)
				{
					Vector3 moveShapes = new Vector3(sequence[i].transform.position.x, sequence[i].transform.position.y, sequence[i].transform.position.z + 1f);
					sequence[i].transform.position = moveShapes;
				}
				coroutineStarted = false;
				coroutine = popper(time);
				GameObject.Find("Main Camera").GetComponent<Raycast>().setMemoryPuzzleCoroutine(false);
				StartCoroutine(coroutine);
			}
			
		}
    }


    IEnumerator popper(float time)
	{
		yield return new WaitForSeconds(time);
		for(int i = 0; i < sequence.Length; i++)
        {
			Vector3 move = new Vector3(sequence[i].transform.position.x, sequence[i].transform.position.y, sequence[i].transform.position.z - 1f);
			sequence[i].transform.position = move;
        }
		if (isShapes == 0)
		{
			for (int i = 0; i < shapes.Length; i++)
			{
				GameObject shape = Instantiate(shapes[i], buttons[i].transform);
				Vector3 move = new Vector3(shape.transform.position.x, shape.transform.position.y, shape.transform.position.z + 0.15f);
				shape.transform.position = move;
				buttonShapes[i] = shape;
				shape.GetComponent<Renderer>().material.SetColor("_Color", Color.black);

			}
		}
		else
		{
			for (int i = 0; i < colors.Length; i++)
			{
				buttons[i].GetComponent<Renderer>().material.SetColor("_Color", colors[i]);
			}
		}
		// waits for t seconds and once finished continues code
		// this is called after each individual shape is presented 

		GameObject.Find("Main Camera").GetComponent<Raycast>().setMemoryPuzzleCoroutine(true);
		canQuit = true;
	}

	public void clickButton(string button)
    {
		//isShapes = 0;
		//print(index);
		int b = int.Parse(button.Substring(6)) - 1;
		if(b == 4 && index > 0)
        {
			
			index--;
			Destroy(displayShapes[index]);
			//displayShapes[index].SetActive(false);
			subDifference += .25f;
			//print(subDifference);
			print("backspace " + subDifference);
			//print(index);
		}
		if(b == 5 && index > 0)
        {
			
			for(int i = 0; i < index; i++)
            {
				Destroy(displayShapes[i]);
				//displayShapes[index].SetActive(false);
				subDifference += .25f;
			}
			index = 0;
        }
		if (b == 6)
		{
			bool isCorrect = true;
			if (isShapes == 0)
			{
                    for (int i = 0; i < sequence.Length; i++)
					{
						if(displayShapes[i].tag != sequence[i].tag)
						{
							isCorrect = false;
						}
					}
            }
            else
            {
				for (int i = 0; i < sequence.Length; i++)
				{
					//GetComponent<Renderer>().material.SetColor("_Color", colors[i]);
					if (displayShapes[i].GetComponent<Renderer>().material.GetColor("_Color") != sequence[i].GetComponent<Renderer>().material.GetColor("_Color"))
					{
						isCorrect = false;
					}
				}
			}

			if(isCorrect)
            {
				print("puzzle complete!");
				puzzleStarted = false;
				GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleComplete("MemoryActivator");
				setPuzzleExit();
			}
            else
            {
				GameObject.Find("GameManager").GetComponent<Gamemanager>().puzzleFailed();
				setPuzzleExit();
            }
			//submit
		}
		else
		{
			if (index < length)
			{
				GameObject obj = null;
				if (isShapes == 0)
				{
					obj = Instantiate(shapes[b], display);
					//obj.transform.position = obj.transform.position + new Vector3(display.position.x - subDifference, display.position.y, display.position.z);
					obj.transform.position = obj.transform.position + new Vector3(subDifference, 0, 0);
					subDifference -= .25f;
					obj.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
					displayShapes[index] = obj;
				}
				else
				{
					//obj = Instantiate(shapes[3], display);
					//obj.transform.position = obj.transform.position + new Vector3(display.position.x - subDifference, display.position.y, display.position.z);
					obj = Instantiate(shapes[b], display);
					obj.transform.position = obj.transform.position + new Vector3(subDifference, 0, 0);
					subDifference -= .25f;
					print("input " + subDifference);
					//print(subDifference);
					obj.GetComponent<Renderer>().material.SetColor("_Color", colors[b]);
					displayShapes[index] = obj;
				}

				//Instantiate(shapes[i], buttons[i].transform);
				selected[index] = b;
				index++;
			}
		}

		Instantiate(buttonSound, transform);
			
	}
		

	

	public void setPuzzleStart()
    {
		puzzleStarted = true;
		//GameObject.Find("Main Camera").GetComponent<Raycast>().setStatus("memory",true);
    }

	public void setPuzzleExit()
    {
		
		StopCoroutine(coroutine);

		for(int i = 0; i < sequence.Length; i++)
        {
			Destroy(sequence[i]);
        }

		coroutineStarted = true;
		for (int i = 0; i < index; i++)
		{
			Destroy(displayShapes[i]);
			//displayShapes[index].SetActive(false);
			subDifference += .25f;
			difference += .25f;
		}

		difference = 1f;

		if(isShapes == 0)
        {
			for(int i = 0; i < buttonShapes.Length; i++)
            {
				Destroy(buttonShapes[i]);
            }
        }
        else
        {
			buttons[0].GetComponent<Renderer>().material = defaultMaterial;
			buttons[1].GetComponent<Renderer>().material = defaultMaterial;
			buttons[2].GetComponent<Renderer>().material = defaultMaterial;
			buttons[3].GetComponent<Renderer>().material = defaultMaterial;
		}
		index = 0;
		//GameObject.Find("Main Camera").GetComponent<Raycast>().setStatus("memory", false);
		Start();
	}

	public bool checkIfCanQuit()
    {
		return canQuit;
    }
}
