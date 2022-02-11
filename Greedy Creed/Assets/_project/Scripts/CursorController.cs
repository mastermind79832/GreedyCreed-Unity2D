using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	public bool CursorVisible;
	//private SpriteRenderer m_Cursor;

	//private void Awake()
	//{
	//	m_Cursor = GetComponent<SpriteRenderer>();
	//}

	private void Start()
	{
		if (!CursorVisible)
			Cursor.visible = false;
		else
			gameObject.SetActive(false);
	}

	private void FixedUpdate()
	{
		transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
