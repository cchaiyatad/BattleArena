using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody rigidbody;
            
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move(new Vector3(Input.GetAxis("Horizontal"),0 ,Input.GetAxis("Vertical")));
    }

    void Move(Vector3 direction){
        rigidbody.AddForce(direction * speed);
    }
}
