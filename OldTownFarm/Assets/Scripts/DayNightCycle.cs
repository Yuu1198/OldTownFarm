using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private float brightness;

    public float tick; // Controls time scale (higher tick = faster time)
    public float mins;
    public int hours;
    public int days = 1;

    public bool activateLights; // checks if lights are on
    public GameObject[] nightLights; // all the lights we want on when its dark

    public UnityEvent<int> OnHourChanged;
    public UnityEvent<int> OnDayChanged;

    private int wakeUpTime = 7;

    public float minDelay = 3f;
    public float maxDelay = 10f;
    public float flashIntensity = 8f;
    private float lightingTimer = 5f;
    

    public static DayNightCycle instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
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

    /// <summary>
    /// Resets lighting timer to random number.
    /// </summary>
    private void ResetLightingTimer()
    {
        lightingTimer = Random.Range(minDelay, maxDelay);
    }

    /// <summary>
    /// LIGHTNING
    /// </summary>
    /// <returns></returns>
    IEnumerator FLASH()
    {
        // first BOOM
        globalLight.intensity = flashIntensity;
        yield return new WaitForSeconds(0.06f);

        globalLight.intensity = brightness;
        yield return new WaitForSeconds(0.05f);

        // second BOOM
        globalLight.intensity = flashIntensity * 0.75f;
        yield return new WaitForSeconds(0.1f);

        globalLight.intensity = brightness;
    }

    private void ControlLight()
    {
        brightness = Weather.instance.GetCurrentWeatherData().brightness;

        // STORM
        if (Weather.instance.currentWeather == WeatherType.STORM)
        {
            // Update timer
            lightingTimer -= Time.deltaTime;
            if (lightingTimer <= 0)
            {
                // BOOM
                StartCoroutine(FLASH());

                ResetLightingTimer();
            }
        }

        if (hours == GetDawnHour()) // Dawn
        {
            globalLight.intensity = (0.005f + mins / 60.3f) * brightness;
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
        else if (hours > GetDawnHour() && hours < GetDuskHour() && globalLight.intensity != brightness && globalLight.intensity <= 1)
        {
            globalLight.intensity = brightness;
        }
        else if (hours == GetDuskHour()) // Dusk
        {
            globalLight.intensity = (1 - mins / 60.3f) * brightness;

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
        else if ((hours > GetDuskHour() || hours < GetDawnHour()) && globalLight.intensity > 0.005f * brightness)
        {
            globalLight.intensity = 0.005f * brightness;
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
        mins = 0;
        if (hours >= wakeUpTime)
        {
            while (hours < 24)
            {
                OnHourChanged.Invoke(++hours);
            }
            hours = 0;
        }
        while (hours < wakeUpTime)
        {
            OnHourChanged.Invoke(++hours);
        }
        OnDayChanged.Invoke(days);

        ControlLight();
    }
}
