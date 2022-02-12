using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashControl : MonoBehaviour
{
	public KeyCode dashKey;
	public float dashSpeed = 5f;
	[Tooltip("effective time = dashTime/10")]
	public float dashTime = 1;
	public float dashCost = 10;
	public float dashCharge = 30;
	public float ChargeRate = 1;
	public float shakeTime = 0.2f;
	public float shakeIntensity = 1.5f;
	public ParticleSystem dashEffect;
	public Image dashUI;

	private bool m_IsDashing;
	private float m_CurrentDashCharge;
	private PlayerController player;
	private PlayerAudioController m_Audio;

	private void Awake()
	{
		m_IsDashing = false;
		m_CurrentDashCharge = dashCharge;
		player = GetComponent<PlayerController>();
		m_Audio = GetComponent<PlayerAudioController>();
	}

	private void Update()
	{
		IncreaseDashCharge();
		GetInput();
	}

	private void GetInput()
	{
		if (Input.GetKeyDown(dashKey))
		{
			if (CanDash())
			{
				Dash();
			}
			else
				m_Audio.PlayAudio(PlayerAudio.denial);
		}
	}

	private void IncreaseDashCharge()
	{
		if (m_CurrentDashCharge < dashCharge)
		{
			m_CurrentDashCharge += ChargeRate * Time.deltaTime;
			updateDashUI();
		}
		else if (m_CurrentDashCharge > dashCharge)
		{
			m_CurrentDashCharge = dashCharge;
		}
	}

	private void updateDashUI()
	{
		dashUI.fillAmount = (m_CurrentDashCharge / dashCharge);
	}

	private void Dash()
	{
		m_Audio.PlayAudio(PlayerAudio.dash);
		m_IsDashing = true;
		CameraController.Instance.CameraShake(shakeIntensity, shakeTime);
		StartCoroutine(Dashing());
	}

	private bool CanDash()
	{
		if(m_CurrentDashCharge >= dashCost)
		{
			m_CurrentDashCharge -= dashCost;
			updateDashUI();
		}
		else
			return false;
		return true;
	}
	 
	public bool IsDashing()
	{
		return m_IsDashing;
	}

	IEnumerator Dashing()
	{
		player.AddVelocity(player.GetMoveDirection(), dashSpeed);
		StartCoroutine(DashEffect());
		yield return new WaitForSeconds(dashTime / 10);
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
}
