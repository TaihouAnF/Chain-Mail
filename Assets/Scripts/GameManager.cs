using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [Header("Game Attributes")]
    public int currScore;
    public TextMeshProUGUI scoreText;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        currScore = 0;
    }
    
    public int ReturnScore() { return currScore; }

    public void UpdateScore(int score) 
    {
        currScore += score;
        scoreText.text = "Score: " + currScore.ToString();
    }
}
