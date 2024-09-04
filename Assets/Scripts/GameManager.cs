using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [Header("Game Attributes")]
    public int baseScoreRequi = 20;
    public int currScore;


    private void Awake() {
        if (Instance == null) 
        {   
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        currScore = 0;
    }
    
    public int ReturnScore() { return currScore; }

    public int GetRequiredScore() { return baseScoreRequi; }

    public void UpdateScore(int score) 
    {
        currScore += score;
    }

    public void UpdateRequiredScore() {
        baseScoreRequi *= 2;
    }

    private void OnDestroy() 
    {
        Instance = null;
    }
}
