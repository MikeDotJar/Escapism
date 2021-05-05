using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    bool tutorial = true;

    int time = 10;

    public bool getIfTutorial()
    {
        return tutorial;
    }

    public void setTutorial(bool status)
    {
        tutorial = status;
    }

    public void  switchTutorial()
    {
        tutorial = !tutorial;
    }

    public void setTime(int time)
    {
        this.time = time;
    }

    public int getTime()
    {
        return time;
    }
}
