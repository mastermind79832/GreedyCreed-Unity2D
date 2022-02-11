using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerContoller : MonoBehaviour
{
    //public string Username = "Creed";
	//public int healthCount = 3;
	//public float piggyBank = 10;
	[Header("Movement")]
    public float MoveSpeed = 2f;

	[Header("Dash")]
	public KeyCode dashKey;
	public float dashSpeed = 5f;
	public float dashTime = 0.2f;
	public float shakeTime = 0.2f;
	public float shakeIntensity = 1.5f;
	public ParticleSystem dashEffect;

    private Rigidbody2D m_Rigidbody;
    private CircleCollider2D m_Collider;
	private Vector2 m_Input;
	private Vector2 m_FacingDirection;
	private bool m_IsDashing;

	private void Awake()
	{
		Caching();
		Initialization();
	}

	private void Initialization()
	{
		m_IsDashing = false;
	}

	private void Caching()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Collider = GetComponent<CircleCollider2D>();
	}

	private void Update()
	{
		GetInput();
	}
	private void FixedUpdate()
	{
		if (m_IsDashing)
			return;
		MovePositon();
		RotateToMouse();
	}

	private void GetInput()
	{
		if (m_IsDashing)
			return;
		m_Input.x = Input.GetAxis("Horizontal");
		m_Input.y = Input.GetAxis("Vertical");

		if (Input.GetKeyDown(dashKey) && m_Input != Vector2.zero)
			Dash();
	}

	#region	Dashing
	private void Dash()
	{
		m_IsDashing = true;
		CameraController.s_instance.CameraShake(shakeIntensity, shakeTime);
		StartCoroutine(Dashing());
	}

	IEnumerator Dashing()
	{
		m_Rigidbody.velocity += m_Input.normalized * dashSpeed;
		StartCoroutine(DashEffect());
		yield return new WaitForSeconds(dashTime/10);
		m_IsDashing = false;
	}

	IEnumerator DashEffect()
	{
		dashEffect.transform.position = transform.position;
		ParticleSystem.EmissionModule emission = dashEffect.emission;
		emission.enabled = true;
		yield return new WaitForSeconds(dashEffect.main.duration);
		emission.enabled = false;
	}
	# endregion

	private void MovePositon()
	{
		m_Rigidbody.velocity = m_Input.normalized * MoveSpeed;
	}

	private void RotateToMouse()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0f;
		m_FacingDirection = (mousePosition - transform.position).normalized;
		transform.up = m_FacingDirection;
	}

}
