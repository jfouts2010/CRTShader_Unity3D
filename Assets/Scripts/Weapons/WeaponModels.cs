using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EnumsForUnity;

public class EnumsForUnity : MonoBehaviour
{
    public enum WeaponType
    {
        Shotgun,
        AR,
        Rifle,
        Sniper,
        SMG,
        MG
    }
    public enum ProjectileType
    {
        BasicProjectile
    }
    public enum FireType
    {
        automatic,
        semiauto,
        charge
    }
}
[Serializable]
public class WeaponPort
{
    public int portNumber;
    public Weapon weapon;
}
[Serializable]
public class Weapon
{
    public int Level;
    public WeaponInfo wepInfo;
    public float lastWeaponFireTime;
    public GameObject WeaponGO;
    public GameObject FiringPoint;
    public float heatOutputPerShot;
    public float shootTime;
    public float velocity;
    public float bulletDamage;
    public int projectileCount;
    public float spreadDeg;
    public string Name;
    public float crit;
    public string ImageName;
    public WeaponType type;
    public List<NodeValues> itemNodes = new List<NodeValues>();
    public Weapon(WeaponInfo wepinfo)
    {
        wepInfo = wepinfo;
        heatOutputPerShot = wepInfo.heatOutputPerShotBase;
        shootTime = wepInfo.shootTimeBase;
        velocity = wepInfo.velocityBase;
        bulletDamage = wepInfo.bulletDamageBase;
        projectileCount = wepinfo.projectileCount;
        spreadDeg = wepinfo.spreadDeg;
        Name = wepinfo.Name;
        crit = wepinfo.crit;
        ImageName = wepinfo.ImageName;
        type = wepinfo.type;
    }
    public void RefreshStats()
    {
        //TODO Clean up text
        heatOutputPerShot = wepInfo.heatOutputPerShotBase * (float)(1M + itemNodes.Where(p => p.effect == NodeEffects.increased_overheat_cost).Sum(p => p.value));
        shootTime = wepInfo.shootTimeBase / (float)(1M + itemNodes.Where(p => p.effect == NodeEffects.increased_attack_speed).Sum(p => p.value));
        velocity = wepInfo.velocityBase * (float)(1M + itemNodes.Where(p => p.effect == NodeEffects.increased_velocity).Sum(p => p.value));
        bulletDamage = wepInfo.bulletDamageBase * (float)(1M + itemNodes.Where(p => p.effect == NodeEffects.increased_damage).Sum(p => p.value));
        spreadDeg = wepInfo.spreadDeg * (float)(1M + itemNodes.Where(p => p.effect == NodeEffects.increased_spread).Sum(p => p.value));
        Name = wepInfo.Name;
        crit = wepInfo.crit * (float)(1M + itemNodes.Where(p => p.effect == NodeEffects.crit_chance).Sum(p => p.value));
    }
    public static Weapon GenerateWeapon(int level)
    {
        List<System.Reflection.MethodInfo> methods = new WeaponInfo().GetType().GetMethods().Where(p => p.GetCustomAttributes(typeof(IsWeapon), false).Count() > 0).ToList();
        int count = UnityEngine.Random.Range(0, methods.Count);
        WeaponInfo wi = methods[count].Invoke(new WeaponInfo(), new object[] { }) as WeaponInfo;

        int rarityCount = UnityEngine.Random.Range(0, 20);
        int rolls = rarityCount <= 13 ? 3 : (rarityCount <= 18 ? 4 : 5);
        //0-13 = common, 14-18 = uncommon 19 = rare

        //acceptableMods
        List<NodeValues> AcceptableWeaponNodes = new List<NodeValues>()
        {
            new NodeValues(){ effect= NodeEffects.increased_damage, text = "#% increased Damage", minValue = .05M, maxValue = .30M },
            new NodeValues(){ effect= NodeEffects.increased_attack_speed, text = "#% increased Attack Speed", minValue = .05M, maxValue = .30M },
            new NodeValues(){ effect= NodeEffects.armor_pen, text = "+# Armor Pen", minValue = .50M, maxValue = 1.00M },
            new NodeValues(){ effect= NodeEffects.crit_chance, text = "#% increased Crit Chance", minValue = .05M, maxValue = .30M },
            new NodeValues(){ effect= NodeEffects.crit_damage, text = "#% increased Crit Damage", minValue = .25M, maxValue = .50M },
            new NodeValues(){ effect= NodeEffects.increased_overheat_cost, text = "#% decreased Heat Gen", minValue = -.10M, maxValue = -.20M },
            new NodeValues(){ effect= NodeEffects.increased_velocity, text = "#% increased Velocity", minValue = .10M, maxValue = .50M },
            new NodeValues(){ effect= NodeEffects.increased_spread, text = "#% decreased Weapon Spread", minValue = -.05M, maxValue = -.20M },
        };
        List<NodeEffects> ChoosenEffects = new List<NodeEffects>();
        List<NodeValues> wepNodes = new List<NodeValues>();
        //ok choose 3, 4, or 5 effects
        for (int i = 0; i < rolls; i++)
        {
            int nodeRoll = UnityEngine.Random.Range(0, AcceptableWeaponNodes.Count - i);
            NodeValues node = AcceptableWeaponNodes.Where(p => !ChoosenEffects.Contains(p.effect)).ToList()[nodeRoll];
            ///ChoosenEffects.Add(node.effect); uncomment this if you want there to be no duplicate nodes
            node.value = Math.Round((decimal)UnityEngine.Random.Range((float)node.minValue, (float)node.maxValue), 1);
            wepNodes.Add(node);
        }
        Weapon wep = new Weapon(wi);
        wep.itemNodes = wepNodes;
        wep.RefreshStats();
        return wep;
    }
}
