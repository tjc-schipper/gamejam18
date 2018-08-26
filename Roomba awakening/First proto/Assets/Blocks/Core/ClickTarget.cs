using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTarget : MonoBehaviour
{

	public delegate void MouseEvent(Vector3 mousePos, GameObject target);
	public event MouseEvent OnDragStart;
	public event MouseEvent OnDragEnd;
	public event MouseEvent OnClick;

	const float CLICK_TIMING = 0.1f;

	bool inClickTiming = false;
	float clickTimer = 0f;


	void Update()
	{
		if (inClickTiming)
		{
			clickTimer += Time.deltaTime;
			if (clickTimer > CLICK_TIMING)
			{
				inClickTiming = false;
				if (this.OnDragStart != null) this.OnDragStart(Input.mousePosition, this.gameObject);
			}
		}
	}

	void OnMouseDown()
	{
		this.clickTimer = 0f;
		this.inClickTiming = true;
	}

	void OnMouseUp()
	{
		if (inClickTiming)
		{
			if (this.OnClick != null) this.OnClick(Input.mousePosition, this.gameObject);
			this.inClickTiming = false;
		}
		else
		{
			if (this.OnDragEnd != null) this.OnDragEnd(Input.mousePosition, this.gameObject);
		}
	}

}