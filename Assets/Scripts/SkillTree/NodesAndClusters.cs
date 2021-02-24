using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public enum NodeEffects
{
    //BASE
    gw,

    //SPEED
    increased_attack_speed,
    damage_to_attack,
    //damage_to_attacks_after_hits,
    //gain_health_after_x_hits,
    //gain_shield_after_x_hits,
    //gain_barrier_after_x_hits,
   //vent_heat_after_hitting_x_times,

    //DAMAGE
    increased_damage,
    //more_damage_less_as,
    //gain_perc_of_dmg_hit_health,
    //gain_perc_of_dmg_hit_shield,
    //vent_perc_of_dmg_hit,
   //gain_perc_of_dmg_hit_barrier,
    //killed_enemies_explode_overkill_dmg,

    //CRIT
    crit_chance,
    crit_damage,

    //OVERHEAT
    increased_overheat_capacity,
    increased_overheat_cost,
    increased_vent_speed,

    //HULL
    increased_hull,

    //SPEED
    increased_ms,


    //Item notables: Weapon
    armor_pen,
    increased_velocity,
    increased_spread,

    //Item notables: Armor
    increased_cqb_damage_taken,
    increased_long_range_damage_taken,
    increased_void_damage_taken,
    increased_armor,
    increased_evasion,
    increased_armor_and_evasion
}
public enum NodeEffectTypes
{
    attack_speed,
    health,
    damage,
    crit,
    overheat,
    movement_speed,
}
public class ClusterAttribute : System.Attribute
{
    public NodeEffectTypes[] types;
    public int weight;
    public ClusterAttribute(NodeEffectTypes[] _types, int _weight)
    {
        types = _types;
        weight = _weight;
    }
}
public class Notable : System.Attribute
{
    public NodeEffectTypes[] types;
    public int weight;
    public Notable(NodeEffectTypes[] _types, int _weight)
    {
        types = _types;
        weight = _weight;
    }
}
public class ClusterGenerator
{
    public static void GenerateCluster(int i, List<Node> nodes)
    {
        Array values = Enum.GetValues(typeof(NodeEffectTypes));
        Random random = new Random();
        NodeEffectTypes effectType = (NodeEffectTypes)values.GetValue(random.Next(values.Length));
        GetCluster(new NodeEffectTypes[] { effectType }).Invoke(new ClusterGenerator(), new object[] { nodes });
    }
    public static MethodInfo GetNotable(NodeEffectTypes[] types)
    {
        
        List<System.Reflection.MethodInfo> methods = new ClusterGenerator().GetType().GetMethods().Where(p => p.GetCustomAttributes(typeof(Notable), false).Select(p2 => p2 as Notable).Where(p2 => types.Intersect(p2.types).Count() > 0).ToList().Count > 0).ToList();
        Dictionary<System.Reflection.MethodInfo, double> method_weight = new Dictionary<System.Reflection.MethodInfo, double>();
        foreach (System.Reflection.MethodInfo method in methods)
        {
            method_weight.Add(method, method.GetCustomAttributes(typeof(Notable), false).Select(p2 => p2 as Notable).First().weight);
        }
        float choice = UnityEngine.Random.Range(0, (float)method_weight.Sum(p => p.Value));
        float currentVal = 0;
        foreach (var pair in method_weight)
        {
            currentVal += (float)pair.Value;
            if (choice < currentVal)
                return pair.Key;
        }
        return method_weight.First().Key;
    }
    public static MethodInfo GetCluster(NodeEffectTypes[] types)
    {
        List<System.Reflection.MethodInfo> methods = new ClusterGenerator().GetType().GetMethods().Where(p => p.GetCustomAttributes(typeof(ClusterAttribute), false).Select(p2 => p2 as ClusterAttribute).Where(p2 => types.Intersect(p2.types).Count() > 0).ToList().Count > 0).ToList();
        Dictionary<System.Reflection.MethodInfo, double> method_weight = new Dictionary<System.Reflection.MethodInfo, double>();
        foreach (System.Reflection.MethodInfo method in methods)
        {
            method_weight.Add(method, method.GetCustomAttributes(typeof(ClusterAttribute), false).Select(p2 => p2 as ClusterAttribute).First().weight);
        }
        float choice = UnityEngine.Random.Range(0, (float)method_weight.Sum(p => p.Value));
        float currentVal = 0;
        foreach (var pair in method_weight)
        {
            currentVal += (float)pair.Value;
            if (choice < currentVal)
                return pair.Key;
        }
        if(methods.Count == 0)
        {

        }
        return method_weight.First().Key;
    }
    #region attackSpeedNodes
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 1000)]
    public static void attackSpeedNormalClusterCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Attack Speed";
                n.values.Add(new NodeValues() { effect = NodeEffects.increased_attack_speed, value = 3, text = $"3% increased attack speed" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 500)]
    public static void attackSpeedNormalClusterUnommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Attack Speed";
                n.values.Add(new NodeValues() { effect = NodeEffects.increased_attack_speed, value = 4, text = $"4% increased attack speed" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 100)]
    public static void attackSpeedNormalClusterRare(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Attack Speed";
                n.values.Add(new NodeValues() { effect = NodeEffects.increased_attack_speed, value = 5, text = $"5% increased attack speed" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    #endregion
    #region damageNodes
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.damage }, 1000)]
    public static void damageNormalClusterCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Damage";
                n.values.Add(new NodeValues() { effect = NodeEffects.increased_damage, value = 8, text = $"8% increased damage" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.damage }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.damage }, 500)]
    public static void damageNormalClusterUnCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Damage";
                n.values.Add(new NodeValues() { effect = NodeEffects.increased_damage, value = 12, text = $"12% increased damage" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.damage }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.damage }, 100)]
    public static void damageNormalClusterRare(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Damage";
                n.values.Add(new NodeValues() { effect = NodeEffects.increased_damage, value = 16, text = $"16% increased damage" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.damage }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    #endregion
    #region crit
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1000)]
    public static void critNormalClusterCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Crit Chance";
                n.values.Add(new NodeValues() { effect = NodeEffects.crit_chance, value = 15, text = $"15% increased Crit Chance" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.crit }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1000)]
    public static void critNormalClusterUnCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Crit Chance";
                n.values.Add(new NodeValues() { effect = NodeEffects.crit_chance, value = 20, text = $"20% increased Crit Chance" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.crit }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1000)]
    public static void critNormalClusterRare(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Crit Chance";
                n.values.Add(new NodeValues() { effect = NodeEffects.crit_chance, value = 25, text = $"25% increased Crit Chance" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.crit }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1000)]
    public static void critDmgNormalClusterCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Crit Damage";
                n.values.Add(new NodeValues() { effect = NodeEffects.crit_damage, value = 10, text = $"10% increased Crit damage" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.crit }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1000)]
    public static void critDmgNormalClusterUnCommon(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Crit Damage";
                n.values.Add(new NodeValues() { effect = NodeEffects.crit_damage, value = 12, text = $"12% increased Crit damage" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.crit }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1000)]
    public static void critDmgNormalClusterRare(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = "Crit Damage";
                n.values.Add(new NodeValues() { effect = NodeEffects.crit_damage, value = 15, text = $"15% increased Crit damage" });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { NodeEffectTypes.crit }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    #endregion
    #region overheat
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 1000)]
    public static void increasedOverheatHoldNormalClusterCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Overheat Capacity", NodeEffects.increased_overheat_capacity, 5, "5% increased Overheat Capacity", NodeEffectTypes.overheat);
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 700)]
    public static void increasedOverheatHoldNormalClusterUnCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Overheat Capacity", NodeEffects.increased_overheat_capacity, 7, "7% increased Overheat Capacity", NodeEffectTypes.overheat);
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 500)]
    public static void increasedOverheatHoldNormalClusterRare(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Overheat Capacity", NodeEffects.increased_overheat_capacity, 10, "10% increased Overheat Capacity", NodeEffectTypes.overheat);
    }

    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 1000)]
    public static void increasedVentNormalClusterCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Vent Speed", NodeEffects.increased_vent_speed, 10, "10% increased Overheat Capacity", NodeEffectTypes.overheat);
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 700)]
    public static void increasedVentNormalClusterUnCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Vent Speed", NodeEffects.increased_vent_speed, 15, "15% increased Overheat Capacity", NodeEffectTypes.overheat);
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 500)]
    public static void increasedventNormalClusterRare(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Vent Speed", NodeEffects.increased_vent_speed, 20, "20% increased Overheat Capacity", NodeEffectTypes.overheat);
    }

    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 1000)]
    public static void decreaseOverheatGenNormalClusterCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "decreased Overheat Generation", NodeEffects.increased_overheat_cost, -2, "2% decreased Overheat Cost", NodeEffectTypes.overheat);
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 700)]
    public static void decreaseOverheatGenNormalClusterUnCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "decreased Overheat Generation", NodeEffects.increased_overheat_cost, -3, "3% decreased Overheat Cost", NodeEffectTypes.overheat);
    }
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 500)]
    public static void decreaseOverheatGenNormalClusterRare(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "decreased Overheat Generation", NodeEffects.increased_overheat_cost, -5, "5% decreased Overheat Cost", NodeEffectTypes.overheat);
    }
    #endregion
    #region Hull
    [ClusterAttribute(new NodeEffectTypes[] { NodeEffectTypes.health }, 1000)]
    public static void increasedHullNormalClusterCommon(List<Node> nodes)
    {
        GenerateNodesClass(nodes, "increased Hull", NodeEffects.increased_hull, 5, "5% increased Hull Strength", NodeEffectTypes.health);
    }
    #endregion
    public static void GenerateNodesClass(List<Node> nodes, string nodeNames, NodeEffects effect, int value, string nodeText, NodeEffectTypes notableEffectType)
    {
        foreach (Node n in nodes)
        {
            if (!n.notable)
            {
                n.Name = nodeNames;
                n.values.Add(new NodeValues() { effect = effect, value = value, text = nodeText });
            }
            else
            {
                GetNotable(new NodeEffectTypes[] { notableEffectType }).Invoke(new ClusterGenerator(), new object[] { n });
            }
        }
    }
    #region AttackSpeedNotables
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 1500)]
    public static void OiledMachine(Node n)
    {
        n.Name = "Oiled Machine";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_attack_speed, value = 10, text = "10% increased attack speed" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 1000)]
    public static void WellOiledMachine(Node n)
    {
        n.Name = "Well Oiled Machine";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_attack_speed, value = 12, text = "12% increased attack speed" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 1000)]
    public static void FinerTippedBullets(Node n)
    {
        n.Name = "Finer Tiped Bullets";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.damage_to_attack, value = 1, text = "Adds 1 damage per level to attacks" } };
    }
  /*  [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed }, 1000)]
    public static void HyperTippedBullets(Node n)
    {
        n.Name = "Hyper Tiped Bullets";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.damage_to_attacks_after_hits, value = 1, text = "Adds 2 damage per level to attacks for 5 seconds after hitting any enemy 3 times " } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed, NodeEffectTypes.health }, 700)]
    public static void RegenBullets(Node n)
    {
        n.Name = "Regeneration Projectiles";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.gain_health_after_x_hits, value = 1, text = "Gain 1 health per level after 3 hits" } };
    }*/
  /*  [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed, NodeEffectTypes.shield }, 700)]
    public static void ShieldBullets(Node n)
    {
        n.Name = "Shielding Projectiles";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.gain_shield_after_x_hits, value = 1, text = "Gain 2 shield per level after 3 hits" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed, NodeEffectTypes.regen }, 700)]
    public static void BarrierBullets(Node n)
    {
        n.Name = "Barrier Projectiles";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.gain_barrier_after_x_hits, value = 1, text = "Gain 3 Barrier per level after 3 hits" } };
    }*/
   /* [Notable(new NodeEffectTypes[] { NodeEffectTypes.attack_speed, NodeEffectTypes.overheat }, 700)]
    public static void HighHeatCapacityProjectiles(Node n)
    {
        n.Name = "High Heat Capacity Projectiles";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.vent_heat_after_hitting_x_times, value = 1, text = "Vent 2 heat after 3 hits" } };
    }*/
    #endregion
    #region HighDamageNotables
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.damage }, 1500)]
    public static void HigherVelocityProjectiles(Node n)
    {
        n.Name = "Higher Velocity Projectiles";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_damage, value = 12, text = "12% increased Damage" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.damage }, 1000)]
    public static void HighestVelocityProjectiles(Node n)
    {
        n.Name = "Highest Velocity Projectiles";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_damage, value = 18, text = "18% increased Damage" } };
    }
   /* [Notable(new NodeEffectTypes[] { NodeEffectTypes.damage, NodeEffectTypes.health }, 700)]
    public static void HealthPercHit(Node n)
    {
        n.Name = "Health Perc Hit";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.gain_perc_of_dmg_hit_health, value = 1, text = "gain 1% of dmg as health" } };
    }*/
   /* [Notable(new NodeEffectTypes[] { NodeEffectTypes.damage, NodeEffectTypes.shield }, 700)]
    public static void ShieldPercHit(Node n)
    {
        n.Name = "Shield Perc Hit";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.gain_perc_of_dmg_hit_shield, value = 1, text = "gain 1% of dmg as shield" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.damage, NodeEffectTypes.regen }, 700)]
    public static void BarrierPercHit(Node n)
    {
        n.Name = "Barrier Perc Hit";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.gain_perc_of_dmg_hit_barrier, value = 1, text = "gain 1% of dmg as barrier" } };
    }*/
   /* [Notable(new NodeEffectTypes[] { NodeEffectTypes.damage, NodeEffectTypes.overheat }, 700)]
    public static void VentPercHit(Node n)
    {
        n.Name = "Barrier Perc Hit";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.vent_perc_of_dmg_hit, value = 1, text = "vent 1% of dmg" } };
    }*/
    #endregion
    #region CriticalNotables
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1500)]
    public static void HighCrit(Node n)
    {
        n.Name = "Higher Crit";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.crit_chance, value = 50, text = "50% increased Crit Chance" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.crit }, 700)]
    public static void HigherCrit(Node n)
    {
        n.Name = "Higher Crit";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.crit_chance, value = 75, text = "75% increased Crit Chance" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.crit }, 1500)]
    public static void HighCritDmg(Node n)
    {
        n.Name = "Higher Crit Dmg";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.crit_damage, value = 25, text = "25% increased Crit Chance" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.crit }, 700)]
    public static void HigherCritDmg(Node n)
    {
        n.Name = "Higher Crit Dmg";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.crit_damage, value = 35, text = "35% increased Crit Chance" } };
    }
    #endregion
    #region OveheatNotables
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 1500)]
    public static void HigherOverheatCap(Node n)
    {
        n.Name = "Higher Overheat Cap";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_overheat_capacity, value = 25, text = "25% increased Overheat Capacity" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 1500)]
    public static void HigherVentSpeed(Node n)
    {
        n.Name = "Higher Vent Speed";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_vent_speed, value = 25, text = "45% increased Vent Speed" } };
    }
    [Notable(new NodeEffectTypes[] { NodeEffectTypes.overheat }, 1500)]
    public static void LowerHeatCost(Node n)
    {
        n.Name = "Lower Vent Speed";
        n.values = new List<NodeValues>() { new NodeValues() { effect = NodeEffects.increased_overheat_cost, value = -15, text = "15% reduced overheat cost" } };
    }
    #endregion
}

