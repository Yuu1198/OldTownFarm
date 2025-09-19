using UnityEngine;

public class Crop
{
    public CropData data;
    public Vector3Int position { get; private set; }
    public int currentGrowthTime { get; private set; } = 0;
    public int currentGrowthStage { get; private set; } = 0;

    private int timeWithoutWater = 0;
    private int timeHarvestable = 0;

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
        if (!IsWatered() && !IsWithered() && !IsHarvestable())
        {
            if(++timeWithoutWater >= 24) // withering
            {
                CropManager.instance.SetWithered(this);
            }
            return false;
        }
        else if (IsWatered() && !IsWithered())
        {
            timeWithoutWater = 0;
        }

        if (IsHarvestable())
        {
            if (++timeHarvestable >= 24) // withering
            {
                CropManager.instance.SetWithered(this);
            }
            return false;
        }
        else if (IsWithered())
        {
            return false;
        }

        bool grown = false;
        if (++currentGrowthTime >= data.timePerGrowthStage)
        {
            if (currentGrowthStage < data.growthStages.Count - 1)
            {
                ++currentGrowthStage;
                grown = true;
            }
            currentGrowthTime = 0;
        }
        return grown;
    }

    public bool IsHarvestable()
    {
        return currentGrowthStage == data.growthStages.Count - 1;
    }

    public bool IsWatered()
    {
        return TileManager.instance.GetTypeOfTile(position) == TileManager.TileType.Watered;
    }

    public bool IsWithered()
    {
        return timeWithoutWater >= 24;
    }
}
