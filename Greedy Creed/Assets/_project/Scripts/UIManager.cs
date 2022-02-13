using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject gameOverPanel;
    public GameObject mainMenuPanel;
    public Text coinText;

	private void Start()
	{
        Instance = this;
        Time.timeScale = 0;
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

	public void GameStart()
	{
        Time.timeScale = 1;
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void GameOver(float value)
	{
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        coinText.text ="Coins:" + value.ToString();
    }

    public void MainMenu()
	{
        SceneManager.LoadScene(0);
    }
}
