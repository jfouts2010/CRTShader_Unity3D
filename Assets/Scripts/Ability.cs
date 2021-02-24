using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IAbility
{
    void ActivateAbility(GameObject Controller, GameObject Player);
    bool CanActivate(float lastActivated);
}
public class Dash : IAbility
{
    private bool active = false;
    private readonly float cooldown;
    private float lastActivated;
    private AbilityController abilController;
    public Dash(AbilityController abilControl, float cd)
    {
        cooldown = cd;
        abilController = abilControl;
    }
    public void ActivateAbility(GameObject Controller, GameObject Player)
    {
        float dashDist = 3;
        float speedMult = 3;
        if (CanActivate(lastActivated))
        {
            if (Controller.tag == "Player")
            {
                PlayerMechController mechControl = Controller.GetComponent<PlayerMechController>();
                abilController.StartCoroutine(abilController.DashCor(dashDist, speedMult));
                lastActivated = Time.time;
            }
        }
    }

    public bool CanActivate(float lastActivated)
    {
        if (!active)
            return Time.time > lastActivated + cooldown;
        else return false;
    }
}
public class Beam : IAbility
{
    private bool active = false;
    private readonly float cooldown;
    private float lastActivated;
    private AbilityController abilController;
    public Beam(AbilityController abilControl, float cd)
    {
        cooldown = cd;
        abilController = abilControl;
    }
    public void ActivateAbility(GameObject Controller, GameObject Player)
    {
        if (CanActivate(lastActivated))
        {
            if (Controller.tag == "Enemy")
            {
                EnemyController mechControl = Controller.GetComponent<EnemyController>();
                abilController.StartCoroutine(abilController.MonsterBeamCor(mechControl.ports.First(), Player, 1, .5f, .5f));
                lastActivated = Time.time;
            }
        }
    }
    public bool CanActivate(float lastActivated)
    {
        if (!active)
            return Time.time > lastActivated + cooldown;
        else return false;
    }
}
