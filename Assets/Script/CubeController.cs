using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float speed = 5f;         // Movement speed
    public float jumpForce = 5f;    // Force applied for jumping
    private Rigidbody rb;           // Reference to the Rigidbody component
    private bool isGrounded = true; // Check if the cube is on the ground
    public Animator animator;       // Reference to Animator component

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a movement vector
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * speed * Time.deltaTime;

        // Move the cube
        transform.Translate(movement, Space.World);

        // Rotate the cube to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }

        // Trigger walk animation if moving, otherwise idle
        bool isWalking = movement.magnitude > 0;
        animator.SetBool("walk", isWalking);

        // Jump when the spacebar is pressed and the cube is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevent double jumping
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the cube is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}