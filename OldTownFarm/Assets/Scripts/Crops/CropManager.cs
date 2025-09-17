using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropManager : MonoBehaviour
{
    public static CropManager instance;

    [SerializeField] private Tilemap cropMap;

    private List<Crop> cropList = new();

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

    private void Start()
    {
        DayNightCycle.instance.OnHourChanged.AddListener(OnHourChanged);
    }

    private void OnHourChanged(int hour)
    {
        foreach (var crop in cropList)
        {
            if (crop.Grow())
            {
                cropMap.SetTile(crop.position, crop.data.growthStages[crop.currentGrowthStage]);
            }
        }
    }

    public void SeedCrop(Vector3Int position, CropData crop)
    {
        cropList.Add(new Crop(crop, position));
        cropMap.SetTile(position, crop.growthStages[0]);
    }

    public Crop GetCropAtTile(Vector3Int position)
    {
        foreach(var crop in cropList)
        {
            if (crop.position == position)
            {
                return crop;
            }  
        }

        return null;
    }

    public void RemoveCrop(Crop crop)
    {
        if (cropList.Contains(crop))
        {
            cropList.Remove(crop);
            cropMap.SetTile(crop.position, null);
        }
    }
}
