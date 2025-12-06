using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;


public enum WeatherType
{
    SUNNY,
    CLOUDY,
    RAIN,
    STORM
}

[System.Serializable]
public struct WeatherDataStruct
{
    public Sprite icon;
    public int probability; // 0-100
    public float brightness;
}

public class Weather : MonoBehaviour
{
    [SerializeField]
    public List<WeatherData> weatherDatabase;

    public ParticleSystem rainParticles;

    [SerializeField]
    private Image weatherUI;

    public WeatherType currentWeather;

    public static Weather instance { get; private set; }

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

    public void Start()
    {
        currentWeather = WeatherType.SUNNY;
        weatherUI.sprite = GetCurrentWeatherData().icon;

        DayNightCycle.instance.OnDayChanged.AddListener(OnDayChanged);
    }

    private void OnDayChanged(int day)
    {
        // Choose Weather
        currentWeather = GetRandomWeatherType();
        weatherUI.sprite = GetCurrentWeatherData().icon;

        if (GetCurrentWeatherData().isRaining)
        {
            rainParticles.Play();
            rainParticles.gameObject.SetActive(false);
        } else
        {
            rainParticles.Stop();
        }

        Debug.Log("Current weather is " + currentWeather);
    }

    public WeatherType GetRandomWeatherType()
    {
        int total = 0;
        foreach (var kvp in weatherDatabase)
            total += kvp.probability;

        int roll = Random.Range(0, total);

        int cumulative = 0;
        foreach (var kvp in weatherDatabase)
        {
            cumulative += kvp.probability;
            if (roll < cumulative)
                return kvp.type;
        }

        return weatherDatabase.Last().type;
    }

    public WeatherData GetCurrentWeatherData()
    {
        return weatherDatabase.First(w => w.type == currentWeather);
    }
}
