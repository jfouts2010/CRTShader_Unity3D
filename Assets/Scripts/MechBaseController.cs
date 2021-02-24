using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBaseController : MonoBehaviour
{
    public MechInfo mechInfo;
    public int floorMask;
    public GameObject Chassis;
    public GameObject Turret;
    public WeaponInfo defaultWeapon;
    public List<WeaponPort> ports = new List<WeaponPort>();
    public float health;
    public Dictionary<KeyCode, IAbility> Abilities = new Dictionary<KeyCode, IAbility>();
    public bool CanTurn = true;
    public bool CanMove = true;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        for (int i = 0; i < mechInfo.portTransforms.Count; i++)
        {
            ports.Add(new WeaponPort());
        }
        foreach (WeaponPort port in ports)
        {
            port.weapon = new Weapon(defaultWeapon);
            GameObject go = Instantiate(port.weapon.wepInfo.GunPrefab, Turret.transform);
            port.weapon.WeaponGO = go;
            GameObject firingPointGO = go.transform.Find("FiringPoint").gameObject;
            port.weapon.FiringPoint = firingPointGO;
            port.weapon.lastWeaponFireTime = 0;
        }
        health = mechInfo.hullHealth;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (health <= 0)
            Destroy(this.gameObject);
    }
}
