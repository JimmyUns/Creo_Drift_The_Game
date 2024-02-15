using Godot;
using System;
using System.Diagnostics;


public partial class World_Manager : Node3D
{
	private bool isFullscreen;

	public override void _Ready()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Change_ScreenMode"))
		{
			isFullscreen = !isFullscreen;
			if (isFullscreen)
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			else
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
	}
}
