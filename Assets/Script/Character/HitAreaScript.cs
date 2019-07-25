using UnityEngine;

public class HitAreaScript : MonoBehaviour
{
    public string attacker;
    public float time = 0.3f;
    public int damage = 1;

    private void Update()
    {
        Destroy(gameObject,time);
    }
}
