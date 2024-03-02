using Godot;
using System;
using System.Diagnostics;

public partial class vehiclebody_Controller : Node
{
	public bool isActive;
	public bool inputEnabled = true;
	
	private float maxXGlobalPosition = 10f;

	[Export] public float steeringStrength = 5f;
	[Export] public float moveSpeed = 50f;

	[Export] private RayCast3D groundcheckRay;
	[Export] public CharacterBody3D vBody;
	[Export] public Node3D vBody_Model;
	[Export] public Particles_Controller particlesController;
	[Export] public AnimationPlayer carAnim;

	private float currentSteeringSpeed;
	private float _delta;
	private Vector3 velocity;
	private float gravity = 20f, currentGravity = 0f;

	public override void _Ready()
	{
		vBody.RotationDegrees = Vector3.Zero;
		carAnim.Play("Rotate_Wheels");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isActive == false) return;

		velocity = vBody.Velocity;
		_delta = (float)delta;

		Driving();


		velocity.Z = -moveSpeed;
		vBody.Velocity = velocity;
		vBody.MoveAndSlide();
	}

	private void Driving()
	{
		if (vBody.IsOnFloor() == false) //Not Grounded
		{
			currentGravity = Mathf.Lerp(currentGravity, gravity, _delta * 5f);
			velocity.Y -= currentGravity * _delta;
			vBody_Model.RotationDegrees = new Vector3(Mathf.Lerp(vBody_Model.RotationDegrees.X, -38f, _delta * 1.2f), vBody_Model.RotationDegrees.Y, vBody_Model.RotationDegrees.Z);
			particlesController.Emit_Desert_Dust(false);
		}
		else //Grounded
		{
			currentGravity = 0f;
			var angle = Mathf.RadToDeg(Mathf.Acos(groundcheckRay.GetCollisionNormal().Dot(Vector3.Up)));
			vBody_Model.RotationDegrees = new Vector3(Mathf.Lerp(vBody_Model.RotationDegrees.X, angle, _delta * 5f), vBody.RotationDegrees.Y, vBody.RotationDegrees.Z);
			particlesController.Emit_Desert_Dust(true);
		}

		if (inputEnabled == false)
		{
			velocity.X = 0f;
			vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Lerp(vBody.RotationDegrees.Y, 0f, _delta * steeringStrength * 6f), vBody.RotationDegrees.Z);
			return;
		}
		int inputDirX = (int)Input.GetAxis("Turn_Right", "Turn_Left");

		if (inputDirX != 0)
		{
			if (vBody.GlobalPosition.X < maxXGlobalPosition && vBody.GlobalPosition.X > -maxXGlobalPosition) //Arent Wall Colliding
			{
				currentSteeringSpeed = Mathf.Clamp(Mathf.Lerp(currentSteeringSpeed, inputDirX, _delta * 10f), -1, 1);
				vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Clamp(vBody.RotationDegrees.Y + (currentSteeringSpeed * steeringStrength), -15f, 15f), vBody.RotationDegrees.Z);
				velocity.X = -vBody.RotationDegrees.Y * steeringStrength;
			}
			else //Is Wall Colliding
			{
				vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Lerp(vBody.RotationDegrees.Y, 0f, _delta * steeringStrength * 6f), vBody.RotationDegrees.Z);
				if ((inputDirX > 0 && vBody.GlobalPosition.X < 0f) || (inputDirX < 0 && vBody.GlobalPosition.X > 0f))
				{
					velocity.X = 0f;
				}
				else
				{
					currentSteeringSpeed = Mathf.Clamp(Mathf.Lerp(currentSteeringSpeed, inputDirX, _delta * 10f), -1, 1);
					vBody.RotationDegrees = new Vector3(vBody.RotationDegrees.X, Mathf.Clamp(vBody.RotationDegrees.Y + (currentSteeringSpeed * steeringStrength), -15f, 15f), vBody.RotationDegrees.Z);
					velocity.X = -vBody.RotationDegrees.Y * steeringStrength;
				}

			}
		}
		else
		{
			if (vBody.GlobalPosition.X < -maxXGlobalPosition || vBody.GlobalPosition.X > maxXGlobalPosition)
			{
				velocity.X = 0f;
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
