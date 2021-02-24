using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    SkillTreeUI ui;
    public void Start()
    {
        ui = transform.root.Find("SkillTree").GetComponent<SkillTreeUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject go = transform.root.Find("SkillTree").Find("NodeInfo").gameObject;
        go.transform.position = this.transform.position + new Vector3(55,-55,0);
        Node n = ui.nodeGOAllDict[this.gameObject];
        go.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = string.Join("\r\n",n.values.Select(p=>p.text));
        go.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {

        GameObject go = transform.root.Find("SkillTree").Find("NodeInfo").gameObject;
        go.SetActive(false);
    }
}
