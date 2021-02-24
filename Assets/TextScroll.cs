using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class TextScroll : MonoBehaviour
{
    TextMeshProUGUI text;
    public TMP_InputField input;
    // Start is called before the first frame update
    void Start()
    {
        /*text = GetComponent<TextMeshProUGUI>();
        text.maxVisibleCharacters = 1;*/
    }

    // Update is called once per frame
    void Update()
    {
       /* input.Select();
        if (text.text.ToList().Count > text.maxVisibleCharacters)
            text.maxVisibleCharacters += 1;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string inputString = input.text;
            input.text = "";
            input.ActivateInputField();
            input.Select();
        }*/
    }
}
