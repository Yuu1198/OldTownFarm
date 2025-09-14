using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropData data;
    private int currentGrowthTime = 0;
    private int currentGrowthStage = 0;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DayNightCycle.Instance.OnHourChanged.AddListener(OnHourChanged);
        spriteRenderer.sprite = data.growthStages[0];
    }

    private void OnHourChanged(int hour)
    {
        if (++currentGrowthTime >= data.timeToGrow)
        {
            Debug.Log("Growth time reached");
            if (currentGrowthStage < data.growthStages.Count-1)
            {
                Debug.Log("Increasing growth stage");
                spriteRenderer.sprite = data.growthStages[++currentGrowthStage];
            }
            currentGrowthTime = 0;
        }
    }
}
