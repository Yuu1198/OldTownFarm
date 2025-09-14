using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Crop Data")]
public class CropData : ScriptableObject
{
    public int timeToGrow = 10; // TODO: which unit?
    public List<HarvestItem> harvestItems;

    public List<Sprite> growthStages;
}

[Serializable]
public class HarvestItem
{
    public Item item;
    public int yield;
}
