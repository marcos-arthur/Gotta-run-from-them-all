using UnityEngine;

public class PlayerController : MonoBehaviour
{
     Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // moveSpeed = 5f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        if (movement.magnitude > 1) movement.Normalize();
        movement *= moveSpeed;

        if (movement != Vector2.zero)
            {
                rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
            }
    }
}
