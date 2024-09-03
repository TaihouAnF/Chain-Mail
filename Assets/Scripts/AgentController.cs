using System.Collections;
using System.Collections.Generic;
using UnityEditor.Il2Cpp;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [Header("Spider Attributes")]
    public float spawnCoolDown = 1.0f;
    private float coolDown;

    [Header("Reference")]
    public Agent agentPrefab;
    public Transform leftSpawn;
    public Transform rightSpawn;
    public BoxCollider2D homeBound;
    public Centipede centipede;
    public Agent currentAgent;    // Temporary as one obj, could be list later

    // void Start() 
    // {
    //     // Debug.Log("Starts");
    //     // coolDown = 0.0f;
    // }

    // Update is called once per frame
    void Update()
    {
        // Stop spawning when game ends
        // if (centipede.sections.Count == 0) { return; }
        // Debug.Log(centipede.sections.Count);
        if (Input.GetKey(KeyCode.K) && currentAgent == null)
        {
            Debug.Log("Spawning");
            // coolDown = spawnCoolDown;
            // time to spawn one
            int spawnCase = Random.Range(0, 2);
            if (spawnCase == 0) // Spawn right and move left
            {   
                currentAgent = Instantiate(agentPrefab, rightSpawn.position, Quaternion.identity);
                currentAgent.moveLeft = true;
            }
            else // Spawn left and move right
            {
                currentAgent = Instantiate(agentPrefab, leftSpawn.position, Quaternion.identity);
                currentAgent.moveLeft = false;
            }
        }
        // else
        // {
        //     coolDown -= Time.deltaTime;
        // }
    }
}
