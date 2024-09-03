using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Agent Attribute")]
    public int agentHealth = 1;
    public float speed = 2.0f;
    public Sprite sprite;
    private float minY;
    private float maxY;
    public bool moveLeft;   // True => move left, False => move right
    private Vector2 targetPos;

    [Header("Reference")]
    public AgentController agentController;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    void Start() 
    {
        minY = agentController.homeBound.bounds.min.y;
        maxY = agentController.homeBound.bounds.max.y;
        targetPos = transform.position;
        targetPos = GridPosition(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.1f) 
        {
            // Choose a new target position
            GetNextTargetPos();
        }
        Vector2 currentPos = transform.position;
        float spd = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPos, targetPos, spd);
        CheckBoundary();
    }

    private void GetNextTargetPos() 
    {
        int up = Random.Range(0, 1);
        float y_pos = 0.0f;
        targetPos = transform.position;
        if (up == 1) {
            y_pos = Random.Range(transform.position.y, maxY);
            targetPos.x += moveLeft ? (y_pos - transform.position.y) : 
                                            -(y_pos - transform.position.y);
            targetPos.y = y_pos;
        }
        else 
        {
            y_pos = Random.Range(minY, transform.position.y);
            targetPos.x += moveLeft ? (transform.position.y - y_pos) : 
                                            -(transform.position.y - y_pos);
            targetPos.y = y_pos;
        }
        targetPos = GridPosition(targetPos);
    }

    private void CheckBoundary()
    {
        bool shouldKill = (moveLeft && 
                          transform.position.x < agentController.leftSpawn.position.x) ||
                          (!moveLeft && 
                          transform.position.x > agentController.rightSpawn.position.x);
        if (shouldKill) 
        {
            Debug.Log("Killing Agents.");
            agentController.currentAgent = null;
            Destroy(gameObject);
        }
    }

    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }
}