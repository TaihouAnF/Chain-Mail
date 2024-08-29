using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public Agent agentPrefab;
    public BoxCollider2D spawnArea;
    public const float spawnCoolDown = 5.0f;
    private float coolDown;

    [Header("Reference")]
    [SerializeField] private Centipede centipede;

    void Awake()
    {
        spawnArea = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop spawning when game ends
        if (centipede.sections.Count == 0) { return; }
        if (coolDown <= 0.0f)
        {
            coolDown = spawnCoolDown;
            // time to spawn one
            // Instantiate();
        }
        else
        {
            coolDown -= Time.deltaTime;
        }
    }
}
