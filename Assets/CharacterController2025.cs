using UnityEngine;

public class CharacterController2025 : MonoBehaviour
{
    Rigidbody rb;
    public float acceleration = 10f;
    public float maxSpeed = 20f;
    public float jumpImpulse = 8f, jumpBoostForce = 8f;

    [Header("Debug Stuff")]
    public bool isGrounded;

    Animator animator;

    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * horizontalSpeed * Time.deltaTime * acceleration;

        float horSpeed = rb.linearVelocity.x;
        horSpeed = Mathf.Clamp(horSpeed, -maxSpeed, maxSpeed);

        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horSpeed;

        rb.linearVelocity = newVelocity;
        if (Mathf.Abs(horizontalSpeed) > 0.1f)
        {
            if (animator) 
            {
                animator?.SetBool("Move", true);
                animator?.SetFloat("Speed", Mathf.Abs(horizontalSpeed));
            }
        }
        else
            if(animator)animator.SetBool("Move", false);



        // test if grounded 
        Collider col = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = col.bounds.extents.y + 0.01f;
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);

        Color color = isGrounded ? Color.green : Color.red;
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) 
        {
            if(animator)animator.SetTrigger("Jump");
           rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
        else if (!isGrounded && Input.GetKey(KeyCode.Space))
        {
            if(rb.linearVelocity.y  > 0f) { }
                rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
        }

        float yawRot;
        if(horizontalSpeed != 0)
            yawRot = (horizontalSpeed > 0f) ? 90f : -90f;
        else 
            yawRot = transform.localEulerAngles.y;
        Quaternion rot = Quaternion.Euler(0f, yawRot, 0f);
        transform.rotation = rot;



        if(transform.position.y < -50f) 
        {
            Debug.Log("You DIIEEDD!" + transform.position.y);
            gameManager.Restart();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Player Wins!");
            gameManager.Restart();
        }

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("You Lose!");
            Destroy(other.gameObject);
            gameObject.SetActive(false);
            gameManager.Restart();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // only if we are beaneath the y value of the brick
        if (transform.position.y > (collision.gameObject.transform.position.y - collision.collider.bounds.extents.y + 0.1f))
            return;
        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject.gameObject); // Destroy the brick
            gameManager.AddPoints(100, 0);
        }
        else if (collision.gameObject.CompareTag("Power Up Brick"))
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("Destroy");
            Destroy(collision.gameObject, 1.1f); // Destroy the brick
            gameManager.AddPoints(100, 1);
        }
    }
}
