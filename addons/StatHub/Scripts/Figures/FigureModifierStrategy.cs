using Godot;

namespace StatHub;

/// <summary>
/// Defines how a figure modifier modifies a requested change in value.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/FigureModifierStrategy.png")]
public abstract partial class FigureModifierStrategy : Resource
{
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
    public abstract float Modify(ModifierInstance instance, float change, float value, FigureValueEditor editor);
}
