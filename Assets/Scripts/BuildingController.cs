using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public int health;
    public bool destroyed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!destroyed && health < 0)
        {
            Destroy(this.gameObject);
            destroyed = true;
        }
    }
}
