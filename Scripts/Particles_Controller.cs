using Godot;
using System;

public partial class Particles_Controller : Node
{
	[Export] private GpuParticles3D[] desertDustParticles;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
	
	public void Emit_Desert_Dust(bool  value)
	{
		foreach (GpuParticles3D particle in desertDustParticles)
		{
			particle.Emitting = value;
		}
	}
}
