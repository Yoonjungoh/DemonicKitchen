using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelViewer : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI _scoreText;

    void Awake()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
    }

    void Update()
    {
        _scoreText.text = $"Level : {gameManager.playerController.Level} ({gameManager.playerController.TotalExp} / {LevelData.data[gameManager.playerController.Level - 1]})";
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
