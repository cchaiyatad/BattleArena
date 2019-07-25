using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Player;
    private Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        Offset = Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position - Offset;
    }
}
