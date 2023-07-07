using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using RPG;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text nameTag;
    [SerializeField] AudioSource audioSource;
    DialogueContainer currentDialogue;

    [SerializeField][Range(0f, 1f)] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;
    [SerializeField] float skipTextWaitTime = 0.1f;
    [SerializeField] SpriteManager spriteManager;
    [SerializeField] SpriteManager backgroundManager;

    public GameObject resumeButton;
    public AudioSource LineSoundEffect;

    string lineToShow;
    [SerializeField] int index;
    Coroutine skipTextCoroutine;

    [SerializeField] DialogueContainer debugDialogueContainer;

    public DialogueContainer DebugDialogueContainer { get => debugDialogueContainer; set => debugDialogueContainer = value; }

    [SerializeField] bool debug = false;

    private void Start()
    {
        if (debug)
        {
            if (debugDialogueContainer != null)
            {
                InitiateDialogue(debugDialogueContainer);
            }
            //CycleLine();
        }


    } //텍스트 자동진행

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PushText();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            skipTextCoroutine = StartCoroutine(SkipText());
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (skipTextCoroutine != null)
            {
                StopCoroutine(skipTextCoroutine);
            }
        }

        TypeOutText();
    }

    public void InitiateDialogue(DialogueContainer dialogueContainer)
    {
        currentDialogue = dialogueContainer;
        index = 0;
        CycleLine();
    }

    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f)
        {
            return;
        }
        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0f, 1f);
        UpdateText();
    }

    void UpdateText()
    {
        int totalLetterToShow = (int)(lineToShow.Length * visibleTextPercent);
        text.text = lineToShow.Substring(0, totalLetterToShow);
    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        CycleLine();
        LineSoundEffect.Play();
    }

    private void CycleLine()
    {
        if (index >= currentDialogue.lines.Count)
        {
            //Functions.ChangeScene("WorldMap");
            resumeButton.SetActive(true);
            Debug.Log("There is no more lines to show");
            return;
        }

        DialogueLine line = currentDialogue.lines[index];

        // 캐릭터 이미지 관리

        if (line.spriteChanges != null)
        {
            for (int i = 0; i < line.spriteChanges.Count; i++)
            {
                if (line.spriteChanges[i].actor == null)
                {
                    spriteManager.Hide(line.spriteChanges[i].onScreenImageID);
                    continue;
                }     
                int expressionID = line.spriteChanges[i].expression;
                spriteManager.Set(
                    line.spriteChanges[i].actor.sprites[expressionID],
                    line.spriteChanges[i].onScreenImageID
                    );

                spriteManager.ActiveShadow(line.spriteChanges[i].onScreenImageID);
            }
        }

        // 배경음악 재생
        if (line.audioClip != null)
        {
            audioSource.clip = line.audioClip;
            audioSource.Play();
        }

        if (line.stopAudio)
        {
            audioSource.Stop();
        }

        //if (line.stopAudio)
        //{
        //    audioSource.Stop();
        //}

        // 배경 이미지 관리
        if (line.backgroundChanges != null)
        {
            for (int i = 0; i < line.backgroundChanges.Count; i++)
            {
                if (line.backgroundChanges[i].sprite == null)
                {
                    backgroundManager.Hide(line.backgroundChanges[i].onScreenImageID);
                    continue;
                }

                var currentBgImage = backgroundManager.Get(0);
                if (currentBgImage.sprite == null)
                {
                    backgroundManager.Set(
                        line.backgroundChanges[i].sprite,
                        line.backgroundChanges[i].onScreenImageID
                        );
                }
                else if (currentBgImage.sprite != line.backgroundChanges[i].sprite)
                {
                    backgroundManager.Set(
                    line.backgroundChanges[0].sprite,
                    line.backgroundChanges[0].onScreenImageID
                    );

                    StartCoroutine(DesolveBackgroud(backgroundManager.Get(1)));
                }
            }
        }

        lineToShow = line.line;

        if (line.actor != null)
        {
            nameTag.text = line.actor.Name;
        }

        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;

        text.text = "";

        index += 1;
    }
    IEnumerator DesolveBackgroud(Image image)
    {

        while (image.color.a < 1f)
        {
            image.color = new Color(1, 1, 1, image.color.a + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        backgroundManager.Get(0).sprite = image.sprite;
        image.color = new Color(1, 1, 1, 0);
        image.gameObject.SetActive(false);

        //backgroundManager.Hide(line.backgroundChanges[i].onScreenImageID);
    }

    IEnumerator SkipText()
    {
        while (true)
        {
            yield return new WaitForSeconds(skipTextWaitTime);
            PushText();
        }
    }
}
