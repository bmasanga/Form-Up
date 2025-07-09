using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private RectTransform rectTransform;
    private Transform objectToFollow;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // Find the first SpriteRenderer in children of root
        var sprite = transform.root.GetComponentInChildren<SpriteRenderer>();
        if (sprite != null)
        {
            objectToFollow = sprite.transform;
        }
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