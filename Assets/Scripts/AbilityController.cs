using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    //abilityCoroutines
    //have to do here cuz requires coroutine, which cant be on ability
    public IEnumerator DashCor(float dashDist, float speedMult)
    {
        CharacterController cc = gameObject.GetComponent<CharacterController>();
        Vector3 forward = cc.velocity.normalized * dashDist * Time.deltaTime * speedMult;
        Vector3 start = transform.position;
        while ((transform.position - start).magnitude < dashDist)
        {
            cc.Move(forward);
            yield return new WaitForSeconds(0);
        }
    }
    public IEnumerator MonsterBeamCor(WeaponPort port, GameObject player, float chargeUpTime, float fireDelay, float fireDuration)
    {
        float startTime = Time.time;
        EnemyController emc = gameObject.GetComponent<EnemyController>();
        
        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = .1f;
        lr.endWidth = .1f;
        while (startTime + chargeUpTime > Time.time)
        {
            Vector3 forward = emc.Turret.transform.forward.normalized;
            Vector3 start = port.weapon.FiringPoint.transform.position;
            lr.SetPositions(new Vector3[] { start, start+ (forward * 10) });
            yield return new WaitForSeconds(0);
        }
        emc.CanMove = false;
        emc.CanTurn = false;
        yield return new WaitForSeconds(fireDelay);
        //create beam
        lr.startWidth = 1;
        lr.endWidth = 1;
        yield return new WaitForSeconds(fireDuration);
        //destroy beam
        emc.CanTurn = true;
        emc.CanMove = true;
        Destroy(lr);
    }
}
