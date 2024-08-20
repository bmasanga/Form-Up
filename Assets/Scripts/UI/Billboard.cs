using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform objectToFollow;
    RectTransform rectTransform;

    void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (objectToFollow != null)
        {
            rectTransform.anchoredPosition = objectToFollow.localPosition;
            rectTransform.rotation = Quaternion.identity;

        }
    }
}
