using Godot;
using System;
using System.Diagnostics;

public partial class Events_Controller : Node3D
{
	public float currentTime;
	private int eventsIndex;
	private int lastEventsIndex;

	[Export] private Node3D playerPos; //For Debugging
	[Export] private vehiclebody_Controller vhController;
	[Export] private Camera_Controller camController;
	[Export] private AnimationPlayer eventAnim;
	[Export] public AnimationPlayer uiAnim;
	[Export] private AudioStreamPlayer gameMusic;
	[Export] private Camera3D mainCamera;

	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Death"))
		{
			StopGame();
		}

		if (eventsIndex == 0) return;
		currentTime += (float)delta;
		TimeEvents();
	}

	private void TimeEvents()
	{
		switch (eventsIndex)
		{
			case 1: //Play A D ui Animaton
				if (currentTime > 6f)
				{
					uiAnim.Play("AD_Animation");

					eventsIndex++;
				}
				break;

			case 2: //Play the billboard Animation
				if (currentTime > 12.1f)
				{
					vhController.inputEnabled = false;
					camController.isActive = false;
					eventAnim.Play("Billboard_Event");

					eventsIndex++;
				}
				break;

			case 3: //Play the Selfies animation 1 (The animation that shows the car from different angles)
				if (currentTime > 25.576)
				{
					vhController.inputEnabled = false;
					eventAnim.Play("Selfies_Animation_1");
					mainCamera.Fov = 55f;


					eventsIndex++;
				}
				break;
		}
	}

	public void CheckPointSpawn()
	{
		switch (lastEventsIndex)
		{
			case 2: //Billboard Animation
				playerPos.GlobalPosition = new Vector3(0f, -5.7153025f, -473f);
				playerPos.Visible = true;
				currentTime = 12.1f;
				eventsIndex = 3;

				vhController.isActive = true;
				vhController.inputEnabled = false;
				camController.isActive = false;
				eventAnim.Play("Billboard_Event");
				break;
		}
	}

	public void StartTime()
	{
		eventsIndex = 1;
	}

	public void BBEvent2()
	{
		eventAnim.Play("Billboard_Event_2");

	}

	private void _on_area_checker_body_entered(Node3D body)
	{
		StopGame();
	}

	public void StopGame()
	{
		lastEventsIndex = 2;
		eventsIndex = 0;
		currentTime = 0f;
		vhController.isActive = false;
		vhController.vBody.LinearVelocity = Vector3.Zero;
		gameMusic.Stop();
		eventAnim.Play("Player_Death");
	}
}
