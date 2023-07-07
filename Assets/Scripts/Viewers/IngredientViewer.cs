using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngredientViewer : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI _ingredientText;

    void Awake()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
    }

    void Update()
    {
        _ingredientText.text = $"{gameManager.playerController.TotalIngredient}";
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
