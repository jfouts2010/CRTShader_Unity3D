using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TextSelectorScript : MonoBehaviour
{
    public GameObject textGO;
    MeshRenderer mr;
    float f = 0;
    Vector3 lastPost = new Vector3();
    public int charCount = 1;
    public Vector3 defaultPosMod = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        TMPro.TextMeshProUGUI tmpinfo = textGO.GetComponent<TMPro.TextMeshProUGUI>();
        var lastCharInfo = tmpinfo.textInfo.characterInfo[charCount];
        Vector3 lastVertexPosition = lastCharInfo.bottomRight;

      

        Vector3 position = tmpinfo.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(lastCharInfo.bottomRight.x, lastCharInfo.baseLine, lastCharInfo.bottomRight.z));
        transform.position = position + new Vector3(.13f,.14f, 0);// + new Vector3(tmpinfo.characterWidthAdjustment, 0, 0);
        if(lastCharInfo.character == ' ')
            transform.position += new Vector3(.13f,0, 0);
        if (charCount == 1)
            transform.position = tmpinfo.gameObject.transform.position + defaultPosMod;
        //ebug.Log(tmpinfo.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(lastChar.baseLine,0,0)));
        bool moved = transform.position != lastPost;
        lastPost = transform.position;
        f += Time.deltaTime;
        if (f > 0.5)
        {
            f = 0;
            if (!moved)
                mr.enabled = !mr.enabled;
            else
                mr.enabled = true;
        }
    }
}
