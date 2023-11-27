using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatNest : MonoBehaviour
{
    public Transform pivotPoint;
    public Vector2 spawnOffset;
    public GameObject Bat;
    private IEnumerator birthCoroutine;
    private bool isStartBirth = false;
    public float birthOffsetSce;
    public float birthSce;

    void Start()
    {
        birthCoroutine = Birth(birthSce);
    }
    private void Update()
    {
        if (birthOffsetSce > 0)
        {
            // Debug.Log("倒數中 " + birthOffsetSce);
            birthOffsetSce -= Time.time;
        }
        else
        {
            if (!isStartBirth)
            {
                // Debug.Log("啟動 " + birthOffsetSce);

                isStartBirth = true;
                StartCoroutine(birthCoroutine);
            }
        }
    }

    IEnumerator Birth(float sce)
    {
        while (true)
        {
            Instantiate(Bat, (Vector2)pivotPoint.transform.position + spawnOffset, Quaternion.identity);
            yield return new WaitForSeconds(sce);
        }

    }
}
