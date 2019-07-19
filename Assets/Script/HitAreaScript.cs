using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAreaScript : MonoBehaviour
{
    public string myTag;
    public float time = 0.5f;

    private void Update()
    {
        Destroy(gameObject,time);
    }
}
