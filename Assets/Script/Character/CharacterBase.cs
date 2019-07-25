using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterBase : MonoBehaviour
{
	public float moveSpeed = 3f;
	public float attackDelay = 3f;
	public int hp = 3;
	public GameObject hitArea;
	[HideInInspector] public HitAreaScript hitAreaScript;
	public Text UI;
	public bool isHitObstacle;

	public bool attackState;
	public float hitDirection;

	public Animator animator { get; set; }
	public float nextAttackTime { get; set; }
	public bool isMove { get; set; }
	public bool isAlreadyDead { get; set; }

	private Vector3 hitLocation;
	public string playerName;

	protected abstract void Move(Vector3 dir);

	protected abstract void CheckObstacle();

	protected void Attack()
	{
		nextAttackTime = Time.time + attackDelay;
		animator.SetTrigger("IsAttack");
		StartCoroutine(SpawnAttack());
	}

	protected void Dead()
	{
		animator.SetTrigger("IsDeath");
        isAlreadyDead = true;
    }

	protected void Damaged(int damage)
	{
		animator.StopPlayback();
		animator.SetTrigger("IsDameged");

		hp -= damage;
		if (!(UI is null))
			UI.text = gameObject.tag + " HP: " + hp;
		StartCoroutine(DamagedDelay());
	}

	protected void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Obstacle"))
		{
			return;
		}
		HitAreaScript hitpointScript = other.GetComponent<HitAreaScript>();
		if (hitpointScript.attacker != playerName && !isAlreadyDead)
		{
			Damaged(hitpointScript.damage);
		}
	}


	protected virtual void CharacterBehavior()
	{

		if (hp == 0 && !isAlreadyDead)
		{
			Dead();
		}

	}

	IEnumerator SpawnAttack()
	{
		attackState = true;
		hitAreaScript.attacker = playerName;
		hitAreaScript.damage = 1;
		yield return new WaitForSeconds(0.5f);
		hitDirection = transform.rotation.eulerAngles.y;
		hitLocation = gameObject.transform.position;
		hitLocation.y = 0.5f;
		hitLocation.x += Mathf.Sin(Mathf.Deg2Rad * hitDirection);
		hitLocation.z += Mathf.Cos(Mathf.Deg2Rad * hitDirection);
		Instantiate(hitArea, hitLocation, Quaternion.identity);
		yield return new WaitForSeconds(0.1f);
		attackState = false;
	}

	IEnumerator DamagedDelay()
	{
		attackState = true;
		yield return new WaitForSeconds(0.3f);
		attackState = false;
	}
}
