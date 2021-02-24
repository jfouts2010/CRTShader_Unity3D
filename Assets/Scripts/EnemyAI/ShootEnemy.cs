using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LazerEnemy : IEnemyAI
{
    public void Update(GameObject Controller, EnemyController EC, GameObject Player)
    {
        if (EC.aggroed)
        {
            bool seePlayer = EC.SeePlayer();
            if (!seePlayer)
            {
                EC.FindPlayerPoint();
            }
            else
            {
                EC.Abilities.First().Value.ActivateAbility(Controller, Player);
                // ShootWeaponPort(ports.First(), player.transform.position, false);
            }
            EC.AimAtPlayer();
        }
        else
        {
            bool seePlayer = EC.SeePlayer();
            if (seePlayer)
                EC.aggroed = true;
        }
    }
}

