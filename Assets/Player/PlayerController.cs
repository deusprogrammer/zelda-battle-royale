using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;
    private Vector2 moveInput;

    private float moveInputTimeout = 0.1f; // Time in seconds to wait before considering movement stopped
    private float moveInputTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        this.transform.Translate(move, Space.World);

        // Rotate the player to face the direction of movement
        if (moveInput != Vector2.zero) {
            Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, moveSpeed * Time.deltaTime);
        }

        // Check if moveInput has stopped
        if (moveInput != Vector2.zero) {
            moveInputTimer = moveInputTimeout;
        } else {
            moveInputTimer -= Time.deltaTime;
            if (moveInputTimer <= 0) {
                OnMoveStopped();
            }
        }
    }

    void OnMove(InputValue inputValue) {
        this.moveInput = inputValue.Get<Vector2>();
        if (!animator.GetBool("running")) {
            animator.SetBool("running", true);
        }
    }

    void OnAttack() {
        animator.SetTrigger("attacking");
    }

    void OnMoveStopped() {
        animator.SetBool("running", false);
    }
}
