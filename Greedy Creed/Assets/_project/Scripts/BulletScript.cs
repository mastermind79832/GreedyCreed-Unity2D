using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public float bulletPower = 5;
	public float travelSpeed;
	public CircleCollider2D col;
	public Animator anim;

	private bool m_IsBlasted = false;

	private void OnEnable()
	{
		StartCoroutine(SetTimer());
	}

	IEnumerator SetTimer()
	{
		yield return new WaitForSeconds(1.2f);
		StartCoroutine(DestroyBullet());
	}

	private void FixedUpdate()
	{
		if(!m_IsBlasted)
			transform.localPosition += Time.fixedDeltaTime * travelSpeed * transform.up;
	}

	public void BulletHit()
	{
		StartCoroutine(DestroyBullet());
	}
	
	IEnumerator DestroyBullet()
	{
		m_IsBlasted = true;
		anim.SetBool("IsHit", true);
		yield return new WaitForSeconds(0.3f);
		col.radius *= 1.5f;
		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject);
	}
}
