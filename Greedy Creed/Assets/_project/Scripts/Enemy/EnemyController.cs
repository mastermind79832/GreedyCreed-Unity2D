using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyController : MonoBehaviour
{
	[Tooltip ("Attackrange = detection range & no movement")]
    public bool isTurretMode;
    public float attackRange;
    public float detectionRange;

	[Header ("Bullet")]
	public Transform aim;
	public BulletScript bullet;
	public float fireRate;

	private bool m_InAttackRange, m_InDetectionRange, m_FireTimeout;
	private PlayerController player;
	
	private void Start()
	{
		player = PlayerController.Instance;
		if (isTurretMode)
			detectionRange = attackRange;
	}

    void Update()
    {
		CheckDetection();
		SetState();
    }

	private void SetState()
	{
		if (m_InDetectionRange)
		{
			RotateTo(player.GetLocation());
		}
		if (m_InAttackRange && !m_FireTimeout)
		{
			StartCoroutine(FireBullet());
		}
	}

	IEnumerator FireBullet()
	{
		m_FireTimeout = true;
		GameObject newBullet = Instantiate<GameObject>(bullet.gameObject,aim.transform.position,transform.rotation);
		newBullet.transform.parent = transform.parent;
		yield return new WaitForSeconds(1/fireRate);
		m_FireTimeout = false;
	}

	private void RotateTo(Vector3 playerLocation)
	{
		Vector2 distance = playerLocation - transform.position;
		float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg - 90;
		float rotation = Mathf.LerpAngle(transform.rotation.z, angle, 3);
		transform.rotation = Quaternion.Euler(0f, 0f, rotation);
	}

	private void CheckDetection()
	{
		m_InDetectionRange = CheckPlayerinRange(detectionRange);
		m_InAttackRange = CheckPlayerinRange(attackRange);
	}

	private bool CheckPlayerinRange(float range)
    {
        float distance = Vector3.Distance(player.GetLocation(), transform.position);
        if (distance > range)
            return false;

        return true;
    }

    #region Editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
	{
		if (!isTurretMode)
		{
			Handles.color = new Color(0f, 1.0f, 0, 0.1f);
			Handles.DrawSolidDisc(transform.position, Vector3.back, detectionRange);
		}
		Handles.color = new Color(1.0f, 0, 0, 0.1f);
		Handles.DrawSolidDisc(transform.position, Vector3.back, attackRange);
	}
	#endif
	#endregion
}
