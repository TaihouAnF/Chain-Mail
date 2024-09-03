using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [Header("Agent Attribute")]
    public int agentHealth = 1;
    public float speed = 2.0f;
    public Sprite sprite;

    private Rigidbody2D rb;
    private Collider2D col;
    

    private SpriteRenderer spriteRenderer;
    private Vector2 direction = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
