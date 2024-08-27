using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    public event Action OnAttack;

    [Header("Enemy Attribute")]
    public float BootTime = 3.0f;
    public float Speed = 5.0f;
    public float ShootCoolDown = 2.0f;
    public Centipede centipede;
    private void Awake() 
    { 
        rb = GetComponent<Rigidbody2D>();
        BootTime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {   
        if (BootTime <= 0) {
            UpdateMovement();
            UpdateAttack();
        }
        else 
        {
            BootTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Borrow from CentipedeSection, getting the position in the grid.
    /// </summary>
    /// <param name="position">Current position in 2D.</param>
    /// <returns>Accurate Integer position</returns>
    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    /// <summary>
    /// Enemy movement.
    /// </summary>
    private void UpdateMovement() 
    {
        CentipedeSection randomSection = centipede.GetRandomSection();  // Only when new section required
        Vector2 targetPos = GridPosition(randomSection.transform.position);
        Vector2 currPos = transform.position;
        targetPos.y = currPos.y;
        float currSpeed = Speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currPos, targetPos, currSpeed);
    }

    /// <summary>
    /// Attack Event.
    /// </summary>
    private void Attack() 
    {
        OnAttack?.Invoke();
    }

    /// <summary>
    /// Enemy attack, triggering the attack event for observers.
    /// </summary>
    private void UpdateAttack() 
    {
        if (ShootCoolDown <= 0) 
        {
            Attack();
            ShootCoolDown = 2.0f;
        }
        else 
        {
            ShootCoolDown -= Time.deltaTime;
        }
    }
}