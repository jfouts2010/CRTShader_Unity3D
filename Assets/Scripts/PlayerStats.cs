using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    SkillTree st;
    public PlayerManager pm;
    public List<NodeValues> takenNodeValues { get { return st.takenNodes.Count > 0 ? st.takenNodes.SelectMany(p => p.values).ToList() : new List<NodeValues>(); } }
    //Vent
    public float BASE_VENT_AMOUNT = 1;

    //Heat
    public float increasedHeatCost { get { return GetNodes(NodeEffects.increased_overheat_cost); } }
    public float increasedHeatCap { get { return GetNodes(NodeEffects.increased_overheat_capacity); } }
    public float increasedVentSpeed { get { return GetNodes(NodeEffects.increased_vent_speed); } }

    //attack speed
    public float increasedAttackSpeed { get { return GetNodes(NodeEffects.increased_vent_speed); } }
    public float addedDamageToAttacks { get { return GetNodes(NodeEffects.damage_to_attack); } }

    //damage
    public float increasedDamage { get { return GetNodes(NodeEffects.increased_damage); } }

    //crit
    public float increasedCrit { get { return GetNodes(NodeEffects.crit_chance); } }
    public float increasedCritDamage { get { return GetNodes(NodeEffects.crit_damage); } }

    //hull
    public float increasedHull { get { return GetNodes(NodeEffects.increased_hull); } }

    //hull
    public float increasedMS { get { return GetNodes(NodeEffects.increased_ms); } }

    private void Awake()
    {
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        st = pm.skillTree;
    }
    public float GetNodes(NodeEffects effect)
    {
        return takenNodeValues.Any(p => p.effect == effect) ? (float)takenNodeValues.Where(p => p.effect == effect).Sum(p => p.value) : 0;
    }
}
