using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ArmorTypes
{
    Chasis,
    Turret,
    Cockpit,
    LArm,
    RArm
}
public class Armor
{
    public int Level = 0;
    public float armor = 0;
    public float evasion = 0;
    public float hull = 0;
    public List<NodeValues> mods = new List<NodeValues>();
    ArmorTypes type;

    public Armor GenerateArmor()
    {
        int rarityCount = UnityEngine.Random.Range(0, 20);
        int rolls = rarityCount <= 13 ? 3 : (rarityCount <= 18 ? 4 : 5);
        List<NodeValues> AcceptableArmorNodes = new List<NodeValues>()
        {
            new NodeValues(){ effect= NodeEffects.increased_cqb_damage_taken, text = "#% reduced CQB Damage Taken", minValue = -.05M, maxValue = -.2M },
            new NodeValues(){ effect= NodeEffects.increased_long_range_damage_taken, text = "#% reduced Long Range Damage Taken", minValue = -.05M, maxValue = -.2M },
            new NodeValues(){ effect= NodeEffects.increased_void_damage_taken, text = "#% reduced Void Damage Taken", minValue = -.05M, maxValue = -.2M },
            new NodeValues(){ effect= NodeEffects.increased_hull, text = "#% increased Hull", minValue = .05M, maxValue = .30M },
            new NodeValues(){ effect= NodeEffects.increased_armor, text = "#% increased Armor", minValue = .1M, maxValue = .3M },
            new NodeValues(){ effect= NodeEffects.increased_evasion, text = "#% increased Evasion", minValue = .1M, maxValue = .3M },
            new NodeValues(){ effect= NodeEffects.increased_armor_and_evasion, text = "#% increased Evasion and Armor", minValue = .1M, maxValue = .3M },

            new NodeValues(){ effect= NodeEffects.crit_chance, text = "#% increased Crit Chance", minValue = .05M, maxValue = .30M },
            new NodeValues(){ effect= NodeEffects.increased_overheat_cost, text = "#% decreased Heat Gen", minValue = -.05M, maxValue = -.07M },
            new NodeValues(){ effect= NodeEffects.increased_velocity, text = "#% increased Velocity", minValue = .10M, maxValue = .15M },
            new NodeValues(){ effect= NodeEffects.increased_spread, text = "#% decreased Weapon Spread", minValue = -.05M, maxValue = -.07M },
            new NodeValues(){ effect= NodeEffects.increased_damage, text = "#% increased Damage", minValue = .1M, maxValue = .3M },
            new NodeValues(){ effect= NodeEffects.increased_attack_speed, text = "#% increased Attack Speed", minValue = .03M, maxValue = .06M },
        };
        List<NodeEffects> ChoosenEffects = new List<NodeEffects>();
        List<NodeValues> armorMods = new List<NodeValues>();
        //ok choose 3, 4, or 5 effects
        for (int i = 0; i < rolls; i++)
        {
            int nodeRoll = UnityEngine.Random.Range(0, AcceptableArmorNodes.Count - i);
            NodeValues node = AcceptableArmorNodes.Where(p => !ChoosenEffects.Contains(p.effect)).ToList()[nodeRoll];
            ///ChoosenEffects.Add(node.effect); uncomment this if you want there to be no duplicate nodes
            node.value = (decimal)UnityEngine.Random.Range((float)node.minValue, (float)node.maxValue);
            armorMods.Add(node);
        }

        Array armorTypes = Enum.GetValues(typeof(ArmorTypes));
        ArmorTypes type = (ArmorTypes)armorTypes.GetValue(UnityEngine.Random.Range(0, armorTypes.Length - 1));

        Armor newArm = new Armor();
        newArm.mods = armorMods;
        newArm.type = type;

        int armorBase = UnityEngine.Random.Range(0, 3);
        if(armorBase == 0)
        {
            newArm.armor = UnityEngine.Random.Range(0, 10);
        }
        if (armorBase == 1)
        {
            newArm.evasion = UnityEngine.Random.Range(0, 10);
        }
        if (armorBase == 2)
        {
            newArm.armor = UnityEngine.Random.Range(0, 5);
            newArm.evasion = UnityEngine.Random.Range(0, 5);
        }
        newArm.hull = UnityEngine.Random.Range(0, 100);
        return newArm;
    }
}


