using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    public InventoryManager inventory;

    private Vector2 moveDirection;
    private InputAction move;
    private InputAction moveFaster;

    private InputAction useTool;

    private void Awake() 
    {
        playerControls = new PlayerInputActions();

        inventory = GetComponent<InventoryManager>();
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
    }

    private void OnDisable() 
    {
        move.Disable();
        moveFaster.Disable();

        useTool.Disable();
    }

    // Update is called once per frame
    void Update() 
    {
        Move();
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
    #endregion

    private void UseTool(InputAction.CallbackContext context)
    {
        Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0); // Player position (PLACEHOLDER: Change to tile in front of player)

        // Set Tile to Dirt Tile if Tile is interactable
        if (GameManager.instance.tileManager.IsInteractable(position))
        {
            GameManager.instance.tileManager.SetInteracted(position); 
        }
    }

    // Drop Item in world (PLACEHOLDER)
    public void DropItem(Item item)
    {
        Vector3 spawnLocation = transform.position;

        // Offset
        Vector3 spawnOffset = Random.insideUnitCircle * 1.5f;

        // Spawn Item
        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        // Make it mory fancy
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }

    // Drop Item in world (PLACEHOLDER)
    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
