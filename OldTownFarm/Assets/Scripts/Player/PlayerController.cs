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

    public InventoryManager inventoryManager;

    private Vector2 moveDirection;
    private InputAction move;
    private InputAction moveFaster;
    private Vector2Int facingDirection = Vector2Int.down;

    private InputAction useTool;

    private TileManager tileManager;

    private void Awake() 
    {
        playerControls = new PlayerInputActions();

        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
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
        tileManager.HighlightTile(transform.position, facingDirection, inventoryManager);
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
    #endregion

    private void UseTool(InputAction.CallbackContext context)
    {
        if (tileManager != null)
        {

            Vector3Int targetTile = tileManager.GetTargetTile(transform.position, facingDirection);

            // Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0); // Player position (PLACEHOLDER: Change to tile in front of player)

            // REFACTURE: Dont let player handle the tiles
            string tileName = tileManager.GetTileName(targetTile);

            if (!string.IsNullOrWhiteSpace(tileName))
            {
                if (tileName == "interactable" && inventoryManager.toolbar.selectedSlot != null && inventoryManager.toolbar.selectedSlot.itemName == "Hoe") // REFACTURE: no string Hoe here make it a enum with all tools
                {
                    tileManager.SetInteracted(targetTile);
                }
            }
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
