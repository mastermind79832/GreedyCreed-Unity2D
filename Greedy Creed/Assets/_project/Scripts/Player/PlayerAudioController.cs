using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerAudio
{
	dash,
	hit,
	denial
}

public class PlayerAudioController : MonoBehaviour
{
	public AudioClip[] dash;
	public AudioClip hit;
	public AudioClip denial;
	public AudioSource source;

	private PlayerAudio m_CurrentAudio;
	private bool m_IsAudioPlaying;

	public void PlayAudio(PlayerAudio sound, bool isLoop = false)
	{
		if (m_IsAudioPlaying && m_CurrentAudio == sound)
			return;

		switch(sound)
		{
			case PlayerAudio.dash:
				source.clip = GetRandomAudio(dash);
				break;
			case PlayerAudio.hit:
				source.clip = hit;
				break;
			case PlayerAudio.denial:
				source.clip = denial;
				break;
		}
		m_CurrentAudio = sound;
		if (source.clip == null)
			return;
		source.loop = isLoop;
		source.Play();
		StartCoroutine(WaitForAudio());
	}

	IEnumerator WaitForAudio()
	{
		m_IsAudioPlaying = true;
		float wait = source.clip.length;
		yield return new WaitForSeconds(wait);
		m_IsAudioPlaying = false;
	}

	private AudioClip GetRandomAudio(AudioClip[] clips)
	{
		return clips[Random.Range(0, clips.Length-1)];
	}
}
