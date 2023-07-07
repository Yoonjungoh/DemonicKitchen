using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    // ������ ���� ������ ���Ѵٸ� �Ʒ��� �ڷ�ƾ�� ����ؼ� 0.3���Ŀ� ���� ����� �÷��̾�� Ÿ���Ѵٰ� �����ϸ�
    // �ش� �ð��� �÷��̾�� �ݶ��̴� �浹 �Ͼ���� �˻� �� ������ ���� �ָ� ��
    // ���� ������ �������� ����
    public static IEnumerator CoSummonDamageViewer(GameObject enemy, int dmg, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        int damage = 0;
        // ������ ��� �� ������ ��� ��ȯ�ϴ� �κ�
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
        // ������ ��� �� ������ ��� ��ȯ�ϴ� �κ�
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
    // load�� instantiate �ѹ���
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
}
