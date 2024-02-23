using Godot;
using System;
using System.Diagnostics;

public partial class MainMenuFunctions : CanvasLayer
{
	[Export] private AnimationPlayer mainmenuCamAnim;
	[Export] private Node3D mainmenu_Map, game_Map;
	[Export] private Events_Controller eventsController;
	[Export] private DirectionalLight3D dirLight;


	[Export] private CanvasLayer mainCanvas;
	[Export] private VBoxContainer optionsContainer;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainmenu_Map.Visible = true;
		game_Map.Visible = false;

		mainmenuCamAnim.Play("Main Menu Loop");


		mainCanvas.Visible = true;
		optionsContainer.Visible = false;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void StartingCutsceneAnimation()
	{
		mainmenu_Map.Visible = false;
		game_Map.Visible = true;
		

		eventsController.StartingCutsceneAnimation();
	}

	#region Main Menu
	public void _on_start_button_button_down()
	{
		mainmenuCamAnim.Stop();
		mainCanvas.Visible = false;
		optionsContainer.Visible = false;
		mainmenuCamAnim.Play("Main Menu To Game Animation");
	}

	public void _on_options_button_button_down()
	{
		mainCanvas.Visible = false;
		optionsContainer.Visible = true;
	}

	public void _on_exit_button_button_down()
	{
		GetTree().Quit();
	}

	#endregion


	#region Options
	public void _on_antialiasing_options_btn_item_selected(int index)
	{
		switch (index)
		{
			case 0:
				GetViewport().Msaa3D = Viewport.Msaa.Disabled;
				break;
			case 1:
				GetViewport().Msaa3D = Viewport.Msaa.Msaa2X;
				break;
			case 2:
				GetViewport().Msaa3D = Viewport.Msaa.Msaa4X;
				break;
			case 3:
				GetViewport().Msaa3D = Viewport.Msaa.Msaa8X;
				break;
		}
	}

	public void _on_screen_space_aa_check_btn_toggled(bool state)
	{
		if (state)
			GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Fxaa;
		else
			GetViewport().ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;

	}

	public void _on_use_taa_check_btn_toggled(bool state)
	{
		GetViewport().UseTaa = state;
	}

	public void _on_back_button_down()
	{
		mainCanvas.Visible = true;
		optionsContainer.Visible = false;
	}

	public void _on_shadow_quality_options_btn_item_selected(int index)
	{
		switch (index)
		{
			case 0:
				dirLight.DirectionalShadowMode = DirectionalLight3D.ShadowMode.Orthogonal;
				break;
			case 1:
				dirLight.DirectionalShadowMode = DirectionalLight3D.ShadowMode.Parallel2Splits;
				break;
			case 2:
				dirLight.DirectionalShadowMode = DirectionalLight3D.ShadowMode.Parallel4Splits;
				break;
		}
	}

	#endregion
}
