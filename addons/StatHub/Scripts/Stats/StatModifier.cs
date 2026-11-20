using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// Modifies a stat's value based on a set strategy.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatModifier.png")]
public sealed partial class StatModifier : Modifier
{
	/// <summary>
	/// The strategy used to modify an input value.
	/// </summary>
	[Export]
	public StatModifierStrategy Strategy { get; private set; }


	/// <summary>
	/// The matcher used to determine applicable stats when used as a global 
	/// modifier.
	/// </summary>
	/// <remarks>
	/// If unassigned, this will not match with any stats whatsoever.
	/// </remarks>
	[Export]
	public TagMatcher StatTagMatcher { get; private set; }
    public override TagMatcher GetAttachableTagMatcher() => StatTagMatcher;


	/// <summary>
	/// Modifies the input value.
	/// </summary>
	/// <param name="instance">The instance used to modify the input.</param>
	/// <param name="input">The input value to be modified.</param>
	/// <returns>The newly modified value.</returns>
    public float Modify(ModifierInstance instance, float input)
    {
		if (instance.Modifier != this)
		{
			PushError($"The instance given is not from the same parent modifier! (actual: \"{instance.Modifier.DebugName}\", expected: \"{DebugName}\" This modification will be ignored.");
			return input;
		}

		return Strategy.Modify(instance, input);
	}
}
