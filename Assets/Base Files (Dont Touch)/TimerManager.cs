using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;
    private GameObject timerObject;
    private Timer timer;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        timerObject = transform.GetChild(0).gameObject;
        timer = timerObject.GetComponent<Timer>();
        timerObject.SetActive(false);
    }

    public void StartTimer(Minigame.GameTime time)
    {
        timerObject.SetActive(true);
        StartCoroutine(timer.GameTimer(time == Minigame.GameTime.Short ? 7 : 15));
    } 

    
}
