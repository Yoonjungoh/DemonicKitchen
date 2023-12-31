﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager
{
    public List<float> MonsterData = new List<float>();
    public List<float> TileData = new List<float>();
    public IEnumerator CoDownloadMonsterDataSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(Managers.URL.MonsterExcelURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);
        Deserialization(data, MonsterData);
    }
    public IEnumerator CoDownloadTileDataSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(Managers.URL.TileExcelURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);
        Deserialization(data, TileData);
    }
    void Deserialization(string data, List<float> datas)
    {
        string[] row = data.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            for (int j = 0; j < columnSize; j++)
            {
                //Debug.Log(column[j]);
                float value;
                bool isInt = float.TryParse(column[j], out value);
                if (isInt)
                    datas.Add(value);
                else
                    datas.Add(-100f);
            }
        }
    }
}