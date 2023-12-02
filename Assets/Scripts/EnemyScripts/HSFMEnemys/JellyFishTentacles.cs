using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishTentacles : MonoBehaviour
{
    public Collider2D TentaclesCollider_Mid;
    public Collider2D TentaclesCollider_Below;
    private void Start()
    {
        TentaclesCollider_Mid.gameObject.SetActive(false);
        TentaclesCollider_Below.gameObject.SetActive(false);
    }
    public void EnableCollider_Mid()
    {
        TentaclesCollider_Mid.gameObject.SetActive(true);
    }
    public void UnEnableCollider_Mid()
    {
        TentaclesCollider_Mid.gameObject.SetActive(false);
    }
    public void EnableCollider_Below()
    {
        TentaclesCollider_Below.gameObject.SetActive(true);
    }
    public void UnEnableCollider_Below()
    {
        TentaclesCollider_Below.gameObject.SetActive(false);
    }
}
