using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
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

    private Vector2 moveDirection;
    private InputAction move;

    private void Awake() {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable() {
        move.Disable();
    }

    // Update is called once per frame
    void Update() {
        moveDirection = move.ReadValue<Vector2>();

        // Not moving diagonally
        if(Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y)) {
            moveDirection.y = 0;
            rb2d.linearVelocity = moveDirection * moveSpeed;
        } else {
            moveDirection.x = 0;
            rb2d.linearVelocity = moveDirection * moveSpeed;
        }

        // Move Right
        if(moveDirection.x > 0) {
            moveRight = true;
            faceRight.SetActive(true);
            faceLeft.SetActive(false);
            faceUp.SetActive(false);
            faceDown.SetActive(false);
            playerAnimations.Play("WalkRight");
        } else {
            moveRight = false;
        }
        // Move Left
        if (moveDirection.x < 0) {
            moveLeft = true;
            faceLeft.SetActive(true);
            faceRight.SetActive(false);
            faceUp.SetActive(false);
            faceDown.SetActive(false);
            playerAnimations.Play("WalkLeft");
        } else {
            moveLeft = false;
        }
        // Move Up
        if (moveDirection.y > 0) {
            moveUp = true;
            faceUp.SetActive(true);
            faceRight.SetActive(false);
            faceLeft.SetActive(false);
            faceDown.SetActive(false);
            playerAnimations.Play("WalkUp");
        } else {
            moveUp = false;
        }
        // Move Down
        if (moveDirection.y < 0) {
            moveDown = true;
            faceDown.SetActive(true);
            faceRight.SetActive(false);
            faceLeft.SetActive(false);
            faceUp.SetActive(false);
            playerAnimations.Play("WalkDown");
        } else {
            moveDown = false;
        }
        if (moveDirection.y == 0 && moveDirection.x == 0) {
            playerAnimations.Play("PlayerIdle");
        }
    }
}
