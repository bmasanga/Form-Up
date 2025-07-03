using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveData", menuName = "AI/ObjectiveData")]
public class ObjectiveData : ScriptableObject
{
    public List<GameObject> objectives;
}
