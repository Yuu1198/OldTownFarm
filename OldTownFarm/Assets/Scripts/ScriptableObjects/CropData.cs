using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Crop Data")]
public class CropData : ScriptableObject
{
    public int timePerGrowthStage = 10;
    public List<HarvestItem> harvestItems;

    public List<Tile> growthStages;
    public Tile withered;
}

[Serializable]
public class HarvestItem
{
    public Item item;
    public int yield;
}
