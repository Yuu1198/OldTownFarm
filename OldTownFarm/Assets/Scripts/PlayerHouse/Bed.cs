using UnityEngine;

public class Bed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetCanMove(false); // Player cannot move during Transition
                StartCoroutine(ScreenTransition.Instance.FadeOutIn(() =>
                {
                    DayNightCycle.Instance.ProgressToNextDay();
                    player.SetCanMove(true); // Player can move again
                }));
            }  
        }
    }
}
