using Godot;

namespace StatHub;

/// <summary>
/// Adds a percentage of a stat's value to itself.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatPercentAdditiveModifierStrategy.png")]
public sealed partial class StatPercentAdditiveModifierStrategy : StatModifierStrategy
{
	/// <summary>
	/// The percentage of the input to add (where 100 = 100%; not decimal form).
	/// </summary>
	[Export]
	public float BasePercentChange { get; private set; }


    public override float Modify(ModifierInstance instance, float input)
    {
        return input + (input * (instance.Level * (BasePercentChange / 100)));
    }
}
