using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

namespace StatHub;

public abstract partial class Stat
{
	/// <summary>
	/// Invoked when this stat’s list of modifiers is changed.
	/// </summary>
	[Signal]
	public delegate void ModifiersChangedEventHandler();


	/// <summary>
	/// A collection of all this stat’s attached modifiers.
	/// </summary>
	/// <remarks>
	/// Always sorted in order of highest to lowest priority.
	/// </remarks>
	public readonly ReadOnlyCollection<StatModifierInstance> Modifiers;
	/// <summary>
	/// A collection of all this stat’s attached modifiers.
	/// </summary>
	protected List<StatModifierInstance> m_Modifiers = new();


	/// <summary>
	/// Adds an existing modifier instance to the stat's <c>Modifiers</c> 
	/// collection.
	/// </summary>
	/// <param name="instance">
	/// The modifier instance to add to the collection.
	/// </param>
	public virtual void AttachModifierInstance(StatModifierInstance instance)
	{
		// insert in order of highest to lowest priority
		// credit: https://stackoverflow.com/a/12172412/19321997

		var index = m_Modifiers.BinarySearch(
			instance, 
			new StatModifierInstance.PriorityComparer()
		);

		if (index < 0) 
		{
			index = ~index;
		}

		m_Modifiers.Insert(index, instance);

		instance.LevelChanged += (_, _) => IsDirty = true;

		EmitSignal(SignalName.ModifiersChanged);
	}
	/// <summary>
	/// Creates and adds a new modifier instance to the stat's <c>Modifiers</c> 
	/// collection
	/// </summary>
	/// <param name="modifier">
	/// The modifier to create an instance of and add to the collection
	/// </param>
	public StatModifierInstance AttachModifier(StatModifier modifier)
	{
		StatModifierInstance __instance = new(modifier, 1);
		AttachModifierInstance(__instance);
		return __instance;
	}
	/// <summary>
	/// Creates and adds a new modifier instance to the stat's <c>Modifiers</c> 
	/// collection
	/// </summary>
	/// <remarks>
	/// (This overload cannot be used in GDScript, as neither C# overloads nor 
	/// defaults seem to be recognized by the language.)
	/// </remarks>
	/// <param name="modifier">
	/// The modifier to create an instance of and add to the collection
	/// </param>
	/// <param name="level">
	/// The initial level of the new modifier instance
	/// </param>
	public StatModifierInstance AttachModifier(StatModifier modifier, float level = 1)
	{
		StatModifierInstance __instance = new(modifier, level);
		AttachModifierInstance(__instance);
		return __instance;
	}


	/// <summary>
	/// Detaches the input <c>instance</c> from the stat, if attached.
	/// </summary>
	/// <param name="instance">The instance to detach</param>
	/// <returns>Whether or not the instance was detached</returns>
	public bool TryDetachModifierInstance(StatModifierInstance instance)
    {
        int __oldCount = m_Modifiers.Count;

        m_Modifiers.Remove(instance);

        if (__oldCount <= m_Modifiers.Count)
        {
            return false;
        }

        EmitSignal(SignalName.ModifiersChanged);
        return true;
    }
	/// <summary>
	/// Detaches any and all instances of the input <c>modifier</c> from the stat.
	/// </summary>
	/// <param name="modifier">The modifier of the instances to detach</param>
	/// <returns>The amount of instances detached</returns>
	public int TryDetachModifier(StatModifier modifier)
    {
		int __count = 0;
        foreach (var __instance in GetInstances(modifier).ToArray())
        {
            if (!TryDetachModifierInstance(__instance))
            {
                continue;
            }
            __count++;
        }
		return __count;
    }


	/// <summary>
	/// Gets the first found instance of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find an instance of</param>
	/// <returns>The first found instance</returns>
	public StatModifierInstance GetInstance(StatModifier modifier)
	{
		return m_Modifiers.FirstOrDefault(m => m.Modifier == modifier);
	}
	/// <summary>
	/// Gets the first found instance of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find an instance of</param>
	/// <param name="instance">The first found instance of the modifier</param>
	/// <returns>Whether or not an instance was found</returns>
	public bool TryGetInstance(StatModifier modifier, out StatModifierInstance instance)
	{
		instance = GetInstance(modifier);
		return instance != null;
	}

	/// <summary>
	/// Gets any attached instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find instances of</param>
	/// <returns>Any found instances</returns>
	public IEnumerable<StatModifierInstance> GetInstances(StatModifier modifier)
	{
		return m_Modifiers.Where(m => m.Modifier == modifier);
	}
	/// <summary>
	/// Gets any attached instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find instances of</param>
	/// <param name="instances">Any found instances of the modifier</param>
	/// <returns>Whether or not any instances were found</returns>
	public bool TryGetInstances(StatModifier modifier, out IEnumerable<StatModifierInstance> instances)
	{
		instances = GetInstances(modifier);
		return instances.Any();
	}


    /// <summary>
    /// Applies all of this stat's modifiers to <c>input</c> in order of 
	/// priority.
    /// </summary>
    public float ApplyModifiers(float input)
	{
		foreach (var __instance in m_Modifiers)
		{
			input = __instance.Modify(input);
		}

		return input;
	}
}
