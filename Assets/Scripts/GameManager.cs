using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject PlayerUnit;
    PlayerManager pm;
    public GameObject TESTDELETE;
    // Start is called before the first frame update
    void Start()
    {
        PlayerUnit = GameObject.Find("Player");
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        pm.gm = this;
    }
    
    // Update is called once per frame
    void Update()
    {

    }
    public void EnemyDeath()
    {
        pm.experience += 100;
    }
}
