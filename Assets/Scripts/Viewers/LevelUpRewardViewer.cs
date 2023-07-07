using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpRewardViewer : MonoBehaviour
{
    // ���� ���� ī�� ��
    public static int showingRewardCount = 0;
    public GameManager gameManager;
    public GameObject weaponPanel;

    public GameObject _weapon_201;
    public List<GameObject> _weapon_202;
    public GameObject _weapon_203;
    public GameObject _weapon_204;
    public GameObject _weapon_205;

    public GameObject _reward_1;
    public GameObject _reward_2;
    public GameObject _reward_3;

    public Weapon_201_Controller weapon_201_Controller;
    public RollingPinController rollingPinController;
    public KnifeController knifeController;
    public TurnerController turnerController;
    public PanController panController;

    public List<WeaponInfo> _rewardWeaponList = new List<WeaponInfo>();
    public List<Sprite> _rewardSpriteList = new List<Sprite>();
    List<int> _randomIndexList = new List<int>();

    public List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();
    public List<Button> _buttonList = new List<Button>();
    public SoundManager soundManager;

    bool _isRewardCleaver = false;
    bool _isRewardRollingPin = false;
    bool _isRewardKnife = false;
    bool _isRewardTurner = false;
    bool _isRewardPan = false;

    public GameObject weaponExplainPanel;

    void Start()
    {
        soundManager = GameObject.Find("@SoundManager").GetComponent<SoundManager>();
        GameObject UI_GameController = GameObject.FindWithTag("GameCanvas");

        for (int i = 0; i < UI_GameController.transform.childCount; i++)
        {
            if (UI_GameController.transform.GetChild(i).name == "CheckPanel")
            {
                weaponPanel = UI_GameController.transform.GetChild(i).GetChild(1).GetChild(1).gameObject;
                break;
            }
        }

        GameObject player = GameObject.FindWithTag("Player");

        GameObject weapons = new GameObject();

        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (player.transform.GetChild(i).gameObject.name == "Weapons")
            {
                weapons = player.transform.GetChild(i).gameObject;
                break;
            }
        }

        _weapon_201 = weapons.transform.GetChild(0).gameObject;
        _weapon_202.Clear();
        _weapon_202.Add(weapons.transform.GetChild(1).GetChild(0).gameObject);
        _weapon_202.Add(weapons.transform.GetChild(1).GetChild(1).gameObject);
        _weapon_202.Add(weapons.transform.GetChild(1).GetChild(2).gameObject);
        _weapon_203 = weapons.transform.GetChild(2).GetChild(0).gameObject;
        _weapon_204 = weapons.transform.GetChild(3).GetChild(0).gameObject;
        _weapon_205 = weapons.transform.GetChild(4).GetChild(0).gameObject;

        _rewardWeaponList.Add(new WeaponInfo(_weapon_201, _rewardSpriteList[0]));
        _rewardWeaponList.Add(new WeaponInfo(_weapon_202[0], _rewardSpriteList[1]));
        _rewardWeaponList.Add(new WeaponInfo(_weapon_203, _rewardSpriteList[2]));
        _rewardWeaponList.Add(new WeaponInfo(_weapon_204, _rewardSpriteList[3]));
        _rewardWeaponList.Add(new WeaponInfo(_weapon_205, _rewardSpriteList[4]));

        _textList.Add(_reward_1.GetComponentInChildren<TextMeshProUGUI>());
        _textList.Add(_reward_2.GetComponentInChildren<TextMeshProUGUI>());
        _textList.Add(_reward_3.GetComponentInChildren<TextMeshProUGUI>());

        _buttonList.Add(_reward_1.GetComponent<Button>());
        _buttonList.Add(_reward_2.GetComponent<Button>());
        _buttonList.Add(_reward_3.GetComponent<Button>());

        MakeRandomIndex();
    }
    //void Update()
    //{
    //    UpdateWeaponLevelText();
    //}
    // ���� ���� �������� �ߺ� ���� �̱�
    // ���߿� max level ����� max level�� ����� ���� ����Ʈ���� ���� ������ֱ� �׷��� ���ҵ�
    public void MakeRandomIndex()
    {
        int random = Random.Range(0, _rewardSpriteList.Count);

        for (int i = 0; i < 3; i++) 
        {
            while (_randomIndexList.Contains(random))
            {
                random = Random.Range(0, _rewardSpriteList.Count);
            }
            _randomIndexList.Add(random);
        }

        MakeRewardImage();
    }
    // ���� ���� ���� �̹��� �����
    void MakeRewardImage()
    {
        _reward_1.GetComponent<Image>().sprite = _rewardSpriteList[_randomIndexList[0]];
        _reward_2.GetComponent<Image>().sprite = _rewardSpriteList[_randomIndexList[1]];
        _reward_3.GetComponent<Image>().sprite = _rewardSpriteList[_randomIndexList[2]];

        _reward_1.GetComponent<Image>().sprite = _rewardSpriteList[_randomIndexList[0]];
        _reward_2.GetComponent<Image>().sprite = _rewardSpriteList[_randomIndexList[1]];
        _reward_3.GetComponent<Image>().sprite = _rewardSpriteList[_randomIndexList[2]];

        CheckWeapon();
    }
    // �������� ���� ���Ⱑ � controller�� ������ �ֳ� Ȯ���ϰ� �ؽ�Ʈ ����� ��ư �����ϰ� ���� ���� �������� �۾�
    void CheckWeapon()
    {
        for(int index = 0; index < 3; index++)
        {
            if (_rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<Weapon_201_Controller>() != null)
            {
                weapon_201_Controller = _rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<Weapon_201_Controller>();

                _textList[index].text = "Ŭ����";

                //if (weapon_201_Controller._level == 0)
                //{
                //    _textList[index].text +=
                //    $"     Lv.{weapon_201_Controller._level + 1} ";
                //}
                //else
                //{
                //    _textList[index].text +=
                //    $"Lv.{weapon_201_Controller._level}\n" +
                //    $"-> Lv.{weapon_201_Controller._level + 1}";
                //}

                _isRewardCleaver = true;

                _buttonList[index].onClick.AddListener(() => LevelUpCleaver());
            }
            else if (_rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<RollingPinController>() != null)
            {
                rollingPinController = _rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<RollingPinController>();
                _textList[index].text = "�Ѹ���";

                //if (rollingPinController._level == 0)
                //{
                //    _textList[index].text +=
                //    $"    Lv.{rollingPinController._level + 1} ";
                //}
                //else
                //{
                //    _textList[index].text +=
                //        $"Lv.{rollingPinController._level}\n" +
                //        $"-> Lv.{rollingPinController._level + 1}";
                //}

                _isRewardRollingPin = true;

                _buttonList[index].onClick.AddListener(() => LevelUpRollingPin());
            }
            else if (_rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<KnifeController>() != null)
            {
                knifeController = _rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<KnifeController>();

                _textList[index].text = "������";

                //if (knifeController._level == 0)
                //{
                //    _textList[index].text +=
                //    $"    Lv.{knifeController._level + 1} ";
                //}
                //else
                //{
                //    _textList[index].text +=
                //        $"Lv.{knifeController._level}\n" +
                //        $"-> Lv.{knifeController._level + 1}";
                //}

                _isRewardKnife = true;

                _buttonList[index].onClick.AddListener(() => LevelUpKnife());
            }
            else if (_rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<TurnerController>() != null)
            {
                turnerController = _rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<TurnerController>();

                _textList[index].text = "�ͳ�";

                //if (turnerController._level == 0)
                //{
                //    _textList[index].text +=
                //    $"    Lv.{turnerController._level + 1} ";
                //}
                //else
                //{
                //    _textList[index].text +=
                //        $"Lv.{turnerController._level}\n" +
                //        $"-> Lv.{turnerController._level + 1}";
                //}

                _isRewardTurner = true;

                _buttonList[index].onClick.AddListener(() => LevelUpTurner());
            }
            else if (_rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<PanController>() != null)
            {
                panController = _rewardWeaponList[_randomIndexList[index]]._weapon.GetComponent<PanController>();
                _textList[index].text = "��������";

                //if (panController._level == 0)
                //{
                //    _textList[index].text +=
                //    $"    Lv.{panController._level + 1} ";
                //}
                //else
                //{
                //    _textList[index].text +=
                //        $"Lv.{panController._level}\n" +
                //        $"-> Lv.{panController._level + 1}";
                //}

                _isRewardPan = true;

                _buttonList[index].onClick.AddListener(() => LevelUpPan());
            }
        }
        // ���� �Ͻ� ����
        Time.timeScale = 0;
        showingRewardCount++;
    }
    public void UpdateWeaponLevelText()
    {
        for(int index = 0; index < 3; index++)
        {
            if (_isRewardCleaver)
            {
                if (weapon_201_Controller._level == 0)
                {
                    _textList[index].text =
                    $"Ŭ����\n     Lv.{weapon_201_Controller._level + 1} ";
                }
                else
                {
                    _textList[index].text =
                    $"Ŭ����\nLv.{weapon_201_Controller._level}\n" +
                    $"-> Lv.{weapon_201_Controller._level + 1}";
                }

            }
            if (_isRewardRollingPin)
            {
                if (rollingPinController._level == 0)
                {
                    _textList[index].text =
                    $"�Ѹ���\n    Lv.{rollingPinController._level + 1} ";
                }
                else
                {
                    _textList[index].text =
                        $"�Ѹ���\nLv.{rollingPinController._level}\n" +
                        $"-> Lv.{rollingPinController._level + 1}";
                }
            }
            if (_isRewardKnife)
            {
                if (knifeController._level == 0)
                {
                    _textList[index].text =
                    $"������\n    Lv.{knifeController._level + 1} ";
                }
                else
                {
                    _textList[index].text =
                        $"������\nLv.{knifeController._level}\n" +
                        $"-> Lv.{knifeController._level + 1}";
                }
            }
            if (_isRewardTurner)
            {
                if (turnerController._level == 0)
                {
                    _textList[index].text =
                    $"�ͳ�\n    Lv.{turnerController._level + 1} ";
                }
                else
                {
                    _textList[index].text =
                        $"�ͳ�\nLv.{turnerController._level}\n" +
                        $"-> Lv.{turnerController._level + 1}";
                }
            }
            if (_isRewardPan)
            {
                if (panController._level == 0)
                {
                    _textList[index].text =
                    $"��\n    Lv.{panController._level + 1} ";
                }
                else
                {
                    _textList[index].text =
                        $"��\nLv.{panController._level}\n" +
                        $"-> Lv.{panController._level + 1}";
                }
            }
        }
    }
    public void LevelUpCleaver()
    {
        weapon_201_Controller._level++;
        // weapon panel �ֽ�ȭ 
        weaponPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Lv.{weapon_201_Controller._level}";
        showingRewardCount--;
        if (showingRewardCount == 0) 
        {
            // ���� ���
            Time.timeScale = 1;
        }
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        gameObject.SetActive(false);
    }
    public void LevelUpRollingPin()
    {
        rollingPinController.transform.parent.gameObject.SetActive(true);
        rollingPinController._level++;
        _weapon_202[1].GetComponent<RollingPinController>()._level++;
        _weapon_202[2].GetComponent<RollingPinController>()._level++;
        // weapon panel �ֽ�ȭ 
        weaponPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Lv.{rollingPinController._level}";
        showingRewardCount--;
        if (showingRewardCount == 0)
        {
            // ���� ���
            Time.timeScale = 1;
        }
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        gameObject.SetActive(false);
    }
    public void LevelUpKnife()
    {
        knifeController.transform.parent.gameObject.SetActive(true);
        knifeController._level++;
        // weapon panel �ֽ�ȭ 
        weaponPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Lv.{knifeController._level}";
        showingRewardCount--;
        if (showingRewardCount == 0)
        {
            // ���� ���
            Time.timeScale = 1;
        }
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        gameObject.SetActive(false);
    }
    public void LevelUpTurner()
    {
        turnerController.transform.parent.gameObject.SetActive(true);
        turnerController._level++;
        // weapon panel �ֽ�ȭ 
        weaponPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Lv.{turnerController._level}";
        showingRewardCount--;
        if (showingRewardCount == 0)
        {
            // ���� ���
            Time.timeScale = 1;
        }
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        gameObject.SetActive(false);
    }
    public void LevelUpPan()
    {
        panController.transform.parent.gameObject.SetActive(true);
        panController._level++;
        // weapon panel �ֽ�ȭ 
        weaponPanel.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Lv.{panController._level}";
        showingRewardCount--;
        if (showingRewardCount == 0)
        {
            // ���� ���
            Time.timeScale = 1;
        }
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        gameObject.SetActive(false);
    }
    public void ShowWeaponExplainPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        weaponExplainPanel.SetActive(true);
    }
    public void CloseWeaponExplainPanel()
    {
        soundManager.effect.PlayOneShot(soundManager.clickEffect);
        weaponExplainPanel.SetActive(false);
    }
}
public class WeaponInfo
{
    public GameObject _weapon;
    public Sprite _sprite;
    public WeaponInfo(GameObject weapon, Sprite sprite)
    {
        _weapon = weapon;
        _sprite = sprite;
    }
}
