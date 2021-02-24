using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public float playerXPos;
    public float timeTillNextFire = 0;
    public float timeLeftInTurn;
    public float TURNTIME = 1;
    public bool run = true;
    public PlayerManager pm;
    public PlayerStats ps;
    public int turn = 0;
    public Dictionary<int, string> playerLog = new Dictionary<int, string>();
    public Dictionary<int, string> enemyLog = new Dictionary<int, string>();
    // Start is called before the first frame update
    public List<SimulationEnemy> Enemies = new List<SimulationEnemy>();
    void Start()
    {
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        ps = pm.ps;
        for (int i = 0; i < 5; i++)
            Enemies.Add(SimulationEnemy.GenerateEnemy(0, i));
        timeLeftInTurn = TURNTIME;
        timeTillNextFire = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NextTurn()
    {
        if (Enemies.Any(p => p.health > 0) && pm.health > 0)
        {
            turn++;
            playerLog.Add(turn, "");
            enemyLog.Add(turn, "");
            // playerLog[turn] += ($"Turn {turn}\n");
            PlayerTurn();
            EnemiesTurn();
        }
    }
    public void PlayerTurn()
    {
        //let the player go first
        //find highest chance to hit enemy and fire
        Dictionary<SimulationEnemy, float> enemies_hitchance = new Dictionary<SimulationEnemy, float>();
        foreach (SimulationEnemy enemy in Enemies)
        {
            float chanceToHit = ChanceToHit(pm.MainWeapon, enemy);
            enemies_hitchance.Add(enemy, chanceToHit);

        }
        enemies_hitchance = enemies_hitchance.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, p => p.Value);

        while (timeLeftInTurn > timeTillNextFire && Enemies.Any(p => p.health > 0))
        {
            SimulationEnemy target = enemies_hitchance.FirstOrDefault(p => p.Key.health > 0).Key;
            float chanceToHit = enemies_hitchance.FirstOrDefault(p => p.Key.health > 0).Value;

            //if chance to hit is too low, move closer
            if (chanceToHit < .75f && Mathf.Abs(target.xpos - playerXPos) > 1)
            {
                //move in .1s incriments
                float movement = pm.playerMovementPerTurn / 10f;
                if (target.xpos - playerXPos > 0)
                    playerXPos += movement;
                else
                    playerXPos -= movement;
                playerLog[turn] += ($"Too far ACC is {chanceToHit}, moving to closest enemy {target.name} at {target.xpos}\n");
                timeLeftInTurn -= .1f;
                timeTillNextFire -= .1f;
                if (timeTillNextFire < 0)
                    timeTillNextFire = 0;
                foreach (SimulationEnemy enemy in Enemies)
                    enemies_hitchance[enemy] = ChanceToHit(pm.MainWeapon, enemy);
            }
            else
            {
                if (pm.currentHeat < pm.maxHeat)
                {
                    timeLeftInTurn -= timeTillNextFire;
                    timeTillNextFire = pm.MainWeapon.shootTime;
                    //FIRE
                    pm.currentHeat += pm.MainWeapon.heatOutputPerShot;
                    float damage = 0;
                    for (int i = 0; i < pm.MainWeapon.projectileCount; i++)
                    {
                        if (UnityEngine.Random.Range(0, 100) > (1 - (chanceToHit * 100f)))
                            damage += pm.MainWeapon.bulletDamage;
                    }

                    float preDamageHealth = target.health;
                    target.health -= damage;
                    if (damage > 0)
                        playerLog[turn] += ($"Hit Enemy_{target.name} for {damage}, has {target.health} health left\n");
                    else
                        playerLog[turn] += ($"Missed OV: {pm.currentHeat}\n");
                    if (preDamageHealth < damage)
                        playerLog[turn] += ($"Killed Enemy_{target.name}\n");
                }
                else
                {
                    //overheated, need to vent
                    if (pm.currentHeat > pm.maxHealth / 2f) //good health, can vent to zero
                    {
                        while (timeLeftInTurn > 0 || pm.currentHeat < 0f)
                        {
                            timeLeftInTurn -= .1f;
                            timeTillNextFire -= .1f;
                            if (timeTillNextFire < 0)
                                timeTillNextFire = 0;
                            pm.currentHeat -= pm.ventPerSecond / 10f;
                            playerLog[turn] += ($"Venting, currently at {pm.currentHeat}\n");
                        }
                    }
                    else //too low, vent to 50%
                    {
                        while (timeLeftInTurn > 0 || pm.currentHeat > pm.maxHeat / 2f)
                        {
                            timeLeftInTurn -= .1f;
                            timeTillNextFire -= .1f;
                            if (timeTillNextFire < 0)
                                timeTillNextFire = 0;
                            pm.currentHeat -= pm.ventPerSecond / 10f;
                            playerLog[turn] += ($"Venting, currently at {pm.currentHeat}\n");
                        }
                    }
                }
            }
            if (!Enemies.Any(p => p.health > 0))
            {
                playerLog[turn] += ("All Enemies Dead\n");
                break;
            }
        }
        timeTillNextFire -= timeLeftInTurn;
        if (timeTillNextFire < 0)
            timeTillNextFire = 0;
        timeLeftInTurn = TURNTIME;
    }
    public void EnemiesTurn()
    {
        enemyLog[turn] += ("Enemies Turn\n");
        //ok enemies turn
        if (Enemies.Any(p => p.health > 0))
            foreach (SimulationEnemy enemy in Enemies.Where(p => p.health > 0))
            {
                enemyLog[turn] += ($"Enemy_{enemy.name}\n");
                float distance = Mathf.Abs(playerXPos - enemy.xpos);
                if (enemy.attackType == EnemyAttackType.melee || enemy.attackType == EnemyAttackType.void_)
                {

                    if (distance < 1.5f)
                    {
                        if (timeLeftInTurn > enemy.timeTillNextAttack)
                        {
                            pm.health -= enemy.attackDamage;
                            enemyLog[turn] += ($"    attacks player for {enemy.attackDamage}, player is at {pm.health} health\n");
                            enemy.timeTillNextAttack = enemy.attackTime;
                        }
                        else
                            enemy.timeTillNextAttack -= TURNTIME;
                    }
                    else
                    {
                        //move closer and lunge for half damage
                        if (distance < enemy.movement)
                        {
                            enemy.xpos = playerXPos + 0.5f;
                            pm.health -= enemy.attackDamage / 2f;
                            enemyLog[turn] += ($"    Enemy_{enemy.name} is too far from player at {playerXPos}, lunged for {enemy.attackDamage / 2f}, player is at {pm.health} health\n");
                            enemy.timeTillNextAttack -= TURNTIME;
                        }
                        else
                        {
                            if (playerXPos - enemy.xpos > 0)
                                enemy.xpos += enemy.movement;
                            else
                                enemy.xpos -= enemy.movement;
                            enemyLog[turn] += ($"    Enemy_{enemy.name} is too far from player at {playerXPos} and moved to {enemy.xpos}\n");
                            enemy.timeTillNextAttack -= TURNTIME;
                        }

                    }

                }
                if (enemy.attackType == EnemyAttackType.ranged)
                {
                    if (timeLeftInTurn > enemy.timeTillNextAttack)
                    {
                        //attacks and always has 75% chance to hit
                        if (UnityEngine.Random.Range(0, 100) > 25)
                        {
                            pm.health -= enemy.attackDamage;
                            enemyLog[turn] += ($"    Enemy_{enemy.name} ranged attacked Player and hit, player is at {pm.health} health\n");
                        }
                        else
                            enemyLog[turn] += ($"    Enemy_{enemy.name} ranged attacked Player and missed\n");
                        enemy.timeTillNextAttack = enemy.attackTime;
                    }
                    else
                        enemy.timeTillNextAttack -= TURNTIME;
                }
                if (pm.health <= 0)
                {
                    enemyLog[turn] += ("Player died\n");
                    break;
                }
                timeLeftInTurn = TURNTIME;
            }
        timeLeftInTurn = TURNTIME;
    }
    public float ChanceToHit(Weapon wep, SimulationEnemy enemy)
    {
        float distance = Mathf.Abs(enemy.xpos - playerXPos);

        float spreadhalfDist = Mathf.Tan(wep.spreadDeg * Mathf.Deg2Rad) * distance;
        float chanceToHitFromSpread = enemy.size / spreadhalfDist * 2;

        float chanceToHitFromVelocity = (40f / (40 + distance)) * (wep.velocity / (enemy.movement / 10f));

        return chanceToHitFromSpread * chanceToHitFromVelocity;
    }
}
