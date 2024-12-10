using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float speed = 5f;         // Movement speed
    public float jumpForce = 5f;    // Force applied for jumping
    private Rigidbody rb;           // Reference to the Rigidbody component
    private bool isGrounded = true; // Check if the cube is on the ground

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a movement vector
        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        // Move the cube
        transform.Translate(movement, Space.World);

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