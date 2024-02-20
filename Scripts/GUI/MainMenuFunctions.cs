using Godot;
using System;
using System.Diagnostics;

public partial class MainMenuFunctions : CanvasLayer
{
	[Export] private Camera3D mainCamera;
	[Export] private Node3D camHolder;
	[Export] private AnimationPlayer mainmenuCamAnim;
	[Export] private Node3D mainmenu_Map, game_Map;
	[Export] private AudioStreamPlayer gameMusic;
	[Export] private TextureRect brokenGlassTextureRect;


	[Export] private Camera_Controller camController;
	[Export] private vehiclebody_Controller vehicleBodyController;
	[Export] private Events_Controller eventsController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainmenu_Map.Visible = true;
		game_Map.Visible = false;

		mainmenuCamAnim.Play("Main Menu Loop");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Skip"))
		{
			GetNode<CanvasLayer>(this.GetPath()).Visible = false;

			eventsController.CheckPointSpawn();
			
			mainCamera.Position = Vector3.Zero;
			mainCamera.RotationDegrees = Vector3.Zero;
			
			brokenGlassTextureRect.Visible = false;
		} else if (Input.IsActionJustPressed("Skip2"))
		{
			GetNode<CanvasLayer>(this.GetPath()).Visible = false;

			StartingCutsceneAnimation();
			
			mainCamera.Position = Vector3.Zero;
			mainCamera.RotationDegrees = Vector3.Zero;
			
			brokenGlassTextureRect.Visible = false;
		}
	}

	public void _on_button_button_down()
	{
		mainmenuCamAnim.Stop();
		GetNode<CanvasLayer>(this.GetPath()).Visible = false;
		mainmenuCamAnim.Play("Main Menu To Game Animation");
	}

	public void StartingCutsceneAnimation()
	{
		mainmenu_Map.Visible = false;
		game_Map.Visible = true;
		mainCamera.GetNode<Camera3D>(mainCamera.GetPath()).Fov = 55f;
		mainmenuCamAnim.Play("Game Starting Cutscene Animation");
		gameMusic.Play();
		eventsController.StartTime();
	}

	public void StartGameplay()
	{
		//mainmenuCamAnim.Stop();
		camHolder.GlobalRotationDegrees = Vector3.Zero;
		mainCamera.Fov = 90f;

		camController.isActive = true;
		vehicleBodyController.isActive = true;
		vehicleBodyController.inputEnabled = true;
	}
}
