using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleButtonController : MonoBehaviour
{

	[SerializeField]
	KeyCode keyCode = KeyCode.Space;

	[SerializeField]
	float tapTiming = 0.2f;

	public delegate void InputEvent();

	public event InputEvent OnHoldDown;
	public event InputEvent OnHoldUp;
	public event InputEvent OnTap;

	private bool buttonDown = false;
	private bool tapOnUp = false;
	private float tapTimer = 0f;


	void Start()
	{
		if (this.OnHoldUp != null) this.OnHoldUp();
	}

	void Update()
	{
		// Handle tap/hold timer
		if (tapOnUp == true)
		{
			tapTimer += Time.deltaTime;
			if (tapTimer >= tapTiming)
			{
				tapOnUp = false;    // Cannot tap anymore, becomes a HoldUp now!
				if (this.OnHoldDown != null) this.OnHoldDown();
			}
		}
		

		// Down logic
		if (Input.GetKeyDown(keyCode))
		{
			if (buttonDown == false)
			{
				tapTimer = 0f;
				buttonDown = true;
				tapOnUp = true;
			}
		}

		// Up logic
		if (Input.GetKeyUp(keyCode))
		{
			if (buttonDown)
			{
				buttonDown = false;
				if (tapOnUp)
				{
					if (this.OnTap != null) this.OnTap();
				}
				else
				{
					if (this.OnHoldUp != null) this.OnHoldUp();
				}
				tapOnUp = false;
			}
		}
	}

}
