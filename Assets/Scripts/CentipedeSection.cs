using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Directions
{
    up,
    right,
    down,
    left
}

public class CentipedeSection : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer SRenderer;
    [HideInInspector]
    public Centipede Centipede;
    [HideInInspector]
    public CentipedeSection Ahead;
    [HideInInspector]
    public CentipedeSection Behind;
    public bool IsHead => Ahead == null;

    private Vector2 direction = Vector2.right + Vector2.down;
    private Directions dir = Directions.right;
    private Vector2 targetPos;

    private void Awake()
    {
        SRenderer = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
            dir = Directions.up;
        else if(Input.GetKey(KeyCode.D))
            dir = Directions.right;
        else if (Input.GetKey(KeyCode.S))
            dir = Directions.down;
        else if (Input.GetKey(KeyCode.A))
            dir = Directions.left;
        if (IsHead && Vector2.Distance(transform.position, targetPos) < 0.1f) 
        {
            UpdateHeadSection();
        }

        Vector2 currentPos = transform.position;
        float speed = Centipede.CentiSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPos, targetPos, speed);

        Vector2 moveDirection = (targetPos - currentPos).normalized;
        float angle = Mathf.Atan2(moveDirection.x, moveDirection.y);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
    public void UpdateHeadSection()
    {
        Vector2 gridPos = GridPosition(transform.position);

        targetPos = gridPos;
        if(dir == Directions.up)
            targetPos.y -= direction.y;
        else if (dir == Directions.right)
            targetPos.x += direction.x;
        else if (dir == Directions.down)
            targetPos.y += direction.y;
        else if (dir == Directions.left)
            targetPos.x -= direction.x;

        if (Physics2D.OverlapBox(targetPos, Vector2.zero, 0f, Centipede.CollisionMask))
        {
            // Reverse horizontal direction
            direction.x = -direction.x;

            // Advance to the next row
            targetPos.x = gridPos.x;
            targetPos.y = gridPos.y + direction.y;

            //Bounds homeBounds = Centipede.HomeBound.bounds;

            // Reverse vertical direction if the segment leaves the home area
            //if ((direction.y == 1f && targetPos.y > homeBounds.max.y) || (direction.y == -1f && targetPos.y < homeBounds.min.y))
            //{
            //    direction.y = -direction.y;
            //    targetPos.y = gridPos.y + direction.y;
            //}
        }
        if (Behind != null)
            Behind.UpdateBodySection();
    }
    private void UpdateBodySection()
    {
        targetPos = GridPosition(Ahead.transform.position);
        direction = Ahead.direction;

        if (Behind != null)
            Behind.UpdateBodySection();
    }
  
    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            //collision.enabled = false;
            
            this.Centipede.RemoveSection(this);
            if (collision.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                //collision.enabled = false;

                this.Centipede.RemoveSection(this);
                Destroy(collision.gameObject);
            }
        }
    }
}
