using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class MainUIManager : MonoBehaviour
{
    public TextMeshProUGUI Hull;
    public TextMeshProUGUI Heat;
    SimulationManager sm;
    public TextMeshProUGUI MainText;
    public TMP_InputField input;
    public GameObject EnemyText;
    public bool first = true;
    public Dictionary<SimulationEnemy, GameObject> EnemyHealth = new Dictionary<SimulationEnemy, GameObject>();
    public float Multi = 0.0204918f;
    // Start is called before the first frame update 
    void Start()
    {
        sm = GameObject.Find("SimulationManager").GetComponent<SimulationManager>();
        Hull.text = sm.pm.health.ToString();
        Heat.text = sm.pm.currentHeat.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            bool firstEnemy = true;
            int count = -1;
            foreach (SimulationEnemy se in sm.Enemies)
            {
                if (firstEnemy)
                {
                    EnemyText = GameObject.Find("InfoPanel").transform.Find("Enemy").gameObject;
                    //EnemyText.transform.Find("Value").gameObject.GetComponent<TextMeshPro>().text = "||||||||||";
                    EnemyHealth.Add(se, EnemyText);
                    firstEnemy = false;
                }
                else
                {
                    GameObject go = Instantiate(EnemyText, new Vector3(EnemyText.transform.position.x, (EnemyText.transform.position.y - 10f -  30f * count++) * Multi, 0), Quaternion.identity, GameObject.Find("InfoPanel").transform);
                    EnemyHealth.Add(se, go);
                }
            }
            first = false;
        }
        foreach (var pair in EnemyHealth)
        {
            SimulationEnemy se = pair.Key;
            GameObject go = pair.Value;
            if (se.health > 0)
                go.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = "||||||||||".Substring(0, (int)(se.health / se.maxHealth * 10f));
            else
                go.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = "";
        }
        Hull.text = Math.Round(sm.pm.health, 1).ToString();
        Heat.text = Math.Round(sm.pm.currentHeat, 1).ToString();
        input.Select();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string inputString = input.text;
            if (inputString.ToLower() == "enemy")
                StartCoroutine(EnemyLog());
            if (inputString.ToLower() == "player")
                StartCoroutine(PlayerLog());
            if (inputString.ToLower() == "turn")
                NextTurn();
            input.text = "";
            input.ActivateInputField();
            input.Select();
        }
    }
    public void NextTurn()
    {
        sm.NextTurn();
        StartCoroutine(PlayerLog());
    }
    IEnumerator EnemyLog()
    {
        if (sm.turn > 0)
        {
            MainText.maxVisibleCharacters = 0;
            MainText.text = ($"Current Turn: {sm.turn}\n" + sm.enemyLog[sm.turn]);
            while (MainText.maxVisibleCharacters < 10000)
            {
                if (MainText.text.ToList().Count > MainText.maxVisibleCharacters)
                    MainText.maxVisibleCharacters += 1;
                yield return null;
            }
        }
        yield return null;
    }
    IEnumerator PlayerLog()
    {
        if (sm.turn > 0)
        {
            MainText.maxVisibleCharacters = 0;
            MainText.text = ($"Current Turn: {sm.turn}\n" + sm.playerLog[sm.turn]);
            while (MainText.maxVisibleCharacters < 10000)
            {
                if (MainText.text.ToList().Count > MainText.maxVisibleCharacters)
                    MainText.maxVisibleCharacters += 1;
                yield return null;
            }
        }
        yield return null;
    }
    IEnumerator SlowPrintText(TextMeshProUGUI text)
    {
        text.maxVisibleCharacters = 0;
        while (MainText.maxVisibleCharacters < 10000)
        {
            if (MainText.text.ToList().Count > MainText.maxVisibleCharacters)
                MainText.maxVisibleCharacters += 1;
            yield return null;
        }
    }
}
