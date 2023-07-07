using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStarter : MonoBehaviour
{
    void Awake()
    {
        if (GameObject.Find("@StatManager") == null) 
        {
            GameObject statManager = new GameObject { name = "@StatManager" };

            // 스탯 저장 부분 불러오기
            statManager.AddComponent<Stat>();
            statManager.GetComponent<Stat>()._maxHp = PlayerPrefs.GetInt("PlayerMaxHP");
            statManager.GetComponent<Stat>()._hp = PlayerPrefs.GetFloat("PlayerHP");
            statManager.GetComponent<Stat>()._attack = PlayerPrefs.GetInt("PlayerAttack");
            statManager.GetComponent<Stat>()._defense = PlayerPrefs.GetInt("PlayerDefense");
            statManager.GetComponent<Stat>()._maxSpeed = PlayerPrefs.GetFloat("PlayerMaxSpeed");

            DontDestroyOnLoad(statManager);
        }
        if (GameObject.Find("@SoundManager") == null)
        {
            GameObject soundManager = Util.Instantiate("@SoundManager");
            DontDestroyOnLoad(soundManager);
        }

        // 레시피 정보 가져오는 부분
        if (PlayerDataSaver.LoadSpecialAbillity("HPPotion") == 1)
        {
            GameManager.SpecialAbilityList.Add(Define.SpecialAbility.HPPotion);
        }
        if (PlayerDataSaver.LoadSpecialAbillity("EarthQuake") == 1)
        {
            GameManager.SpecialAbilityList.Add(Define.SpecialAbility.EarthQuake);
        }
        if (PlayerDataSaver.LoadSpecialAbillity("Reflection") == 1)
        {
            GameManager.SpecialAbilityList.Add(Define.SpecialAbility.Reflection);
        }
        if (PlayerDataSaver.LoadSpecialAbillity("Resurrection") == 1)
        {
            GameManager.SpecialAbilityList.Add(Define.SpecialAbility.Resurrection);
        }
        //foreach (var ability in GameManager.SpecialAbilityList)
        //    Debug.Log($"{ability} 정보를 가져왔습니다");
    }
}
