using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Transform parent;
    [SerializeField] private EnemyController enemy;

    public float Speed = 50f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        col = GetComponent<Collider2D>();
        col.enabled = false;

        parent = transform.parent;
        if (enemy != null) 
        {
            enemy.OnAttack += OnAttackHappened;
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

    /// <summary>
    /// An Observing method to subscribe to enemy attack.
    /// </summary>
    private void OnAttackHappened() {
        if (rb.isKinematic) 
        {
            transform.SetParent(null);
            rb.bodyType = RigidbodyType2D.Dynamic;
            col.enabled = true;
        }
    }

    private void OnDestroy()
    {
        if (enemy != null) 
        {
            enemy.OnAttack -= OnAttackHappened;
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
