using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackGroundController : MonoBehaviour
{
    // õ�迡�� ���� y �� �Ѿ� �� ������ ���� ����
    public Texture2D[] _heavenImageArray = new Texture2D[3];
    public RawImage _heavenBackGround;

    void Start()
    {
        _heavenBackGround = transform.GetChild(2).GetComponent<RawImage>();
    }
    // ��ħ ������� ����
    public void ChangeMorningImage()
    {
        _heavenBackGround.texture = _heavenImageArray[0];
    }
    // ���� ������� ����
    public void ChangeEveningImage()
    {
        _heavenBackGround.texture = _heavenImageArray[1];
    }
    // �� ������� ����
    public void ChangeNightImage()
    {
        _heavenBackGround.texture = _heavenImageArray[2];
    }

    //IEnumerator GetTextureHeaven1()
    //{
    //    var url = $"https://evenidemonickitchen.s3.ap-northeast-2.amazonaws.com/Heaven_1.png";
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //    yield return www.SendWebRequest();
    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        _heavenImageArray[0] = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        // õ�� �ʱ� �̹��� ����
    //        _heavenBackGround.texture = _heavenImageArray[0];
    //        Debug.Log($"Sucess{_heavenImageArray[0].name}");
    //    }
    //}

    //IEnumerator GetTextureHeaven2()
    //{
    //    var url = $"https://evenidemonickitchen.s3.ap-northeast-2.amazonaws.com/Heaven_2.png";
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //    yield return www.SendWebRequest();
    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        _heavenImageArray[1] = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        Debug.Log($"Sucess{_heavenImageArray[1].name}");
    //    }
    //}

    //IEnumerator GetTextureHeaven3()
    //{
    //    var url = $"https://evenidemonickitchen.s3.ap-northeast-2.amazonaws.com/Heaven_3.png";
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
    //    yield return www.SendWebRequest();
    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        _heavenImageArray[2] = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        Debug.Log($"Sucess{_heavenImageArray[2].name}");
    //    }
    //}
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    private void OnEnable()

    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
