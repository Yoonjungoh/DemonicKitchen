using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldViewer : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI _goldText;

    void Awake()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
    }

    void Update()
    {
        _goldText.text = $"{gameManager.playerController.TotalGold}";
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
