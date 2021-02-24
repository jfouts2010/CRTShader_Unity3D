using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;
using static EnumsForUnity;
using UnityEngine.EventSystems;

public class MainWeaponUI : MonoBehaviour
{
    public TextMeshProUGUI MainWeaponText;
    public TextMeshProUGUI ModWeaponText;
    public RawImage img;
    public GameObject WeaponInventorySelectionPrefab;
    public GameObject WeaponInventoryList;
    PlayerManager pm;
    Weapon selectedWep;
    WeaponType selectedType;
    float Multi = 0.0204918f;
    int firstFew = 0;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        selectedWep = pm.MainWeapon;
        WeaponInventoryList = transform.Find("WeaponInventoryList").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFew < 10)
        {
            selectedWep = pm.MainWeapon;
            firstFew++;
        }
        if (selectedWep == null)
        {
            MainWeaponText.text = "";
            img.enabled = false;
        }
        else
        {
            MainWeaponText.text = $"Damage: {Math.Round(selectedWep.bulletDamage, 1)}\n";
            MainWeaponText.text += $"Bullets: {Math.Round((double)selectedWep.projectileCount, 1)}\n";
            MainWeaponText.text += $"Fire Rate: {Math.Round(selectedWep.shootTime, 1)}\n";
            MainWeaponText.text += $"Heat/Shot: {Math.Round(selectedWep.heatOutputPerShot, 1)}\n";
            MainWeaponText.text += $"Spread: {Math.Round(selectedWep.spreadDeg, 1)}\n";
            MainWeaponText.text += $"Velocity: {Math.Round(selectedWep.velocity, 1)}\n";
            MainWeaponText.text += $"Crit: {Math.Round(selectedWep.crit, 1)}\n";
            img.texture = Resources.Load<Texture>(selectedWep.ImageName);//img.texture
            img.enabled = true;
            ModWeaponText.text = "";
            foreach (NodeValues node in selectedWep.itemNodes)
                ModWeaponText.text += node.text.Replace("#", (node.value* 100).ToString()) + "\n";
            StopCoroutine("SlowPrintText");
            StartCoroutine(SlowPrintText(MainWeaponText));
        }
    }
    public void SelectWeapon(int number)
    {
        pm.MainWeapon = pm.WeaponInventory.Where(p => p.type == selectedType).OrderBy(p => p.Name).ToList()[number];
    }
    public void WeaponSelection(string type)
    {
        selectedType = (WeaponType)Enum.Parse(typeof(WeaponType), type, true);
        selectedWep = null;
        foreach (Transform child in WeaponInventoryList.transform)
            child.gameObject.SetActive(false);
        List<Weapon> listedWeapons = pm.WeaponInventory.Where(p => p.type == selectedType).OrderBy(p => p.Name).ToList();
        int count = 0;
        foreach (Weapon wep in listedWeapons)
        {
            WeaponInventoryList.transform.GetChild(count).gameObject.SetActive(true);
            WeaponInventoryList.transform.GetChild(count).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = wep.Name;
            count++;
        }
    }
    public void WeaponHover(int number)
    {
        selectedWep = pm.WeaponInventory.Where(p => p.type == selectedType).OrderBy(p => p.Name).ToList()[number];
        StopCoroutine("SlowPrintText");
        StartCoroutine(SlowPrintText(MainWeaponText));
    }
    IEnumerator SlowPrintText(TextMeshProUGUI text)
    {

        text.maxVisibleCharacters = 0;
        while (text.maxVisibleCharacters < 10000)
        {
            if (text.text.ToList().Count > text.maxVisibleCharacters)
                text.maxVisibleCharacters += 1;
            yield return null;
        }
    }
}
