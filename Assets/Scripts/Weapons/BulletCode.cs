using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCode : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public bool playerBullet;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = rb.velocity;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {
            other.GetComponent<BuildingController>().health -= 1;
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy" && playerBullet)
        {
            other.transform.root.GetComponent<EnemyController>().health -= 1;
            Destroy(this.gameObject);
        }
        else if(other.tag == "Player" && !playerBullet)
        {
            other.transform.root.GetComponent<PlayerMechController>().health -= 1;
            Destroy(this.gameObject);
        }
        else if(other.tag == "Floor")
            Destroy(this.gameObject);
    }
}
