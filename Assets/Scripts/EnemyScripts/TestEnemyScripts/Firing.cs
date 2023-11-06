using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    public EnemyAttribute Attribute;
    [Header("ReadfromData")]
    public float Cooldown;
    public float Duration;
    public float FireRange;
    public float Force;

    [Header("ManualAccessVVV")]
    public Transform target;
    public GameObject gun;
    public GameObject bullet;
    public Transform shootpoint;
    Vector2 Direction;
    bool Detected = false;
    float nextTimeToFire = 0;
    // Start is called before the first frame update
    void Start()
    {
        //     Cooldown;
        //    Duration;
        //      FireRange;
        //      force;
        if (target == null)
        {
            // targetedPerson = GameObject.FindGameObjectWithTag(turrentData.targetTagName).GetComponent<Transform>();
        }
        StartCoroutine(Detection());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = target.transform.position;
        Direction = targetPos - (Vector2)transform.position;
    }
    void shoot()
    {
        GameObject BulletIns = Instantiate(bullet, shootpoint.position, gun.transform.rotation);
        BulletIns.GetComponent<Rigidbody2D>().velocity = Direction * Force * 0.1f;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, FireRange);
    }
    IEnumerator Detection()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //Debug.Log("2isOn");
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, FireRange);
            if (rayInfo)
            {
                // if (rayInfo.collider.gameObject.tag == turrentData.targetTagName)
                // {
                //     if (Detected == false)
                //     {
                //         Detected = true;
                //         Debug.Log("trueonein");
                //     }
                // }
                // else
                // {

                //     if (Detected == true)
                //     {
                //         Detected = false;
                //     }
                // }
                if (Detected)
                {
                    gun.transform.right = Direction;
                    shoot();
                }
            }
        }
    }
}