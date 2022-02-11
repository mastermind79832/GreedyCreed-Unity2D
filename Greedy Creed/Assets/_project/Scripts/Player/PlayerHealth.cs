using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public float maxHealth = 50;
	private float m_CurrentHealth;
	public Image healthUI;

	private void Awake()
	{
		m_CurrentHealth = maxHealth;
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
		m_CurrentHealth -= amount;
		UpdateHealthUI();
	}

	private void UpdateHealthUI()
	{
		healthUI.fillAmount = m_CurrentHealth / maxHealth;
	}
}
