using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public interface IEnemyAI 
{
    void Update(GameObject Controller, EnemyController EC, GameObject Player);
}
[SerializeField]
public enum AIType
{
    Lazer,
    Shoot
}

