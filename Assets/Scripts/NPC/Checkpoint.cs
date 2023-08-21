using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Checkpoint", menuName = "ScriptableObjects/Checkpoint", order = 1)]
public class Checkpoint : ScriptableObject
{
    public new string name;
    public bool isCompleted;
}
