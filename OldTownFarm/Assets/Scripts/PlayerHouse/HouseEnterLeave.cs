using UnityEngine;

public class HouseEnterLeave : MonoBehaviour
{
    [SerializeField] private GameObject spawnPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = spawnPosition.transform.position;
        }
    }
}
