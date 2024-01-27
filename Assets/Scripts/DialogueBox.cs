using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
public class DialogueBox : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private SpriteRenderer _characterPortraitSprite;

    [SerializeField]
    private TMP_Text _dialogueTextObject;

    public string textContent;

    private int _idx;

    public enum DialogueBoxState
    {
        None,
        InProgress,
        Done,
    }

    public DialogueBoxState state;

    void Start()
    {
        state = DialogueBoxState.Done;
    }

    public void DisplayText(string text, float speed)
    {
        _dialogueTextObject.text = "";
        _idx = 0;
        state = DialogueBoxState.InProgress;
        StartCoroutine(DisplayTextVisual(text, speed));
    }

    IEnumerator DisplayTextVisual(string text, float speed)
    {

        yield return new WaitForSeconds(speed);

        _dialogueTextObject.text += text[_idx];
        _idx++;

        if (_idx >= text.Length)
        {
            state = DialogueBoxState.Done;
            yield return null;
        } 
        else
        {
            StartCoroutine(DisplayTextVisual(text, speed));
        }
    }

    public void OpenDialogueBox()
    {

    }
}
