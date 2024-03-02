using Godot;
using System;

namespace UI.SFX
{
	public partial class UI_SFX_Manager : Node
	{
		[Export] public AudioStreamPlayer click_SFX;
		[Export] public AudioStreamPlayer hover_SFX;
		[Export] public AudioStreamPlayer switchOn_SFX;

		public void _on_button_down()
		{
			click_SFX.Play();
		}
		
		public void _on_button_mouse_entered()
		{
			hover_SFX.Play();
		}
		
		public void _on_item_selected(bool isOn)
		{
			switchOn_SFX.Play();
		}
	}
}
