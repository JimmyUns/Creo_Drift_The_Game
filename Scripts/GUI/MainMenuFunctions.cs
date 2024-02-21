using Godot;
using System;
using System.Diagnostics;

public partial class MainMenuFunctions : CanvasLayer
{
	[Export] private AnimationPlayer mainmenuCamAnim;
	[Export] private Node3D mainmenu_Map, game_Map;
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
		
		eventsController.StartingCutsceneAnimation();
	}
}
