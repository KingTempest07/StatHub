using System;
using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// A modifier that has two basic additive modification methods.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/SimpleModifier.png")]
public partial class SimpleModifier : StatModifier
{
	/// <summary>
	/// Defines methods for applying the base modification amount to an input.
	/// </summary>
    public enum ModificationOptions
	{
		/// <summary>
		/// Simply adds the amount to the input.
		/// </summary>
		FLAT_ADDITIVE = 0,
		/// <summary>
		/// Adds a percentage of the input to itself.
		/// </summary>
		/// <remarks>
		/// Note that the amount is read where "100" = 100%, "50" = 50%, etc., 
		/// as opposed to other systems that use "1" = 100% and "0.5" = 50%.
		/// </remarks> 
		PERCENT_ADDITIVE = 1,
	}
	/// <summary>
	/// Defines how the base modification amount is applied to an input.
	/// </summary>
	[Export]
	public ModificationOptions ModificationOption { get; private set; }


	/// <summary>
	/// The base amount used to modify stats, depending on the selected 
	/// <c>ModificationOption</c>.
	/// </summary>
	/// <remarks>
	/// This is multiplied by the level for the final value, giving linear level 
	/// scaling.
	/// </remarks>
	/// <value></value>
	[Export]
	public float BaseModificationAmount { get; private set; }


    public override float Modify(StatModifierInstance instance, float input)
    {
		if (instance.Modifier != this)
		{
			PushError($"The instance given is not from the same parent modifier! (actual: \"{instance.Modifier.DebugName}\", expected: \"{DebugName}\" This modification will be ignored.");
			return input;
		}

        return ModificationOption switch 
		{
			ModificationOptions.FLAT_ADDITIVE 
				=> input + (BaseModificationAmount * instance.Level),
			ModificationOptions.PERCENT_ADDITIVE
				=> input + (input * (BaseModificationAmount / 100) * instance.Level),
			_
				=> throw new NotImplementedException()
		};
    }
}
