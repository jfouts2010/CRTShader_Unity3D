using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MechInfo", menuName = "ScriptableObjects/MechInfo", order = 1)]
public class MechInfo : ScriptableObject
{
    public List<Vector3> portTransforms;
    public float hullHealth;
    public float baseSpeed;
}
