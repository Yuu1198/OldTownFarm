using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI earnedMoneyText;
    [SerializeField] private Chest chest;

    private PlayerController player;

    [SerializeField] private Button continueButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetCanMove(false); // Player cannot move during Transition

                StartCoroutine(ScreenTransition.Instance.FadeNightBackground(() =>
                {
                    // Fade In Complete

                    // Show Money in chest
                    earnedMoneyText.gameObject.SetActive(true);
                    earnedMoneyText.text = "+ " + chest.chestMoney.GetMoneyString();

                    // Put money from chest to player
                    PlayerMoneyManager.instance.AddMoney(chest.chestMoney);
                    chest.chestMoney = new Currency(0, 0, 0); // Reset money in chest

                    continueButton.interactable = true;

                }, true));
            }  
        }
    }

    // Progress to next day when player presses "Continue Button"
    public void OnContinue()
    {
        continueButton.interactable = false;
        earnedMoneyText.gameObject.SetActive(false);
        
        // Reset energy
        player.ResetEnergy();

        StartCoroutine(ScreenTransition.Instance.FadeNightBackground(() =>
        {
            // Fade Out Completed

            DayNightCycle.instance.ProgressToNextDay();
            player.SetCanMove(true); // Player can move again
            player = null;

        }, false));
    }
}
