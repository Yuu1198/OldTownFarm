using UnityEngine;

[CreateAssetMenu(menuName = "Weather Data")]
public class WeatherData : ScriptableObject
{
    public WeatherType type;
    public Sprite icon;
    public int probability;
    public float brightness;
    public bool isRaining;
}
