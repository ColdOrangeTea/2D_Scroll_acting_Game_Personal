using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingAnimation : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public SpriteRenderer Effect;
    void Start()
    {
        Sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Effect = transform.GetChild(1).GetComponent<SpriteRenderer>();
        Effect.gameObject.SetActive(false);

    }
}
