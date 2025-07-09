using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointsStatusAdapter : MonoBehaviour, IStatusProvider
{
    private IHitPointsReadable hitPoints;

    void Awake()
    {

        hitPoints = transform.root.GetComponentInChildren<IHitPointsReadable>();
    }

    public float GetCurrentValue() => hitPoints != null ? hitPoints.GetCurrentHP() : 0f;
    public float GetMaxValue() => hitPoints != null ? hitPoints.GetMaxHP() : 1f;
}
