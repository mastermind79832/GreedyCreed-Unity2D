using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance { get; private set; }
	
    public float MoveSpeed = 2f;
	public AudioSource m_Source;
	public AudioClip[] moveAudio;

    private Rigidbody2D m_Rigidbody;
    private CircleCollider2D m_Collider;
	private Vector2 m_Input;
	private Vector2 m_FacingDirection;
	private DashControl m_Dash;
	private PlayerHealth m_health;
	private bool m_IsAudioPlaying;

	private void Awake()
	{
		Instance = this;
		Caching();
	}

	private void Caching()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
		m_Collider = GetComponent<CircleCollider2D>();
		m_Dash = GetComponent<DashControl>();
		m_health = GetComponent<PlayerHealth>();
		m_Source = GetComponent<AudioSource>();
	}

	private void Update()
	{
		GetInput();

	}
	private void FixedUpdate()
	{
		if (m_Dash.IsDashing())
			return;
		MovePositon();
		RotateToMouse();
	}
	
	private void GetInput()
	{
		m_Input.x = Input.GetAxis("Horizontal");
		m_Input.y = Input.GetAxis("Vertical");
		if (m_Input != Vector2.zero && !m_IsAudioPlaying)
			StartCoroutine(WaitForAudio(GetRandomAudio(moveAudio)));
	}

	IEnumerator WaitForAudio(AudioClip clip)
	{
		m_IsAudioPlaying = true;
		m_Source.clip = clip;
		m_Source.Play();
		float wait = clip.length;
		yield return new WaitForSeconds(wait);
		m_IsAudioPlaying = false;
	}

	private AudioClip GetRandomAudio(AudioClip[] clips)
	{
		return clips[Random.Range(0, clips.Length - 1)];
	}

	public Vector2 GetMoveDirection()
	{
		return m_Input.normalized;
	}

	public Vector2 GetLocation()
	{
		return transform.position;
	}
	
	private void MovePositon()
	{
		m_Rigidbody.velocity = m_Input.normalized * MoveSpeed;
	}

	public void AddVelocity(Vector2 dir ,float velocity)
	{
		m_Rigidbody.velocity += dir * velocity;
	}

	private void RotateToMouse()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0f;
		m_FacingDirection = (mousePosition - transform.position).normalized;
		transform.up = m_FacingDirection;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (m_Dash.IsDashing())
			return;
	
		BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
		if (bullet != null)
		{
			m_health.GetDamaged(bullet.bulletPower);
			bullet.BulletHit();
		}
	}
}
