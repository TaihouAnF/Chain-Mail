using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;

    public float Speed = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

    }
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += direction.normalized * Speed * Time.fixedDeltaTime;
        rb.MovePosition(position);

    }
}
