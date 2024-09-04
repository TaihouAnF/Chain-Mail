using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public TextMeshProUGUI FinalScoreText;
    public GameManager GameManager;
    public GameObject LosePanel;
    public GameObject NextLevelPanel;
    public Slider LoadingSlider;
    public TextMeshProUGUI PedeLenText;
    public TextMeshProUGUI scoreText;

    public bool startScreen = false;
    private bool loadingNextLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        //LosePanel.SetActive(false);
        //NextLevelPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        LosePanel.SetActive(true);
        FinalScoreText.text = GameManager.Instance.ReturnScore().ToString();
    }
    public IEnumerator NextLevel()
    {
        NextLevelPanel.SetActive(true);
        LosePanel.SetActive(false);
        loadingNextLevel = true;
        yield return new WaitForSecondsRealtime(5);
        GameManager.Instance.UpdateRequiredScore();
        NextLevelPanel.SetActive(false);
        SceneManager.LoadScene(1);
    }

    private void FixedUpdate()
    {
        if(GameObject.FindObjectOfType<Centipede>() != null)
        {
            PedeLenText.text = "Length: " + GameObject.FindObjectOfType<Centipede>().PedeLength.ToString();
            scoreText.text = "Score: " + GameManager.Instance.ReturnScore().ToString();
        }
    }
    private void Update()
    {
        if (Input.anyKeyDown && startScreen)
        {
            LosePanel.SetActive(false);
            NextLevelPanel.SetActive(true);
        }
        if (loadingNextLevel)
            LoadingSlider.value = LoadingSlider.value + (Time.deltaTime / 3);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
