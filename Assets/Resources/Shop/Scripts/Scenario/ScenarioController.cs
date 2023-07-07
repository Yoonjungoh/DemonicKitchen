using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioController : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public DialogueContainer[] dialogueContainers;
    public int[] sceneNo;

    // Start is called before the first frame update
    void Start()
    {
        SetNetSceneNumber();
        SetDialogContainer();
    }

    // Update is called once per frame
    public void SetDialogContainer()
    {
        dialogueSystem.DebugDialogueContainer = dialogueContainers[GameManager.Instance.DialogIndex];
        dialogueSystem.InitiateDialogue(dialogueSystem.DebugDialogueContainer);
    }

    void SetNetSceneNumber()
    {
        FindObjectOfType<SceneController>().sceneNo = sceneNo[GameManager.Instance.DialogIndex];
        Debug.Log(sceneNo[GameManager.Instance.DialogIndex]);
    }
}
