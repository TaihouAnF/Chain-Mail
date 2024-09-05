using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

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
    public bool isLockedOn = false;     // Target marker

    private HudController HudCanvas;
    private Vector2 direction = Vector2.right + Vector2.down;
    private Directions dir = Directions.right;
    private Vector2 targetPos;

    public AudioSource soundSuccess;
    public bool Dying = false;

    private void Awake()
    {
        SRenderer = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        HudCanvas = GameObject.FindObjectOfType<HudController>();
    }

    private void Update()
    {
        if (!Dying)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Centipede.joystick.Vertical > 0.05)
                dir = Directions.up;
            else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Centipede.joystick.Horizontal > 0.05)
                dir = Directions.right;
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Centipede.joystick.Vertical < -0.05)
                dir = Directions.down;
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Centipede.joystick.Horizontal < -0.05)
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
            //soundSuccess.Play();
        }
    }
    public void UpdateHeadSection()
    {
        //SRenderer.sprite = HeadSprite;
        Vector2 gridPos = GridPosition(transform.position);

        targetPos = gridPos;
        if (targetPos.y >= 17.6)
            targetPos.y = 17;
        if (targetPos.y <= -18.6)
            targetPos.y = -18;
        if (targetPos.x >= 17.6)
            targetPos.x = 17;
        if (targetPos.x <= -17.6)
            targetPos.x = -17;

        if (dir == Directions.up)
            targetPos.y += Vector2.up.y;
        else if (dir == Directions.right)
            targetPos.x += Vector2.right.x;
        else if (dir == Directions.down)
            targetPos.y += Vector2.down.y;
        else if (dir == Directions.left)
            targetPos.x += Vector2.left.x;

        if (Physics2D.OverlapBox(targetPos, Vector2.zero, 0f, Centipede.CollisionMask))
        {
            // Reverse horizontal direction
            if (dir == Directions.right || dir == Directions.left)
            {
                direction.x = -direction.x;

                // Advance to the next row
                targetPos.x = gridPos.x;
                targetPos.y = gridPos.y + direction.y;
            }
            // Reverse horizontal direction
            if (dir == Directions.up || dir == Directions.down)
            {
                direction.y = -direction.y;

                // Advance to the next row
                targetPos.y = gridPos.y;
                targetPos.x = gridPos.x + direction.x;
            }

            
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
            
            this.Centipede.RemoveSectionAndSpawn(this);

        }
        else if (collision.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //collision.enabled = false;
            this.Centipede.RemoveSectionAndSpawn(this);
            collision.gameObject.GetComponent<Barrier>().Damage(1);
        }
        else if (collision.enabled && IsHead && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))   // Hitting the enemy with a head
        {
            // Score Update methods as well as clearing the sections
            GameManager.Instance.UpdateScore(HitEnemy());
        }
    }
    
    /// <summary>
    /// Method that being called after A *HEAD* hitting the enemy, clearing all other sections and returning the score for that 
    /// Centipede.
    /// </summary>
    /// <returns>The score earned by that centipede. </returns>
    private int HitEnemy()
    {
        soundSuccess.Play();
        int score = 0;
        CentipedeSection sec = this;
        while (sec != null) {
            score++;
            CentipedeSection sec_prev = sec;
            sec = sec.Behind;
            Centipede.RemoveSection(sec_prev);
            sec_prev.GetComponent<Animator>().SetTrigger("Die");
        }
        return score;

    }
    public void DestroyAfterAnim()
    {
        Destroy(gameObject);
    }
}
