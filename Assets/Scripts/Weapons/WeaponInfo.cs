using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EnumsForUnity;

public class IsWeapon : System.Attribute
{
}
    public class WeaponInfo 
{
    public GameObject GunPrefab;
    public GameObject BulletPrefab;
    public float heatOutputPerShotBase;
    public float shootTimeBase;
    public float velocityBase;
    public float bulletDamageBase;
    public int projectileCount;
    public float spreadDeg;
    public float crit;
    public WeaponType type;
    public ProjectileType projType;
    public FireType fireType;
    public string Name;
    public string ImageName;
    [IsWeapon]
    public WeaponInfo Shotgun()
    {
        return new WeaponInfo()
        {
            heatOutputPerShotBase = 15, //damage per mag 80
            shootTimeBase = 1,
            velocityBase = .7f,
            bulletDamageBase = 1,//dps 12
            projectileCount = 12,
            spreadDeg = 25,
            projType = ProjectileType.BasicProjectile,
            fireType = FireType.semiauto,
            type = WeaponType.Shotgun,
            crit = 5,
            Name = "Shotgun",
            ImageName = "ShotgunOutline"
        };
    }
    [IsWeapon]
    public WeaponInfo AssaultRifle()
    {
        return new WeaponInfo()
        {
            heatOutputPerShotBase = 4, //damage per mag 50
            shootTimeBase = .25f,
            velocityBase = 1f,
            bulletDamageBase = 2, //dps 8
            projectileCount = 1,
            spreadDeg = 10,
            projType = ProjectileType.BasicProjectile,
            fireType = FireType.automatic,
            type = WeaponType.AR,
            crit = 6,
            Name = "Assault Rifle",
            ImageName = "ShotgunOutline"
        };
    }
    [IsWeapon]
    public WeaponInfo SubMachineGun()
    {
        return new WeaponInfo()
        {
            heatOutputPerShotBase = 4, //damage per mag 37.5
            shootTimeBase = .1f,
            velocityBase = .8f,
            bulletDamageBase = 1.5f, //dps 15
            projectileCount = 1,
            spreadDeg = 15,
            projType = ProjectileType.BasicProjectile,
            fireType = FireType.automatic,
            type = WeaponType.SMG,
            crit = 4,
            Name = "SubMachineGun",
            ImageName = "ShotgunOutline"
        };
    }
    [IsWeapon]
    public WeaponInfo MachineGun()
    {
        return new WeaponInfo()
        {
            heatOutputPerShotBase = 2f, //damage per mag 80
            shootTimeBase = .4f,
            velocityBase = 1f,
            bulletDamageBase = 2, //dps 5
            projectileCount = 1,
            spreadDeg = 10,
            projType = ProjectileType.BasicProjectile,
            fireType = FireType.automatic,
            type = WeaponType.MG,
            crit = 5,
            Name = "Machine Gun",
            ImageName = "ShotgunOutline"
        };
    }
    [IsWeapon]
    public WeaponInfo Rifle()
    {
        return new WeaponInfo()
        {
            heatOutputPerShotBase = 12, //damage per mag 50
            shootTimeBase = 1f,
            velocityBase = 1.2f,
            bulletDamageBase = 6,//dps 6
            projectileCount = 1,
            spreadDeg = 2,
            projType = ProjectileType.BasicProjectile,
            fireType = FireType.automatic,
            type = WeaponType.Rifle,
            crit = 7,
            Name = "Rifle",
            ImageName = "ShotgunOutline"
        };
    }
    [IsWeapon]
    public WeaponInfo SniperRifle()
    {
        return new WeaponInfo()
        {
            heatOutputPerShotBase = 25, //damage per mag 40
            shootTimeBase = 1.5f,
            velocityBase = 1.5f,
            bulletDamageBase = 10,//dps 6.66
            projectileCount = 1,
            spreadDeg = 1,
            projType = ProjectileType.BasicProjectile,
            fireType = FireType.automatic,
            type = WeaponType.Sniper,
            crit = 8,
            Name = "Sniper Rifle",
            ImageName = "ShotgunOutline"
        };
    }

}
