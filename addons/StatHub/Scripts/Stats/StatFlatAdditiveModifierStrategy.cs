using Godot;

namespace StatHub;

/// <summary>
/// Adds a flat value to the input.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatFlatAdditiveModifierStrategy.png")]
public sealed partial class StatFlatAdditiveModifierStrategy : StatModifierStrategy
{
	/// <summary>
	/// The flat value to add to the input.
	/// </summary>
	[Export]
	public float BaseAddend { get; private set; }


    public override float Modify(ModifierInstance instance, float input)
    {
        return input + (BaseAddend * instance.Level);
    }
}
