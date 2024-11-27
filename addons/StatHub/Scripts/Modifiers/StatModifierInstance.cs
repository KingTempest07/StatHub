using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// DOC
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
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="level"></param>
	public StatModifierInstance(StatModifier modifier, float level = 1) 
	{
		Modifier = modifier;
		m_level = level;
	}


	/// <summary>
	/// Used mainly for GDScript interfacing. In C#, you may be better off using 
	/// the regular constructor.
	/// </summary>
	/// <param name="modifier">The modifier to create an instance of</param>
	/// <returns>The newly created modifier instance</returns>
	public static StatModifierInstance Create(StatModifier modifier)
	{
		return new(modifier);
	}


	public delegate void LevelChanged(float previous, float current);
	public event LevelChanged onLevelChanged;


	public readonly StatModifier Modifier;


	/// <summary>
	/// Effectively acts as the "strength" of the modifier. No functionality is
	/// defined by default that uses it, but it may be used as a variable in 
	/// expression mods and is always used in the linear scaling of simple mods.
	/// </summary>
	/// <value></value>
	public float Level { 
		get => m_level; 
		set {
			float __previous = m_level;
			m_level = value;
			onLevelChanged?.Invoke(__previous, m_level);
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
