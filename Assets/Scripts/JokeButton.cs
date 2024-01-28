using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JokeButton : MonoBehaviour
{
    // Start is called before the first frame update

    public Joke ButtonJoke 
    { 
        get
        {
            return _buttonJoke;
        }
        set
        {
            _buttonJoke = value;
            textObject.text = ButtonJoke.JokeText;
        } 
    }

    private Joke _buttonJoke;

    public TMP_Text textObject;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => DialogueManager.instance.ChooseJoke(_buttonJoke));
    }


}
