using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAreaScript : MonoBehaviour
{
    public string attacker;
    public float time = 0.3f;
    public int damage = 1;

    public void OnHit(){

    }

    private void Update()
    {
        Destroy(gameObject,time);
    }
}
