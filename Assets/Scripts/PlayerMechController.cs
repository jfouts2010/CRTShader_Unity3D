using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechController : MechBaseController
{
  /*  CharacterController characterController;
    LineRenderer lr;
    PlayerManager pm;
    PlayerStats ps;
    bool VentOpen = false;
    public float Heat = 0;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //setup local variables
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        ps = GameObject.Find("PlayerManager").GetComponent<PlayerStats>();
        characterController = GetComponent<CharacterController>();
        lr = GetComponent<LineRenderer>();
    }
    public void SetupAbilities()
    {
        Abilities.Add(KeyCode.LeftShift, new Dash(gameObject.GetComponent<AbilityController>(), 3));
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        foreach (var ability in Abilities)
        {
            if (Input.GetKeyDown(ability.Key))
                ability.Value.ActivateAbility(this.gameObject, new GameObject());
        }
        Movement();
        RaycastHit? hit = MouseRaycast();
        PositionAndRotation(hit);
        ShootAction(hit);
        VentAction();
    }
    public void Movement()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            input += new Vector3(-1, 0, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += new Vector3(1, 0, -1);
        }
        if (Input.GetKey(KeyCode.W))
        {
            input += new Vector3(1, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            input += new Vector3(-1, 0, -1);
        }
        input = input.normalized;
        if (input != Vector3.zero)
            Chassis.transform.forward = input;
        characterController.Move(input * (mechInfo.baseSpeed * Time.deltaTime));
    }
    public void PositionAndRotation(RaycastHit? hit)
    {
        RaycastHit? floorHit = PointRaycastFloor(transform.position + new Vector3(0, 1, 0), Vector3.down);
        if (floorHit.HasValue)
        {
            transform.position = Vector3.Lerp(transform.position + new Vector3(0, 1, 0), floorHit.Value.point + new Vector3(0, 1, 0), 1);
        }

        if (hit.HasValue)
        {
            Vector3 playerToMouse = hit.Value.point - transform.position;
            Quaternion forward = Quaternion.LookRotation(playerToMouse);
            forward.x = 0;
            forward.z = 0;
            Turret.transform.rotation = Quaternion.Lerp(Turret.transform.rotation, forward, .1f);
            Quaternion lightForward = forward;
            //lr.SetPositions(new Vector3[] { GunPoint.position, hit.Value.point });
        }
    }
    public void ShootAction(RaycastHit? hit)
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            if (hit.HasValue)
            {
                foreach (var port in ports)
                {
                    // if (Time.time > port.weapon.shootTime + port.weapon.lastWeaponFireTime)
                    {
                        if ((port.weapon.wepInfo.fireType == FireType.semiauto && Input.GetMouseButtonDown(0)) || (port.weapon.wepInfo.fireType == FireType.automatic && Input.GetMouseButton(0)))
                        {
                            ShootWeaponPort(port, hit.Value.point);
                        }
                    }
                }
            }
        }
    }
    public void VentAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
            VentOpen = !VentOpen;
        if (Input.GetKey(KeyCode.R))
            if (VentOpen)
                if (Heat > 0)
                {
                    Heat -= ps._TotalVentSpeed * Time.deltaTime;
                }
    }
    public RaycastHit? MouseRaycast()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;
        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, 1000, floorMask))
        {
            return floorHit;
        }
        return null;
    }
    public RaycastHit? PointRaycast(Vector3 origin, Vector3 direction)
    {
        Ray camRay = new Ray()
        {
            origin = origin,
            direction = direction
        };

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 1000))
        {
            return floorHit;
        }
        return null;
    }
    public RaycastHit? PointRaycastFloor(Vector3 origin, Vector3 direction)
    {
        Ray camRay = new Ray()
        {
            origin = origin,
            direction = direction
        };

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 1000, floorMask))
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

            for (int i = 0; i < port.weapon.projectileCount; i++)
            {
                float angle = UnityEngine.Random.Range(0, port.weapon.spreadDeg);
                if (UnityEngine.Random.Range(0, 2) == 1)
                    angle = -angle;
                float y = UnityEngine.Random.Range(0, Mathf.Tan(port.weapon.spreadDeg * Mathf.Deg2Rad) * port.weapon.velocity);
                if (UnityEngine.Random.Range(0, 2) == 1)
                    y = -y;
                GameObject bullet = Instantiate(port.weapon.wepInfo.BulletPrefab, weaponFirePoint, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, angle, 0) * (bulletTraj.normalized * port.weapon.velocity) + new Vector3(0,y,0);
                bullet.GetComponent<BulletCode>().playerBullet = player;
            }

            port.weapon.lastWeaponFireTime = Time.time;
            Heat += port.weapon.heatOutputPerShot;
        }
    }*/
}
