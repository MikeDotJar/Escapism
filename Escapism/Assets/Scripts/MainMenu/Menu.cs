using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Text timeText;
    public CharacterController controller;
    public GameObject menuAudio;

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public InputField time;

    public void PlayGame()
    {
        //bool tutorial = tutorialToggle.isOn;
        //if (tutorial)
        //{
        //    GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTutorial(true);
        //}
        //else
        //{
        //    GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTutorial(false);
        //}

        //string temp = timeText.text.Substring(0, 1);
        //print(temp);
        //int time = int.Parse(temp);
        //GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTime(time);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //foreach (GameObject obj in scriptsToEnable)
        //{
        //    MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        //    foreach (MonoBehaviour script in scripts)
        //    {
        //        script.enabled = true;
        //    }

        //}

        MonoBehaviour script1 = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        MonoBehaviour script2 = GameObject.Find("Main Camera").GetComponent<MouseLook>();
        MonoBehaviour script3 = GameObject.Find("Main Camera").GetComponent<Raycast>();

        MonoBehaviour script4 = GameObject.Find("Main Camera").GetComponent<Rotator>();

        script1.enabled = true;
        script2.enabled = true;
        script3.enabled = true;

        script4.enabled = false;

        gameObject.SetActive(false);
        controller.enabled = true;

        try
        {
            Destroy(menuAudio);
        }
        catch
        {
            print("Audio already destroyed");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void QuitGame()
    {
        print("Game Quit");
        Application.Quit();
    }

    public void AddTime()
    {
        string temp = time.text.Substring(0, 2);
        int newTime;
        if (temp.Contains(":"))
        {
            newTime = int.Parse(temp.Substring(0, 1));
        }
        else
        {
            newTime = int.Parse(temp);
            if(newTime > 20)
            {
                newTime = 20;
            }
        }

        time.text = (newTime + 1).ToString() + ":00";

        //GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTime(newTime + 1);

    }

    public void SubtractTime()
    {
        string temp = time.text.Substring(0, 2);
        int newTime;
        if (temp.Contains(":"))
        {
            newTime = int.Parse(temp.Substring(0, 1));
            if (newTime < 2)
            {
                newTime = 2;
            }
            
        }
        else
        {
            newTime = int.Parse(temp);
            if (newTime < 2)
            {
                newTime = 2;
            }
        }

        time.text = (newTime - 1).ToString() + ":" + "00";

    }

    public int getTime()
    {
        return int.Parse(timeText.text.Substring(0, timeText.text.IndexOf(":")));
    }
}
