using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 0.1f;

    private Vector3 direction;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (direction != Vector3.zero)
        {
            animator.SetBool("IsMove", true);
        }
        else
        {
            animator.SetBool("IsMove",  false);
        }
    }

    void FixedUpdate()
    {
        Move(direction);

    }

    void Move(Vector3 dir){
        if (dir == Vector3.zero) {
            return;
        }
        transform.Translate(dir * speed * Time.deltaTime);
        transform.Rotate(new Vector3(0, Mathf.Atan(dir.x / dir.z) * 180 / Mathf.PI, 0)
            * Time.fixedDeltaTime,Space.World );


    }


}
