using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float slowerMoveSpeed;
    public float fasterMoveSpeed;
    public Rigidbody2D rb2d;
    public PlayerInputActions playerControls;
    public bool moveRight;
    public bool moveLeft;
    public bool moveUp;
    public bool moveDown;
    public int playerDirection;
    public GameObject faceRight;
    public GameObject faceLeft;
    public GameObject faceUp;
    public GameObject faceDown;
    public Animator playerAnimations;
    public float moveSpeed;

    public InventoryManager inventoryManager;

    private Vector2 moveDirection;
    private InputAction move;
    private InputAction moveFaster;
    private Vector2Int facingDirection = Vector2Int.down;

    private InputAction useTool;
    private InputAction interact;

    public int currentEnergy;
    public int maxEnergy;
    public EnergyBar energyBar;
    public Bed bedPosition;
    public bool fainted;

    private void Awake() 
    {
        playerControls = new PlayerInputActions();

        inventoryManager = GetComponent<InventoryManager>();

        currentEnergy = maxEnergy;
        energyBar.UpdateBar(currentEnergy);
    }

    private void OnEnable() 
    {
        move = playerControls.Player.Move;
        move.Enable();

        moveFaster = playerControls.Player.MoveFaster;
        moveFaster.Enable();
        moveFaster.performed += MoveFaster;

        useTool = playerControls.Player.UseTool;
        useTool.Enable();
        useTool.performed += UseTool;

        interact = playerControls.Player.Interact;
        interact.Enable();
    }

    private void OnDisable() 
    {
        move.Disable();
        moveFaster.Disable();

        useTool.Disable();
        interact.Disable();
    }

    // Update is called once per frame
    void Update() 
    {
        Move();
        TileManager.instance.HighlightTile(transform.position, facingDirection, inventoryManager);
    }

    #region Movement
    private void Move()
    {
        moveDirection = move.ReadValue<Vector2>();

        // Not moving diagonally
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            moveDirection.y = 0;
            rb2d.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            moveDirection.x = 0;
            rb2d.linearVelocity = moveDirection * moveSpeed;
        }

        // Move Right
        if (moveDirection.x > 0)
        {
            moveRight = true;
            faceRight.SetActive(true);
            faceLeft.SetActive(false);
            faceUp.SetActive(false);
            faceDown.SetActive(false);
            playerAnimations.Play("WalkRight");

            facingDirection = Vector2Int.right;
        }
        else
        {
            moveRight = false;
        }
        // Move Left
        if (moveDirection.x < 0)
        {
            moveLeft = true;
            faceLeft.SetActive(true);
            faceRight.SetActive(false);
            faceUp.SetActive(false);
            faceDown.SetActive(false);
            playerAnimations.Play("WalkLeft");

            facingDirection = Vector2Int.left;
        }
        else
        {
            moveLeft = false;
        }
        // Move Up
        if (moveDirection.y > 0)
        {
            moveUp = true;
            faceUp.SetActive(true);
            faceRight.SetActive(false);
            faceLeft.SetActive(false);
            faceDown.SetActive(false);
            playerAnimations.Play("WalkUp");

            facingDirection = Vector2Int.up;
        }
        else
        {
            moveUp = false;
        }
        // Move Down
        if (moveDirection.y < 0)
        {
            moveDown = true;
            faceDown.SetActive(true);
            faceRight.SetActive(false);
            faceLeft.SetActive(false);
            faceUp.SetActive(false);
            playerAnimations.Play("WalkDown");

            facingDirection = Vector2Int.down;
        }
        else
        {
            moveDown = false;
        }
        if (moveDirection.y == 0 && moveDirection.x == 0)
        {
            playerAnimations.Play("PlayerIdle");
        }
    }

    private void MoveFaster(InputAction.CallbackContext context) 
    {
        // Changes move speed when button is pressed
        if (moveSpeed == slowerMoveSpeed) 
        {
            moveSpeed = fasterMoveSpeed;
            playerAnimations.speed *= 2;
        }
        else 
        {
            moveSpeed = slowerMoveSpeed;
            playerAnimations.speed = 1;
        }
    }

    public void SetCanMove(bool canMove)
    {
        if (canMove)
        {
            move.Enable();
            moveFaster.Enable();
            useTool.Enable();
        }
        else
        {
            move.Disable();
            moveFaster.Disable();
            useTool.Disable();

            rb2d.linearVelocity = Vector2.zero; // Stop movement immediately
        }
    }
    #endregion

    private void UseTool(InputAction.CallbackContext context)
    {
        if (TileManager.instance != null && !UI_Manager.instance.inventoryPanel.activeSelf && !UI_Manager.instance.vendorPanel.activeSelf)
        {
            Vector3Int targetTile = TileManager.instance.GetTargetTile(transform.position, facingDirection);

            if (inventoryManager.toolbar.selectedSlot != null &&
                inventoryManager.toolbar.selectedSlot.itemData != null)
            {
                inventoryManager.toolbar.selectedSlot.itemData.Use(targetTile);

                ExtractEnergy(5); // Use energy when use tool
            }
        }
    }

    public void SubscribeToInteract(System.Action<InputAction.CallbackContext> action)
    {
        interact.performed += action;
    }

    public void UnsubscribeFromInteract(System.Action<InputAction.CallbackContext> action)
    {
        interact.performed -= action;
    }

    public void ExtractEnergy(int amount)
    {
        currentEnergy -= amount;
        energyBar.UpdateBar(currentEnergy);

        if (currentEnergy <= 0)
        {
            SetCanMove(false);

            // fade screen to black
            StartCoroutine(ScreenTransition.Instance.FadeNightBackground(() => {

                // teleport player to bed
                this.gameObject.transform.position = bedPosition.transform.position;

            }, true));

            // progress to next day (automatisch?)

            // energy is half full :<
            fainted = true;
        }
    }

    /// <summary>
    /// Reset after going to bed.
    /// </summary>
    public void ResetEnergy()
    {
        if (fainted)
        {
            currentEnergy = maxEnergy / 2;
            fainted = false;
        }
        else
        {
            currentEnergy = maxEnergy;
        }

        energyBar.UpdateBar(currentEnergy);
    }
}
