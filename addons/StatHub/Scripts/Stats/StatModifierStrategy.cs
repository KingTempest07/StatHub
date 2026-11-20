using Godot;

namespace StatHub;

/// <summary>
/// Defines how a stat modifier modifies an input value.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatModifierStrategy.png")]
public abstract partial class StatModifierStrategy : Resource
{
	/// <summary>
	/// Modifies the input value.
	/// </summary>
	/// <param name="instance">The instance used to modify the input.</param>
	/// <param name="input">The input value to be modified.</param>
	/// <returns>The newly modified value.</returns>
    public abstract float Modify(ModifierInstance instance, float input);
}
