using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultViewer : MonoBehaviour
{
    public GameManager gameManager;
    private PlayerController _player;
    // scroll view의 content
    public GameObject _ingredientInventory;
    public TextMeshProUGUI _obtainedScoreText;
    public TextMeshProUGUI _obtainedGoldText;

    void Awake()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        _player = gameManager.GetComponent<PlayerController>();
        // 결과창에 스코어 뷰어 소수점을 정수로 처리하는 부분
        string scoreText = (gameManager.toatlScore).ToString();
        string str = string.Format("{0:0} ", double.Parse(scoreText));
        _obtainedScoreText.text = $"{str}";

        _obtainedGoldText.text = $"{gameManager.playerController.TotalGold}";
        FinanceManager.coins += gameManager.playerController.TotalGold;
        if (gameManager.playerController.HighestHeight > PlayerDataSaver.highestScore)
        {
            PlayerDataSaver.highestScore = gameManager.playerController.HighestHeight;
            PlayerDataSaver.SaveScore();
        }

        PlayerDataSaver.SaveMoney();
        for (int i = 0; i < gameManager.ingredientDataList.Count; i++)
        {
            InformationManager.addedIngredientCount.Add(gameManager.ingredientDataList[i]._ingredientID);
            Debug.Log("Gamemanager index: " + gameManager.ingredientDataList[i]._ingredientID);
            Debug.Log("Infomanager index: " + InformationManager.addedIngredientCount[i]);
        }
        RenderIngredientInventory();
    }
    void RenderIngredientInventory()
    {
        if (gameManager.ingredientDataList == null)
            return;
        for (int i = 0; i < gameManager.ingredientDataList.Count; i++)
        {
            GameObject ingredient = Util.Instantiate("IngredientBackground");
            Sprite ingredientSprite = gameManager.ingredientDataList[i]._ingredientSprite;
            ingredient.transform.GetChild(0).GetComponent<Image>().sprite = ingredientSprite;
            ingredient.transform.SetParent(_ingredientInventory.transform);
        }
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
