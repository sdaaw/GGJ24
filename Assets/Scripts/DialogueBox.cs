using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
public class DialogueBox : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject _dialogueBoxObject;

    [SerializeField]
    private SpriteRenderer _characterPortraitSprite;

    [SerializeField]
    private TMP_Text _dialogueTextObject;

    public string textContent;

    private int _idx;

    void Start()
    {
        
    }

    public void DisplayText(string text, float speed)
    {
        StartCoroutine(DisplayTextVisual(text, speed));
    }

    IEnumerator DisplayTextVisual(string text, float speed)
    {
        yield return new WaitForSeconds(speed);


        _dialogueTextObject.text += text[_idx];
        _idx++;

        if(_idx == textContent.Length) yield return null;

        StartCoroutine(DisplayTextVisual(text, speed));
    }

    public void OpenDialogueBox()
    {

    }
}
