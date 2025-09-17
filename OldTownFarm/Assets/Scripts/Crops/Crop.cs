using UnityEngine;

public class Crop
{
    public CropData data;
    public Vector3Int position { get; private set; }
    public int currentGrowthTime { get; private set; } = 0;
    public int currentGrowthStage { get; private set; } = 0;

    public bool harvestable = false;

    public Crop(CropData data, Vector3Int position)
    {
        this.data = data;
        this.position = position;
    }

    /*
     * Called each hour
     */
    public bool Grow()
    {
        bool grown = false;
        if (++currentGrowthTime >= data.timePerGrowthStage)
        {
            Debug.Log("Growth time reached");
            if (currentGrowthStage < data.growthStages.Count - 1)
            {
                ++currentGrowthStage;
                Debug.Log("Increasing growth stage");
                grown = true;
            }
            else
            {
                harvestable = true; // Ready for the harvest
            }
            currentGrowthTime = 0;
        }
        return grown;
    }
}
