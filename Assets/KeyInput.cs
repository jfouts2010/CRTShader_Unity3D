using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    public TMPro.TextMeshProUGUI gui;
    TextSelectorScript tss;
    // Start is called before the first frame update
    void Start()
    {
        tss = GetComponent<TextSelectorScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            string acceptableKeys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (acceptableKeys.Contains(kcode.ToString()) && Input.GetKeyDown(kcode))
                {
                    gui.text += kcode.ToString().ToLower();
                    tss.charCount++;
                }
                if(Input.GetKeyDown(kcode) && kcode == KeyCode.Space)
                {
                    gui.text += " ";
                    tss.charCount++;
                }
                if (Input.GetKeyDown(kcode) && kcode == KeyCode.Return)
                {
                    gui.text += "\n";
                }
                if (Input.GetKeyDown(kcode) && kcode == KeyCode.Backspace)
                {
                    gui.text = gui.text.Remove(gui.text.Length - 1);
                    tss.charCount--;
                }
            }
        }
    }
}

