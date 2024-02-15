using Godot;
using System;

public partial class Camera_Controller : Node
{
	public bool isActive;
	
	[Export] private Node3D vehicleBody;
	[Export] private Node3D camHolder;
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if(isActive == false) return;
		camHolder.GlobalPosition = camHolder.GlobalPosition.Lerp(vehicleBody.GlobalPosition, (float)delta * 7f);
		
	}
	
	public void EnableInput()
	{
		isActive = true;
	}
}
