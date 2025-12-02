using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private Slider slider;

    [SerializeField]
    private Color greenEnergyColor;
    [SerializeField]
    private Color yellowEnergyColor;
    [SerializeField]
    private Color redEnergyColor;
    [SerializeField]
    private Image fillImage;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void UpdateBar(int energyValue)
    {
        slider.value = energyValue;

        if (energyValue >= 50)
        {
            fillImage.color = greenEnergyColor;
        }
        else if (energyValue >= 20)
        {
            fillImage.color = yellowEnergyColor;
        }
        else
        {
            fillImage.color = redEnergyColor;
        }
    }
}
