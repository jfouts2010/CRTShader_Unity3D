using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MechBaseController
{
    NavMeshAgent agent;
    public GameObject player;
    public bool aggroed;
    int minDist = 2;
    int maxDist = 10;
    int aggroRange = 15;
    IEnemyAI AI;
    public AIType AIName;

    public int testValue = 0;
    bool test => testValue > 1;
    bool oldtest { get { return testValue > 1; } }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //get the AI classes, only way to set it and I hate it
        Dictionary<AIType, IEnemyAI> AIOptions = new Dictionary<AIType, IEnemyAI>();
        AIOptions.Add(AIType.Lazer, new LazerEnemy());
        AIOptions.Add(AIType.Shoot, new ShootEnemy());
        AI = AIOptions[AIName];
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        aggroed = false;
        agent.speed = mechInfo.baseSpeed;
        Abilities.Add(KeyCode.LeftShift, new Beam(gameObject.GetComponent<AbilityController>(), 3));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        AI.Update(this.gameObject, this, player);
    }
    public void AimAtPlayer()
    {
        Quaternion forward = Quaternion.LookRotation(player.transform.position - Turret.transform.position);
        Turret.transform.rotation = Quaternion.Lerp(Turret.transform.rotation, forward, 0.1f);
    }
    public bool SeePlayer()
    {
        RaycastHit? hit = PointRaycastPlayer(transform.position, player.transform.position - transform.position);
        if (hit.HasValue)
        {
            return hit.Value.transform.tag == "Player";
        }
        else
            return false;
    }
    public void FindPlayerPoint()
    {
        //see if you can just walk right to the player
        float distanceToPlayer = minDist;
        Vector3 EnemyToPlayer = transform.position - player.transform.position;
        if (EnemyToPlayer.magnitude > maxDist)
            distanceToPlayer = maxDist;
        else if (EnemyToPlayer.magnitude < minDist)
            distanceToPlayer = minDist;
        else
            distanceToPlayer = EnemyToPlayer.magnitude;

        Vector3 Destination = (EnemyToPlayer.normalized * distanceToPlayer) + player.transform.position;
        NavMeshPath testPath = new NavMeshPath();
        agent.CalculatePath(Destination, testPath);
        Vector3 finalPoint = testPath.corners[testPath.corners.Length - 1];
        //can we see the player?
        RaycastHit? hit = PointRaycastBuildings(finalPoint, player.transform.position - finalPoint);
        if (hit.HasValue)
        {
            for (int i = 0; i < 90; i++)
            {
                Vector3 vector = Quaternion.Euler(0, i, 0) * (transform.position - player.transform.position);
                Destination = ((vector).normalized * distanceToPlayer) + player.transform.position;
                testPath = new NavMeshPath();
                agent.CalculatePath(Destination, testPath);
                if (testPath.corners.Length > 0)
                {
                    finalPoint = testPath.corners[testPath.corners.Length - 1];
                    //can we see the player?
                    hit = PointRaycastBuildings(finalPoint, player.transform.position - finalPoint);
                    if (!hit.HasValue)
                    {
                        Debug.DrawLine(finalPoint, player.transform.position, Color.green, 10);
                        agent.SetDestination(finalPoint);
                        break;
                    }
                    else
                    {
                        //Debug.DrawLine(finalPoint, hit.Value.point, Color.red, 10);
                    }
                }

                vector = Quaternion.Euler(0, -i, 0) * (transform.position - player.transform.position);
                Destination = ((vector).normalized * distanceToPlayer) + player.transform.position;
                testPath = new NavMeshPath();
                agent.CalculatePath(Destination, testPath);
                if (testPath.corners.Length > 0)
                {
                    finalPoint = testPath.corners[testPath.corners.Length - 1];
                    hit = PointRaycastBuildings(finalPoint, player.transform.position - finalPoint);
                    if (!hit.HasValue)
                    {
                        Debug.DrawLine(finalPoint, player.transform.position, Color.green, 10);
                        agent.SetDestination(finalPoint);
                        break;
                    }
                    else
                    {
                        //Debug.DrawLine(finalPoint, hit.Value.point, Color.red, 10);
                    }
                }
            }
        }
        else
        {
            agent.SetDestination(finalPoint);
        }
    }
    public RaycastHit? PointRaycastBuildings(Vector3 origin, Vector3 direction)
    {
        Ray camRay = new Ray()
        {
            origin = origin,
            direction = direction
        };

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 1000, LayerMask.GetMask("Building")))
        {
            return floorHit;
        }
        return null;
    }
    public RaycastHit? PointRaycastPlayer(Vector3 origin, Vector3 direction)
    {
        Ray camRay = new Ray()
        {
            origin = origin,
            direction = direction
        };

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, aggroRange, ~LayerMask.GetMask("Enemy")))
        {
            return floorHit;
        }
        return null;
    }
    public void ShootWeaponPort(WeaponPort port, Vector3 target, bool player = true)
    {
        if (Time.time > port.weapon.shootTime + port.weapon.lastWeaponFireTime)
        {
            Vector3 weaponFirePoint = port.weapon.FiringPoint.transform.position;
            Vector3 bulletTraj = target - weaponFirePoint;
            GameObject bullet = Instantiate(port.weapon.wepInfo.BulletPrefab, weaponFirePoint, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = bulletTraj.normalized * port.weapon.velocity;
            bullet.GetComponent<BulletCode>().playerBullet = player;
            port.weapon.lastWeaponFireTime = Time.time;
        }
    }
}
