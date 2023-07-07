using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameController : MonoBehaviour
{
    public GameObject _scorePanel;
    public GameObject _buttonPanel;
    public GameObject _deathPanel;
    public GameObject _clearPanel;
    public GameObject _checkPanel;
    public SoundManager soundManager;
    public GameObject checkMark;
    public bool _wantShaking = true;

    public bool _isRestart = false;
    private void Start()
    {
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }
    public void InactiveDeathPanel()
    {

        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Destroy(GameObject.Find("@Managers"));
        Destroy(GameObject.Find("BackGroundCanvas"));
        Destroy(GameObject.Find("Player"));

        if (_isRestart)
        {
            Time.timeScale = 1;
            _deathPanel.SetActive(false);
            SceneManager.LoadScene("ShopScene");
            Destroy(gameObject);
        }
        else
        {
            Time.timeScale = 1;
            _deathPanel.SetActive(false);
            SceneManager.LoadScene("Main");
            Destroy(gameObject);
        }
    }
    public void ActiveDeathPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Time.timeScale = 0;
        _clearPanel.SetActive(false);
        _deathPanel.SetActive(true);
    }
    public void ActiveCheckPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Time.timeScale = 0;
        _checkPanel.SetActive(true);
    }
    public void YesCheckPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        _deathPanel.SetActive(true);
        _checkPanel.SetActive(false);
    }
    public void NoCheckPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Time.timeScale = 1;
        _checkPanel.SetActive(false);
    }
    public void RestartCheckPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Time.timeScale = 1;
        _isRestart = true;
        _checkPanel.SetActive(false);
        _deathPanel.SetActive(true);
    }
    public void RestartGame()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        Destroy(GameObject.Find("@Managers"));
        Destroy(GameObject.Find("BackGroundCanvas"));
        Destroy(GameObject.Find("Player"));
        Time.timeScale = 1;
        _deathPanel.SetActive(false);
        SceneManager.LoadScene("ShopScene");
        Destroy(gameObject);
    }
    public void WantShaking()
    {
        if (checkMark.activeInHierarchy)
        {
            checkMark.SetActive(false);
            _wantShaking = false;
        }
        else
        {
            checkMark.SetActive(true);
            _wantShaking = true;
        }
    }
}
