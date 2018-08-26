using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SingleButtonController), typeof(Rigidbody))]
public class TestSingleButton : MonoBehaviour
{

	[SerializeField]
	float forwardSpeed = 1f;

	[SerializeField]
	float backwardSpeed = 1f;

	[SerializeField]
	float backwardRotation = 1f;

	[SerializeField]
	float jumpForce = 1f;

	private Rigidbody rb;
	private SingleButtonController controller;
	private bool forward = true;
	private bool grounded = true;
	
	void Awake()
	{
		this.rb = GetComponent<Rigidbody>();

		this.controller = GetComponent<SingleButtonController>();
		this.controller.OnTap += Controller_OnTap;
		this.controller.OnHoldDown += Controller_OnHoldDown;
		this.controller.OnHoldUp += Controller_OnHoldUp;
	}


	void Update()
	{
		if (grounded)
		{
			if (forward)
			{
				// move forwards
				Vector3 velocity = this.transform.forward * forwardSpeed * Time.deltaTime;
				this.rb.MovePosition(this.transform.position + velocity);
			}
			else
			{
				// rotate backwards
				// move forwards
				Vector3 velocity = -this.transform.forward * backwardSpeed * Time.deltaTime;
				this.rb.MovePosition(this.transform.position + velocity);
				Quaternion rotation = Quaternion.AngleAxis(backwardRotation * Time.deltaTime, Vector3.Lerp(Vector3.up, this.transform.up, 0.5f));
				this.rb.MoveRotation(this.transform.rotation * rotation);
			}
		}
		else
		{
			
		}
	}


	private void Jump()
	{
		this.grounded = false;
		this.rb.AddForce(Vector3.up * jumpForce);
	}


	private void Controller_OnHoldUp()
	{
		this.forward = true;
	}

	private void Controller_OnHoldDown()
	{
		this.forward = false;
	}

	private void Controller_OnTap()
	{
		if (grounded)
			Jump();
	}


	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("ground"))
		{
			this.grounded = true;
		}
	}
}
