using UnityEngine;

public class HitAreaScript : MonoBehaviour
{
    public string attacker;
    public float time = 0.1f;
    public int damage = 1;
    public bool isMoving;

    private void Update()
    {
        Destroy(gameObject, time);
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (isMoving)
        {
            if (other.CompareTag("Enemy") && other.GetComponent<CharacterBase>().hp <= 0)
                return;
            Destroy(gameObject);
        }
    }
}
