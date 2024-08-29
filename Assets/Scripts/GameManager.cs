using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    [Header("Game Attributes")]
    public int currScore;
    public Text scoreText;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        currScore = 0;
    }

    public void UpdateScore(int score) 
    {
        currScore += score;
        scoreText.text = "Score: " + currScore.ToString();
    }
}
