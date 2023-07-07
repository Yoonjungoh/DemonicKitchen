using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageProgressController : MonoBehaviour
{
    [SerializeField]
    private float _updateSpeed = 0.2f;
    private Slider _progressSlider;
    private GameManager gameManager;
    private PlayerController player;
    private GameObject _bossPortal;
    public void Start()
    {
        SetStageProgress();
    }   
    public void UpdateStageProgress()
    {
        _progressSlider.value = Mathf.Lerp(_progressSlider.value, (float)player.HighestHeight / (float)_bossPortal.transform.position.y, _updateSpeed);
    }
    private void Update()
    {
        UpdateStageProgress();
    }
    public void SetStageProgress()
    {
        gameManager = GameObject.Find("@Managers").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _bossPortal = GameObject.Find("BossPortal");
        _progressSlider = GetComponent<Slider>();
        _progressSlider.value = (float)player.HighestHeight / (float)_bossPortal.transform.position.y;
    }
}