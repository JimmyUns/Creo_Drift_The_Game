using Godot;
using System;

public partial class Camera_Controller : Node
{
	public bool isActive;
	public bool smoothLerp = true;
	
	[Export] private Node3D vehicleBody;
	[Export] private Node3D camHolder;
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if(isActive == false) return;
		if(smoothLerp)
			camHolder.GlobalPosition = camHolder.GlobalPosition.Lerp(vehicleBody.GlobalPosition, (float)delta * 7f);
		else
			camHolder.GlobalPosition = vehicleBody.GlobalPosition;
		
	}
	
	public void EnableInput()
	{
		isActive = true;
	}
}
