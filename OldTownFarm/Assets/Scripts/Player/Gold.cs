using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Gold : MonoBehaviour
{
    public int gold;
    public int silver;
    public int copper;

    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Convert copper or silver to gold.
    /// </summary>
    private void CalculateCurrentGold()
    {
        if (copper >= 100)
        {
            int remainingCopper = copper % 100;
            int silverFromCopper = copper / 100;

            copper = remainingCopper;
            silver += silverFromCopper;
        }

        if (silver >= 100)
        {
            int remainingSilver = silver % 100;
            int goldFromCopper = silver / 100;

            silver = remainingSilver;
            gold += goldFromCopper;
        }
    }

    /// <summary>
    /// Update UI based on current gold.
    /// </summary>
    public void UpdateUI()
    {
        CalculateCurrentGold();
        goldText.text = gold.ToString() + "g" + silver.ToString() + "s" + copper.ToString() + "c";
    }
}
