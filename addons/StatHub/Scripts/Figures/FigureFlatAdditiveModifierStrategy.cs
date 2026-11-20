using Godot;

namespace StatHub;

/// <summary>
/// Adds a flat value to a figure's requested change in value.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/FigureFlatAdditiveModifierStrategy.png")]
public sealed partial class FigureFlatAdditiveModifierStrategy : FigureModifierStrategy
{	
	/// <summary>
	/// The flat value to add to the requested change in value.
	/// </summary>
	[Export]
	public float BaseAddend { get; private set; }


    public override float Modify(ModifierInstance instance, float change, float value, FigureValueEditor editor)
    {
        return change + (BaseAddend * instance.Level);
    }
}
