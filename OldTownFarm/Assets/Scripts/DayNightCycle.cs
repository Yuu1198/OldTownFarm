using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay; // Display Time
    public TextMeshProUGUI dayDisplay; // Display Day
    private Light2D globalLight;

    public float tick; // Controls time scale (higher tick = faster time)
    public float mins;
    public int hours;
    public int days = 1;

    public bool activateLights; // checks if lights are on
    public GameObject[] nightLights; // all the lights we want on when its dark

    public UnityEvent<int> OnHourChanged;
    public UnityEvent<int> OnDayChanged;

    private int wakeUpTime = 7;

    public static DayNightCycle Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        globalLight = gameObject.GetComponent<Light2D>();
        globalLight.intensity = 0.005f;
    }

    // Update is called once per frame
    void FixedUpdate() // we used fixed update, since update is frame dependant. 
    {
        CalcTime();
        DisplayTime();
    }

    private void CalcTime()
    {
        mins += Time.fixedDeltaTime * tick;

        if (mins >= 60)
        {
            mins = 0;
            hours += 1;
            OnHourChanged.Invoke(hours);
        }

        if (hours >= 24)
        {
            hours = 0;
            days += 1;
            OnDayChanged.Invoke(days);
        }
        ControlLight(); // changes lighting after calculation
    }

    private void ControlLight()
    {
        if (hours == GetDuskHour()) // Dusk
        {
            globalLight.intensity = 1 - mins / 60.3f;

            if (activateLights == false)
            {
                if (mins > 45)
                {
                    for (int i = 0; i < nightLights.Length; i++)
                    {
                        nightLights[i].SetActive(true);
                    }
                    activateLights = true;
                }
            }
        }


        if (hours == GetDawnHour()) // Dawn
        {
            globalLight.intensity = 0.005f + mins / 60.3f;
            if (activateLights == true)
            {
                if (mins > 30)
                {
                    for (int i = 0; i < nightLights.Length; i++)
                    {
                        nightLights[i].SetActive(false);
                    }
                    activateLights = false;
                }
            }
        }
    }

    public int GetDuskHour()
    {
        return 21; // hardcoded for now, customizable for seasons integration at a later time
    }

    public int GetDawnHour()
    {
        return 6; // hardcoded for now, customizable for seasons integration at a later time
    }

    public void DisplayTime() // Shows time and day in UI
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
        dayDisplay.text = "Day " + days; // display day counter
    }

    public void ProgressToNextDay()
    {
        days++;
        OnDayChanged.Invoke(days);
        hours = wakeUpTime;
        OnHourChanged.Invoke(hours);

        ControlLight();
    }
}