using Godot;
using System;
using System.Diagnostics;

public partial class vehiclebody_Controller : Node
{
	public bool isActive;
	public bool inputEnabled = true;

	[Export] public float steeringStrength = 5f;
	[Export] public float moveSpeed = 50f;

	[Export] private RayCast3D groundcheckRay;
	[Export] private RayCast3D wallcheckkRay1, wallcheckkRay2;
	[Export] public RigidBody3D vBody;
	[Export] public Node3D vBody_Model;
	[Export] public Particles_Controller particlesController;

	private float currentSteeringSpeed;
	private float _delta;
	private Vector3 velocity;
	private float gravity = 20f, currentGravity = 0f;

	public override void _PhysicsProcess(double delta)
	{
		if (isActive == false) return;

		velocity = vBody.LinearVelocity;
		_delta = (float)delta;

		Driving();


		velocity.Z = -moveSpeed;
		vBody.LinearVelocity = velocity;
	}

	private void Driving()
	{
		if (groundcheckRay.IsColliding() == false) //Not Grounded
		{
			currentGravity = Mathf.Lerp(currentGravity, gravity, _delta * 5f);
			velocity.Y -= currentGravity * _delta;
			vBody_Model.RotationDegrees = new Vector3(Mathf.Lerp(vBody_Model.RotationDegrees.X, -38f, _delta * 1.2f), vBody.RotationDegrees.Y, vBody.RotationDegrees.Z);
			particlesController.Emit_Desert_Dust(false);
		}
		else //Grounded
		{
			currentGravity = 0f;
			var angle = Mathf.RadToDeg(Mathf.Acos(groundcheckRay.GetCollisionNormal().Dot(Vector3.Up)));
			vBody_Model.RotationDegrees = new Vector3(angle, vBody.RotationDegrees.Y, vBody.RotationDegrees.Z);
			particlesController.Emit_Desert_Dust(true);
		}

		if (inputEnabled == false)
		{
			velocity.X = 0f;
			vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Lerp(vBody.RotationDegrees.Y, 0f, _delta * steeringStrength * 6f), vBody.RotationDegrees.Z);
			return;
		}
		int inputDirX = (int)Input.GetAxis("Turn_Right", "Turn_Left");

		RayCast3D wallCheckRay = inputDirX > 0 ? wallcheckkRay1 : wallcheckkRay2;
		bool areTheyColliding = wallcheckkRay1.IsColliding() || wallcheckkRay2.IsColliding();
		if (inputDirX != 0)
		{
			if (!wallCheckRay.IsColliding())
			{
				currentSteeringSpeed = Mathf.Clamp(Mathf.Lerp(currentSteeringSpeed, inputDirX, _delta * 10f), -1, 1);
				vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Clamp(vBody.RotationDegrees.Y + (currentSteeringSpeed * steeringStrength), -15f, 15f), vBody.RotationDegrees.Z);
				velocity.X = -vBody.RotationDegrees.Y * steeringStrength;
			}
			else
			{
				vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Lerp(vBody.RotationDegrees.Y, 0f, _delta * steeringStrength * 6f), vBody.RotationDegrees.Z);
			}
		}
		else
		{
			if (areTheyColliding)
			{
				vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Lerp(vBody.RotationDegrees.Y, 0f, _delta * steeringStrength * 6f), vBody.RotationDegrees.Z);
			}
			else
			{
				currentSteeringSpeed = 0f;
				vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Lerp(vBody.RotationDegrees.Y, 0f, _delta * steeringStrength * 2f), vBody.RotationDegrees.Z);
				velocity.X = -vBody.RotationDegrees.Y * steeringStrength;
				if (vBody.RotationDegrees.Y < 1f && vBody.RotationDegrees.Y > -1f)
				{
					velocity.X = 0f;
				}
			}
		}


	}

	public void EnableInput()
	{
		inputEnabled = true;
	}
}
