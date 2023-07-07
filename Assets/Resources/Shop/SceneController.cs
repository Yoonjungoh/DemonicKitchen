using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG;

public class SceneController : MonoBehaviour
{
    SoundManager soundManager;
    public GameObject quitPanel;
    public GameObject settingPanel;
    public int sceneNo = -1;

    private void Start()
    {
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>(); 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "Main")
            {
                if (settingPanel.activeInHierarchy)
                {
                    soundManager.effect.PlayOneShot(soundManager.clickEffect);
                    settingPanel.SetActive(false);
                }
                else
                {
                    soundManager.effect.PlayOneShot(soundManager.clickEffect);
                    quitPanel.SetActive(true);
                }
            }
            else if(scene.name == "StartingScene")
            {
                soundManager.effect.PlayOneShot(soundManager.clickEffect);
                quitPanel.SetActive(true);
            }
            else
            {
                soundManager.effect.PlayOneShot(soundManager.clickEffect);
                SceneManager.LoadScene("Main");
            }
        }
    }
    public void LoadGameScene()
    {
        Time.timeScale = 1;
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("Devildom");
    }
    public void LoadShopScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("ShopScene");
    }
    public void LoadMainScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("Main");
    }
    public void LoadKitchenScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("CookingScene");
    }

    public void LoadRubyShopScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("RubyShopScene");
    }

    public void LoadMyInfoScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("MyInfoScene");

    }

    public void LoadScenarioScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("ScenarioScene");
    }

    public void LoadSkillScene()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        SceneManager.LoadScene("SpecialSkillScene");
    }

    public void QuitGame()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Application.Quit();
    }
    public void CloseQuitPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        quitPanel.SetActive(false);
    }

    public void OnSceneChangeButtonClick(int idx)
    {
        var sceneCont = FindObjectOfType<ScenarioController>();

        if (sceneCont && FindObjectOfType<ScenarioController>().sceneNo[GameManager.Instance.DialogIndex] == 0)
        {
            GameManager.Instance.SetScenario(++GameManager.Instance.DialogIndex);
            Functions.ChangeScene(1);
        }
        else if (sceneNo == -1)
        {
            if (sceneNo == 1)
            {
                GameManager.Instance.SetScenario(++GameManager.Instance.DialogIndex);
            }
            Functions.ChangeScene(idx);
        }
        else
        {
            Functions.ChangeScene(sceneNo);
        }
    }

    public void SetScenarioNumber(int scenarioNo)
    {
        GameManager.Instance.DialogIndex = scenarioNo;
    }

    public void kitchenSceneShift()
    {
        if (sceneNo == -1)
        {
            OnSceneChangeButtonClick(4);
        }
        else
        {
            OnSceneChangeButtonClick(sceneNo);
        }
    }
}
