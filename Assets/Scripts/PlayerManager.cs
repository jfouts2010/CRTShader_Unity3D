using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Weapon> WeaponInventory = new List<Weapon>();
    public List<Armor> ArmorInventory = new List<Armor>();
    public List<Board> BoardsInventory = new List<Board>();

    public Weapon MainWeapon;
    public Armor Chasis;
    public Armor Turret;
    public Armor Cockpit;
    public Armor LArm;
    public Armor RArm;

    public List<Armor> AllArmor { get { return new List<Armor>() { Chasis, Turret, Cockpit, LArm, RArm }; } }
    public SkillTree skillTree = new SkillTree();
    public PlayerStats ps;
    public GameManager gm;

    public float playerMovementPerTurn = 30;
    public float experience = 0;
    public int level = 1;
    public int maxHealth { get { return (int)((70f + (10f * (float)level)) * (1f + ps.increasedHull)); } }
    public float health;
    public int maxHeat { get { return (int)(100f * (1f + ps.increasedHeatCap)); } }
    public float currentHeat;
    public float ventPerSecond { get { return (int)(25f * (1f + ps.increasedVentSpeed)); } }
    public string mainWepName;
    private void Start()
    {
        ps = gameObject.GetComponent<PlayerStats>();
        ps.pm = this;
        health = (float)maxHealth;
        currentHeat = 0;
        MainWeapon = Weapon.GenerateWeapon(0);
        Debug.Log(MainWeapon.Name);
        for (int i = 0; i < 10; i++)
            WeaponInventory.Add(Weapon.GenerateWeapon(0));
    }
    public void Update()
    {
        mainWepName = MainWeapon.Name;
    }
}
