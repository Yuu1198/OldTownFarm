using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake() {
        // Create Inventory with Slots
        inventory = new Inventory(21);
    }
}
