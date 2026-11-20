using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// Modifies a figure's requested change in value based on a set strategy.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/FigureModifier.png")]
public sealed partial class FigureModifier : Modifier
{
    /// <summary>
	/// The strategy used to modify a figure's requested change in value.
	/// </summary>
	[Export]
	public FigureModifierStrategy Strategy { get; private set; }	


	/// <summary>
	/// The matcher used to determine applicable figures when used as a global 
	/// modifier.
	/// </summary>
	/// <remarks>
	/// If unassigned, this will not match with any figures whatsoever.
	/// </remarks>
	[Export]
	public TagMatcher FigureTagMatcher { get; private set; }
    public override TagMatcher GetAttachableTagMatcher() => FigureTagMatcher;


    /// <summary>
    /// Modifies the requested change in a figure's value.
    /// </summary>
    /// <param name="instance">The instance used to modify the change.</param>
    /// <param name="change">
    /// The value to be modified; the requested change in value.
    /// </param>
    /// <param name="value">The figure's value before modification.</param>
    /// <param name="editor">The editor requesting the value change.</param>
    /// <returns>The newly modified requested change in value.</returns>
    public float Modify(ModifierInstance instance, float change, float value, FigureValueEditor editor)
    {
		if (instance.Modifier != this)
		{
			PushError($"The instance given is not from the same parent modifier! (actual: \"{instance.Modifier.DebugName}\", expected: \"{DebugName}\" This modification will be ignored.");
			return change;
		}

		return Strategy.Modify(instance, change, value, editor);
	}
}
