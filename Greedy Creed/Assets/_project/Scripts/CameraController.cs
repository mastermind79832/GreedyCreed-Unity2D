using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
	public static CameraController s_instance { get; private set; } 

    private CinemachineVirtualCamera m_LiveCam;
	private CinemachineBasicMultiChannelPerlin m_Perlin;

	private void Awake()
	{
		Initialize();
	}

	private void Initialize()
	{
		if (s_instance != null)
			s_instance = null;
		s_instance = this;
		m_LiveCam = GetComponent<CinemachineVirtualCamera>();
		m_Perlin = m_LiveCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	public void CameraShake(float intensity, float time)
	{
		m_Perlin.m_AmplitudeGain = intensity;
		StartCoroutine(Shaking(time));
	}

	IEnumerator Shaking(float time)
	{	
		yield return new WaitForSeconds(time);
		m_Perlin.m_AmplitudeGain = 0f;
	}
}
