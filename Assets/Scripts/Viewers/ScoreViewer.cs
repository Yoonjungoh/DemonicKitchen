using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreViewer : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI _scoreText;

    void Awake()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
    }

    void Update()
    {
        
        string scoreText = (gameManager.toatlScore + gameManager.playerController.HighestHeight).ToString();
        string str = string.Format("{0:0} ", double.Parse(scoreText));
        _scoreText.text = $"Score : {str}";
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameManager.sceneType == Define.SceneType.Game || gameManager.sceneType == Define.SceneType.Boss) 
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
