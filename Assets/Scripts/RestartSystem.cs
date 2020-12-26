using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartSystem : MonoBehaviour
{
    // score text
    public Text scoreText;
    int score;


    // Start is called before the first frame update
    void Start()
    {
        score = GameManager.getScore();
        scoreText.text = score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
    public void Home()
    {
        SceneManager.LoadScene("Scenes/Title");
    }
}
