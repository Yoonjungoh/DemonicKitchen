using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다
    ResourceManager _resource = new ResourceManager();
    DataManager _data = new DataManager();
    UIManager _ui = new UIManager();
    URLManager _url = new URLManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static DataManager Data { get { return Instance._data; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static URLManager URL { get { return Instance._url; } }

	void Start()
    {
        Init();
        // 주석 해제하면 서버에서 Data 정보 가져옴
        StartCoroutine(CoDataManagerInit());
    }

    static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }		
	}

    public static void Clear()
    {
        //Scene.Clear();
        UI.Clear();
    }
    public IEnumerator CoDataManagerInit()
    {
        // 추가될 json 데이터들 가져오는 코루틴 넣어주기
        StartCoroutine(Managers.Data.CoDownloadMonsterDataSheet());
        StartCoroutine(Managers.Data.CoDownloadTileDataSheet());

        yield return null;
    }
}
