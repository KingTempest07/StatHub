using Godot;

namespace StatHub;

/// <summary>
/// Adds a percentage of a figure's requested change in value to itself.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/FigurePercentAdditiveModifierStrategy.png")]
public sealed partial class FigurePercentAdditiveModifierStrategy : FigureModifierStrategy
{	
	/// <summary>
	/// The percentage of the change in value to add (where 100 = 100%; not 
	/// decimal form).
	/// </summary>
	[Export]
	public float BasePercentChange { get; private set; }


    public override float Modify(ModifierInstance instance, float change, float value, FigureValueEditor editor)
    {
        return change + (change * (instance.Level * (BasePercentChange / 100)));
    }
}
