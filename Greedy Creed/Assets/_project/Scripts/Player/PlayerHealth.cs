using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public float maxHealth = 50;
	private float m_CurrentHealth;
	public Image healthUI;
	public float damageShakeIntensity;
	public float damageShakeTime;
	private PlayerAudioController m_Audio;

	private void Awake()
	{
		m_CurrentHealth = maxHealth;
		m_Audio = GetComponent<PlayerAudioController>();
	}

	public void Heal(float amount)
	{
		m_CurrentHealth += amount;
		if (m_CurrentHealth > maxHealth)
			m_CurrentHealth = maxHealth;
		UpdateHealthUI();
	}

	public void GetDamaged(float amount)
	{
		m_Audio.PlayAudio(PlayerAudio.hit);
		CameraController.Instance.CameraShake(damageShakeIntensity, damageShakeTime);
		m_CurrentHealth -= amount;
		UpdateHealthUI();
		if(m_CurrentHealth <= 0)
		{
			PlayerController.Instance.GameOver();
		}
	}

	private void UpdateHealthUI()
	{
		healthUI.fillAmount = m_CurrentHealth / maxHealth;
	}
}
