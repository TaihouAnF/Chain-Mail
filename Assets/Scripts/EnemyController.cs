using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Enemy Attribute")]
    public float Speed = 1.0f;
    public float ShootCoolDown = 2.0f;
    public Centipede centipede;
    // Start is called before the first frame update
    private void Awake() 
    { 
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = GridPosition(targetHead.transform.position);
        Vector2 currPos = transform.position;
        float currSpeed = Speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currPos, targetPos, currSpeed);
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
}