using Godot;
using System;
using System.Diagnostics;

public partial class Events_Controller : Node3D
{
	public float currentTime;
	private int eventsIndex, lastEventsIndex;
	[Export] private Node3D playerBody; //For Debugging

	//Controllers
	[Export] private vehiclebody_Controller vhController;
	[Export] private Camera_Controller camController;
	//Animation Players
	[Export] private AnimationPlayer eventAnim;
	[Export] public AnimationPlayer uiAnim;
	//Nodes
	[Export] private Camera3D mainCamera;
	[Export] private Node3D camHolder;
	[Export] private AudioStreamPlayer[] gameMusic;
	[Export] private TextureRect brokenGlassTextureRect;

	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Death"))
		{
			StopGame();
		}

		if (Input.IsActionJustPressed("Skip"))
		{
			CheckPointSpawn();

			mainCamera.Position = Vector3.Zero;
			mainCamera.RotationDegrees = Vector3.Zero;

			brokenGlassTextureRect.Visible = false;
		}
		else if (Input.IsActionJustPressed("Skip2"))
		{
			StartingCutsceneAnimation();

			mainCamera.Position = Vector3.Zero;
			mainCamera.RotationDegrees = Vector3.Zero;

			brokenGlassTextureRect.Visible = false;
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
				playerBody.GlobalPosition = new Vector3(0f, -5.7153025f, -473f);
				playerBody.Visible = true;
				currentTime = 12.1f;
				eventsIndex = 3;

				vhController.isActive = true;
				vhController.inputEnabled = false;
				camController.isActive = false;
				eventAnim.Play("Billboard_Event");

				gameMusic[1].Play();
				break;
		}
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
		foreach (AudioStreamPlayer gameM in gameMusic)
		{
			gameM.Stop();
		}
		eventAnim.Play("Player_Death");
	}

	public void StartingCutsceneAnimation()
	{
		mainCamera.GetNode<Camera3D>(mainCamera.GetPath()).Fov = 55f;
		eventAnim.Play("Game Starting Cutscene Animation");
		gameMusic[0].Play();
		eventsIndex = 1;
	}

	public void StartGameplay()
	{
		camHolder.GlobalRotationDegrees = Vector3.Zero;
		mainCamera.Fov = 90f;

		camController.isActive = true;
		vhController.isActive = true;
		vhController.inputEnabled = true;
	}
}
