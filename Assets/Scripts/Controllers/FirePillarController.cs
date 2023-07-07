using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillarController : MonoBehaviour
{
    [SerializeField]
    float _speed = 2f;
    public bool _isUp = true;
    void Update()
    {
        if (_isUp)
            transform.position += new Vector3(0f, _speed * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().OnDamaged(int.MaxValue);
            Debug.Log("Die cause fire");
        }
        else if (collision.tag == "Wall")
        {
            collision.gameObject.SetActive(true);
        }
        else
            collision.gameObject.SetActive(false);
    }
}
