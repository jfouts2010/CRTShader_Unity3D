using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationEnemy
{
    public EnemyAttackType attackType;
    public float health;
    public float maxHealth;
    public float attackDamage;
    public float attackTime;
    public float movement;
    public int level;
    public float size;
    public float xpos;
    public string name;
    public float timeTillNextAttack;
    public static SimulationEnemy GenerateEnemy(int level, int enemyNumber)
    {
        SimulationEnemy se = new SimulationEnemy()
        {
            attackType = (EnemyAttackType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EnemyAttackType)).Length),
            maxHealth = 10,
            health = 10,
            attackDamage = 10,
            movement = 10,
            attackTime = 2,
            xpos = UnityEngine.Random.Range(-20, 20),
            size = 0.5f,
            timeTillNextAttack = 0,
            name = enemyNumber.ToString()
        };
        if (se.attackType == EnemyAttackType.ranged)
            se.attackDamage = 5;
        return se;
    }
}
public enum EnemyAttackType
{
    ranged = 0,
    melee = 1,
    void_ = 2
}
