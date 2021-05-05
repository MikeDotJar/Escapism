using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Text timeText;
    public Toggle tutorialToggle;

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start()
    {
        //Cursor.visible = true;
        
        DontDestroyOnLoad(GameObject.Find("HouseTutorialOption"));
    }

    public InputField time;
    public InputField overrides;

    public void PlayGame()
    {
        bool tutorial = tutorialToggle.isOn;
        if (tutorial)
        {
            GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTutorial(true);
        }
        else
        {
            GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTutorial(false);
        }

        string temp = timeText.text.Substring(0, 1);
        print(temp);
        int time = int.Parse(temp);
        GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTime(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
            newTime = int.Parse(temp.Substring(0,1));
        }
        else
        {
            newTime = int.Parse(temp);
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
        }
        else
        {
            newTime = int.Parse(temp);
        }

        time.text = (newTime - 1).ToString() + ":" + "00";

        //GameObject.Find("HouseTutorialOption").GetComponent<Tutorial>().setTime(newTime - 1);
    }

    public void AddOverrides()
    {
        int newOverride = int.Parse(overrides.text) + 1;
        overrides.text = (newOverride).ToString();
        //overrides.text = overrides.text + 1;
    }

    public void SubtractOverrides()
    {
        int newOverride = int.Parse(overrides.text) - 1;
        overrides.text = (newOverride).ToString();
        //overrides.text = overrides.text - 1;
    }
}
