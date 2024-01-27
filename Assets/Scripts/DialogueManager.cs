using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update

    public readonly string[] INTRO_DIALOGUES = new string[]
    {
        "bald people are allowed to speak only with a supervisor",
        "nothing else, thank you"
    };


    [SerializeField]
    private GameObject _dialogePanelObject;

    private DialogueBox _dialogueBox;

    private int _idx;

    public bool introSceneRunning;


    void Start()
    {
        _dialogueBox = _dialogePanelObject.GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (introSceneRunning) IntroDialogueScene();
    }

    public void IntroDialogueScene()
    {
        _dialogePanelObject.SetActive(introSceneRunning);
        if (_dialogueBox.state == DialogueBox.DialogueBoxState.Done)
        {
            if (_idx == INTRO_DIALOGUES.Length)
            {
                introSceneRunning = false;
                return;
            }
            _dialogueBox.DisplayText(INTRO_DIALOGUES[_idx], 0.05f);
            _idx++;
        }
    }
}
