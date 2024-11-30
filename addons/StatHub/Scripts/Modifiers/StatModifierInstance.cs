using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// An instance is an attachable representative for a modifier.
/// </summary>
[GlobalClass]
public sealed partial class StatModifierInstance : RefCounted
{
	/// <summary>
	/// A comparer used to determine the ordering of modifier applications
	/// </summary>
	public class PriorityComparer : Comparer<StatModifierInstance>
	{
        public override int Compare(StatModifierInstance modifier, StatModifierInstance otherModifier)
        {
            if (modifier.Modifier.Priority > otherModifier.Modifier.Priority)
			{
				return -1;
			}
			if (modifier.Modifier.Priority < otherModifier.Modifier.Priority)
			{
				return 1;
			}
			return 0;
        }
    }


	/// <summary>
	/// Creates a new instance of the input <c>modifier</c> with the input 
	/// <c>level</c>.
	/// </summary>
	/// <param name="modifier">The modifier to make an instance of</param>
	/// <param name="level">The inital level to set the instance to</param>
	public StatModifierInstance(StatModifier modifier, float level = 1) 
	{
		Modifier = modifier;
		m_level = level;
	}


	/// <summary>
	/// Creates a new instance of the input modifier.
	/// </summary>
	/// <remarks>
	/// This is used mainly for GDScript interfacing. In C#, you may be better 
	/// off using the regular constructor.
	/// </remarks>
	/// <param name="modifier">The modifier to create an instance of</param>
	/// <returns>The newly created modifier instance</returns>
	public static StatModifierInstance Create(StatModifier modifier)
	{
		return new(modifier);
	}


	/// <summary>
	/// Emitted when the level of this instance is changed.
	/// </summary>
	/// <param name="previous">The previous level</param>
	/// <param name="current">The current level</param>
	[Signal]
	public delegate void LevelChangedEventHandler(float previous, float current);


	/// <summary>
	/// The parent modifier of this instance.
	/// </summary>
	public readonly StatModifier Modifier;


	/// <summary>
	/// Effectively acts as the "strength" of the modifier. No functionality is
	/// defined by default that uses it, but it may be used as a variable in 
	/// expression mods and is always used in the linear scaling of simple mods.
	/// </summary>
	public float Level { 
		get => m_level; 
		set {
			float __previous = m_level;

			m_level = value;

			EmitSignal(SignalName.LevelChanged, __previous, m_level);
		}
	}
	private float m_level;


	/// <summary>
	/// Modifies the input value based on this instance and its parent modifier.
	/// </summary>
	/// <remarks>
	/// This is shorthand and goes through the parent modifier of this instance.
	/// </remarks>
	/// <param name="input">The value to be modified.</param>
	/// <returns>The newly modified value</returns>
	public float Modify(float input)
	{
		return Modifier.Modify(this, input);
	}
}
