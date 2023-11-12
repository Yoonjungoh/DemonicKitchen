using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLManager
{
    // 파싱할 JSON 파일의 서버 주소
    public string JsonPath = "https://evenidemonickitchen.s3.ap-northeast-2.amazonaws.com/CouponData.json";
    // 파싱할 Google Excel의 서버 주소
    public string MonsterExcelURL = "https://docs.google.com/spreadsheets/d/1uek10ZYP02SrXp0yESA2Ou8PxkQMe0TPlFufEh9fzgo/export?format=tsv";
    public string TileExcelURL = "https://docs.google.com/spreadsheets/d/1f1CuHW1khnSGupcvn6W8HvHLlWQt3dvASy2lzMnpzYs/export?format=tsv";
}
