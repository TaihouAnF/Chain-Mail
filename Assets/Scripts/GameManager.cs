using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [Header("Game Attributes")]
    private const int maxLevel = 5;
    public int currLevel = 0;
    public const int baseScore = 20;
    public int scoreRequire = baseScore;
    public int currScore;
    public const int basePedeLength = 25;
    public int currPedeLength = basePedeLength;
    public const float baseEnemySpeed = 5.0f;
    public float currEnemySpeed = baseEnemySpeed;
    public const float baseEnemyBootTime = 3.0f;
    public float currEnemyBootTime = baseEnemyBootTime;
    public const float baseEnemyCdTime = 2.0f;
    public float currEnemyCdTime = baseEnemyCdTime;


    private void Awake() {
        if (Instance == null) 
        {   
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        currScore = 0;
    }
    
    public int ReturnScore() { return currScore; }

    public int GetRequiredScore() { return scoreRequire; }

    public void UpdateScore(int score) 
    {
        currScore += score;
    }

    public void ChangeLevel() 
    {
        currPedeLength += 10;
        scoreRequire += currPedeLength - 5;
        currLevel = (currLevel + 1) % maxLevel;
        switch (currLevel) 
        {
            case 1:
                currEnemySpeed += 0.5f;
                break;
            case 2:
                currEnemyBootTime -= 1f;
                break;
            case 3:
                currEnemyCdTime -= 0.5f;
                break;
            case 4:
                scoreRequire += 10;
                // Could make two agents.
                break;
            default:    // reset them back
                scoreRequire = baseScore;
                currScore = 0;
                currPedeLength = basePedeLength;
                currEnemySpeed = baseEnemySpeed;
                currEnemyBootTime = baseEnemyBootTime;
                currEnemyCdTime = baseEnemyCdTime;
                break;
        }
    }
}
