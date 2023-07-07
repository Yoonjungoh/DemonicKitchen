using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponController : MonoBehaviour
{
    PlayerController _player;
    public UI_WeaponController _uI_WeaponController;
    public GameObject showWeaponUIButton;
    SoundManager soundManager;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
    }
    public void ShowWeaponUI()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        _uI_WeaponController.gameObject.SetActive(true);
        showWeaponUIButton.SetActive(false);
    }
    public void CloseWeaponUI()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        gameObject.SetActive(false);
        showWeaponUIButton.SetActive(true);
    }
    //public void ChangeCleaver()
    //{
    //    _player._weaponIDList.Clear();
    //    _player._weaponIDList.Add(201);
    //    _player.SetWeapon();
    //}
    //public void ChangeRollingPin()
    //{
    //    _player._weaponIDList.Clear();
    //    _player._weaponIDList.Add(202);
    //    _player.SetWeapon();
    //}
    //public void ChangeKnife()
    //{
    //    _player._weaponIDList.Clear();
    //    _player._weaponIDList.Add(203);
    //    _player.SetWeapon();
    //}
    //public void ChangeTurner()
    //{
    //    _player._weaponIDList.Clear();
    //    _player._weaponIDList.Add(204);
    //    _player.SetWeapon();
    //}
    //public void ChangePan()
    //{
    //    _player._weaponIDList.Clear();
    //    _player._weaponIDList.Add(205);
    //    _player.SetWeapon();
    //}
}
