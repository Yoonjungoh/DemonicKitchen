using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject UI_GameController;
    public StageProgressController stageProgress;
    public PlayerController playerController;
    public float toatlScore = 0f;
    public Stat playerStat;
    public Define.SceneType sceneType;
    public Define.StageType stageType = Define.StageType.Devildom;
    public List<IngredientInfo> ingredientDataList = new List<IngredientInfo>();
    public static int killLuciferCount = 1;

    public static GameManager Instance;
    public int DialogIndex = 0;
    public static int Scenario = 0;
    public static List<Define.SpecialAbility> SpecialAbilityList = new List<Define.SpecialAbility>();

    void Awake()
    {
        if (GameObject.Find("Player") == null) 
        {
            player = Util.Instantiate("Player");
            DontDestroyOnLoad(player);
        }
        //// TODO 상점에 맞게 수정
        sceneType = Define.SceneType.Game;
        playerController = player.GetComponent<PlayerController>();
        playerStat = player.GetComponent<Stat>();
        //stageProgress = UI_GameController.transform.Find("StageProgress").GetComponent<StageProgressController>();

        // 식재료 정보 최신화
        IngredientData.SetIngredientSprite();
    }

    private void Start()
    {
        PlayerDataSaver.LoadScenario();
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Devildom")
        {
            sceneType = Define.SceneType.Game;
            stageType = Define.StageType.Devildom;
            player.GetComponent<PlayerController>()._playerScale = 1.0f;
            if (GameObject.Find("FirePillars") != null)
                GameObject.Find("FirePillars").GetComponent<FirePillarController>()._isUp = true;
        }

        else if (scene.name == "HumanWorld")
        {
            sceneType = Define.SceneType.Game;
            stageType = Define.StageType.HumanWorld;
            player.GetComponent<PlayerController>()._playerScale = 1.0f;
            if (GameObject.Find("FirePillars") != null)
                GameObject.Find("FirePillars").GetComponent<FirePillarController>()._isUp = true;
        }

        else if (scene.name == "Heaven")
        {
            sceneType = Define.SceneType.Game;
            stageType = Define.StageType.Heaven;
            player.GetComponent<PlayerController>()._playerScale = 1.0f;
            if (GameObject.Find("FirePillars") != null)
                GameObject.Find("FirePillars").GetComponent<FirePillarController>()._isUp = true;
        }
        else if (scene.name == "DevildomBoss")
        {
            sceneType = Define.SceneType.Boss;
            stageType = Define.StageType.Devildom;
            player.GetComponent<PlayerController>()._playerScale = 1.0f;
            if (GameObject.Find("FirePillars") != null)
                GameObject.Find("FirePillars").GetComponent<FirePillarController>()._isUp = false;
        }
        else if (scene.name == "HumanWorldBoss")
        {
            sceneType = Define.SceneType.Boss;
            stageType = Define.StageType.HumanWorld;
            player.GetComponent<PlayerController>()._playerScale = 1.0f;
            if (GameObject.Find("FirePillars") != null)
                GameObject.Find("FirePillars").GetComponent<FirePillarController>()._isUp = false;
        }
        else if (scene.name == "HeavenBoss")
        {
            sceneType = Define.SceneType.Boss;
            stageType = Define.StageType.Heaven;
            player.GetComponent<PlayerController>()._playerScale = 1.0f;
            if(GameObject.Find("FirePillars") != null)
                GameObject.Find("FirePillars").GetComponent<FirePillarController>()._isUp = false;
        }
        if (sceneType == Define.SceneType.Game && stageProgress != null)
        {
            stageProgress.SetStageProgress();
            stageProgress.gameObject.SetActive(true);
        }
        else if (sceneType == Define.SceneType.Boss && stageProgress != null)
            stageProgress.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public bool SetScenario(int _idx)
    {
        if (_idx > Scenario)
        {
            Scenario = DialogIndex = _idx;
            PlayerPrefs.SetInt("Scenario" + _idx.ToString(), Scenario);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}

