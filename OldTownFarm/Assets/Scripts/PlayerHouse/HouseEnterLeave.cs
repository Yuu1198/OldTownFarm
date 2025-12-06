using UnityEngine;

public class HouseEnterLeave : MonoBehaviour
{
    [SerializeField] private GameObject spawnPosition;
    public bool isEnteringHouse = false;

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
                    collision.transform.position = spawnPosition.transform.position;
                    player.SetCanMove(true); // Player can move again
                    if (isEnteringHouse)
                    {
                        Weather.instance.rainParticles.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (Weather.instance.GetCurrentWeatherData().isRaining)
                        {
                            Weather.instance.rainParticles.gameObject.SetActive(true);
                            Weather.instance.rainParticles.Simulate(100);
                            Weather.instance.rainParticles.Play();
                        }
                    }
                }));
            }

            
        }
    }
}
