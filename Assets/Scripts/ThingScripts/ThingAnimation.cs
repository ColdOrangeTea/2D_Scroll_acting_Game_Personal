using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingAnimation : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public SpriteRenderer Effect;
    public ParticleSystem PS;
    void Start()
    {
        Sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Effect = transform.GetChild(1).GetComponent<SpriteRenderer>();
        PS = transform.GetChild(1).GetComponent<ParticleSystem>();

        Effect.gameObject.SetActive(false);

    }
}
