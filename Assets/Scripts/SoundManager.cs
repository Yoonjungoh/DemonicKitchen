using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource effect;
    public List<AudioClip> bgmList = new List<AudioClip>();

    public AudioClip clickEffect;
    public AudioClip jumpEffect;
    public AudioClip teleportEffect;

    public AudioClip cleaverEffect;
    public AudioClip rollingPinEffect;
    public AudioClip knifetEffect;
    public AudioClip turnerEffect;
    public AudioClip panEffect;


    public AudioClip dogDieEffect;
    public AudioClip bugDieEffect;
    public AudioClip mutantDieEffect;
    public AudioClip pigDieEffect;
    public AudioClip sheepDieEffect;
    public AudioClip chickenDieEffect;
    public AudioClip luciferDieEffect;
    public AudioClip bellDieEffect;

    public AudioClip summonEffect;
    public AudioClip laserEffect;
    public AudioClip judgementEffect;



    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bgm.Stop();
        if(scene.name == "Main")
        {
            bgm.clip = bgmList[0];
            bgm.Play();
        }
        else if (scene.name == "Devildom")
        {
            bgm.clip = bgmList[1];
            bgm.Play();
        }
        else if(scene.name == "HumanWorld")
        {
            bgm.clip = bgmList[2];
            bgm.Play();
        }
        else if(scene.name == "Heaven")
        {
            bgm.clip = bgmList[3];
            bgm.Play();
        }
        else if(scene.name == "CookingScene")
        {
            bgm.clip = bgmList[4];
            bgm.Play();
        }
        else if(scene.name == "RubyShopScene")
        {
            bgm.clip = bgmList[5];
            bgm.Play();
        }
        else if (scene.name == "DevildomBoss")
        {
            bgm.clip = bgmList[6];
            bgm.Play();
        }
        else if (scene.name == "HumanWorldBoss")
        {
            bgm.clip = bgmList[7];
            bgm.Play();
        }
        else if (scene.name == "HeavenBoss")
        {
            bgm.clip = bgmList[8];
            bgm.Play();
        }
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
