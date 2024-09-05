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
    public List<Agent> currentAgentList = new();    // Temporary as one obj, could be list later
    public int agentAmount = 1;
    public bool shouldStop;

    private void Start()
    {
        coolDown = spawnCoolDown;
        shouldStop = false;
        agentAmount = GameManager.Instance.agentAmt;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop spawning when game ends
        if (centipede.sections.Count <= 0) 
        {
            shouldStop = true;
            return;
        }
        // Stop spawning when game ends
        if (currentAgentList.Count > 0) { return; }
        //Debug.Log(centipede.sections.Count);
        if (coolDown <= 0)
        {
            coolDown = spawnCoolDown;
            StartCoroutine(SpawnAgentWithDelay(2));
        }
        else
        {
             coolDown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnAgentWithDelay(float delay) {
        int spawnCase;
        for (int i = 0; i < agentAmount; i++) {
            spawnCase = Random.Range(0, 2);
            if (spawnCase == 0) // Spawn right and move left
            {   
                Vector2 posi = rightSpawn.position;
                posi.y = Random.Range(lowerBound.position.y, upperBound.position.y);
                Agent agent = Instantiate(agentPrefab, posi, Quaternion.identity);
                agent.agentController = this;
                agent.moveLeft = true;
                currentAgentList.Add(agent);
            }
            else // Spawn left and move right
            {
                Vector2 posi = leftSpawn.position;
                posi.y = Random.Range(lowerBound.position.y, upperBound.position.y);
                Agent agent = Instantiate(agentPrefab, leftSpawn.position, Quaternion.identity);
                agent.agentController = this;
                agent.moveLeft = false;
                currentAgentList.Add(agent);
            }
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
