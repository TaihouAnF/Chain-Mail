using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Transform parent;

    public float Speed = 50f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        col = GetComponent<Collider2D>();
        col.enabled = false;

        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic && Input.GetButton("Fire1"))
        {
            transform.SetParent(null);
            rb.bodyType = RigidbodyType2D.Dynamic;
            col.enabled = true;
        }
    }
    public void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            Vector2 pos = rb.position;
            pos += Vector2.up * Speed * Time.fixedDeltaTime;
            rb.MovePosition(pos);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
    }
}
