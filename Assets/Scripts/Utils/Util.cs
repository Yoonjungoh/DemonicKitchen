using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    // 세밀한 공격 판정을 원한다면 아래의 코루틴을 사용해서 0.3초후에 공격 모션이 플레이어에게 타격한다고 가정하면
    // 해당 시간에 플레이어와 콜라이더 충돌 일어나는지 검사 후 데미지 판정 주면 됨
    // 사용시 실제로 데미지도 입음
    public static IEnumerator CoSummonDamageViewer(GameObject enemy, int dmg, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        int damage = 0;
        // 데미지 계산 후 데미지 뷰어 소환하는 부분
        if (enemy.GetComponent<MonsterController>() != null)
        {
            damage = enemy.GetComponent<MonsterController>().OnDamaged(dmg);
            GameObject damageText = Resources.Load("Prefabs/UI/DamageText") as GameObject;
            damageText.GetComponent<DamageViewer>().damage = damage;
            damageText.GetComponent<DamageViewer>()._type = Define.CreatureType.Monster;
            Vector3 instantiatePos = new Vector3(enemy.transform.GetComponent<MonsterController>()._hpBarSlider.transform.position.x,
                enemy.transform.GetComponent<MonsterController>()._hpBarSlider.transform.position.y
                ,
                0);
            Instantiate(damageText, instantiatePos, Quaternion.identity);
        }
        if (enemy.GetComponent<PlayerController>() != null)
        {
            damage = enemy.GetComponent<PlayerController>().OnDamaged(dmg);
            GameObject damageText = Resources.Load("Prefabs/UI/DamageText") as GameObject;
            damageText.GetComponent<DamageViewer>().damage = damage;
            damageText.GetComponent<DamageViewer>()._type = Define.CreatureType.Player;
            Vector3 instantiatePos = new Vector3(enemy.transform.position.x,
                enemy.transform.position.y + (enemy.GetComponent<BoxCollider2D>().size.y),
                0);
            Instantiate(damageText, instantiatePos, Quaternion.identity);
        }
    }
    public static void SummonDamageViewer(GameObject enemy,int dmg)
    {
        int damage = 0;
        // 데미지 계산 후 데미지 뷰어 소환하는 부분
        if (enemy.GetComponent<MonsterController>() != null)
        {
            damage = enemy.GetComponent<MonsterController>().OnDamaged(dmg);
            GameObject damageText = Resources.Load("Prefabs/UI/DamageText") as GameObject;
            damageText.GetComponent<DamageViewer>().damage = damage;
            damageText.GetComponent<DamageViewer>()._type = Define.CreatureType.Monster;
            Vector3 instantiatePos = new Vector3(enemy.transform.GetComponent<MonsterController>()._hpBarSlider.transform.position.x,
                enemy.transform.GetComponent<MonsterController>()._hpBarSlider.transform.position.y,
                0);
            Instantiate(damageText, instantiatePos, Quaternion.identity);
        }
        if (enemy.GetComponent<PlayerController>() != null)
        {
            damage = enemy.GetComponent<PlayerController>().OnDamaged(dmg);
            GameObject damageText = Resources.Load("Prefabs/UI/DamageText") as GameObject;
            damageText.GetComponent<DamageViewer>().damage = damage;
            damageText.GetComponent<DamageViewer>()._type = Define.CreatureType.Player;
            Vector3 instantiatePos = new Vector3(enemy.transform.position.x,
                enemy.transform.position.y + (enemy.GetComponent<BoxCollider2D>().size.y),
                0);
            Instantiate(damageText, instantiatePos, Quaternion.identity);
        }
    }
    // load와 instantiate 한번에
    public static GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            //Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);

        return go;
    }
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

}
