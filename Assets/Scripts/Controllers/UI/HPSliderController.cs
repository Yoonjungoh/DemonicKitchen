using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 몬스터 전용 HPBar
public class HPSliderController : MonoBehaviour
{
    [SerializeField]
    private float _lossSpeed = 0.2f;
    private Slider _hpBarSlider;
    private Stat _stat;
    private void Start()
    {
        _stat = GetComponentInParent<Stat>();
        _hpBarSlider = GetComponent<Slider>();
        _hpBarSlider.value = (float)_stat.HP / (float)_stat.MaxHP;
    }
    public void UpdateHP()
    {
        //_hpBarSlider.value = (float)_stat.HP / (float)_stat.MaxHP;
        _hpBarSlider.value = Mathf.Lerp(_hpBarSlider.value, (float)_stat.HP / (float)_stat.MaxHP, _lossSpeed);
    }
    private void Update()
    {
        UpdateHP();
    }
}
