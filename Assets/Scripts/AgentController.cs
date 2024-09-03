using System.Collections;
using System.Collections.Generic;
using UnityEditor.Il2Cpp;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [Header("Spider Attributes")]
    public float spawnCoolDown = 10.0f;
    [HideInInspector]  public float coolDown;

    [Header("Reference")]
    public Agent agentPrefab;
    public Transform leftSpawn;
    public Transform rightSpawn;
    public Transform upperBound;
    public Transform lowerBound;
    public Centipede centipede;
    public Agent currentAgent;    // Temporary as one obj, could be list later

    private void Start()
    {
        coolDown = spawnCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop spawning when game ends
        if (centipede.sections.Count == 0 || currentAgent != null) { return; }
        //Debug.Log(centipede.sections.Count);
        if (coolDown <= 0)
        {
            Debug.Log("Spawning");
            coolDown = spawnCoolDown;
            // time to spawn one
            int spawnCase = Random.Range(0, 2);
            if (spawnCase == 0) // Spawn right and move left
            {   
                currentAgent = Instantiate(agentPrefab, rightSpawn.position, Quaternion.identity);
                currentAgent.agentController = this;
                currentAgent.moveLeft = true;  
            }
            else // Spawn left and move right
            {
                currentAgent = Instantiate(agentPrefab, leftSpawn.position, Quaternion.identity);
                currentAgent.agentController = this;
                currentAgent.moveLeft = false;
            }
        }
        else
        {
             coolDown -= Time.deltaTime;
        }
    }
}
