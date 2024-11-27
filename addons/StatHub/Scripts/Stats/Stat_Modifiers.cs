using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StatHub;

public abstract partial class Stat
{
	public delegate void ModifiersChanged();
	public event ModifiersChanged onModifiersChanged;


	/// <summary>
	/// Should always be sorted in order of highest to lowest priority
	/// </summary>
	public readonly ReadOnlyCollection<StatModifierInstance> Modifiers;
	/// <summary>
	/// DOC
	/// </summary>
	/// <returns></returns>
	protected List<StatModifierInstance> m_Modifiers = new();


	/// <summary>
	/// Adds an existing modifier instance to the stat's <c>Modifiers</c> 
	/// collection
	/// </summary>
	/// <param name="instance">
	/// The modifier instance to add to the collection
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

		instance.onLevelChanged += (_, _) => IsDirty = true;

		onModifiersChanged?.Invoke();
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
	/// DOC
	/// </summary>
	/// <param name="instance"></param>
	/// <returns></returns>
	public bool TryDetachModifierInstance(StatModifierInstance instance)
    {
        int __oldCount = m_Modifiers.Count;

        m_Modifiers.Remove(instance);

        if (__oldCount <= m_Modifiers.Count)
        {
            return false;
        }

        onModifiersChanged?.Invoke();
        return true;
    }
	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <returns></returns>
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
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <returns></returns>
	public StatModifierInstance GetInstance(StatModifier modifier)
	{
		return m_Modifiers.FirstOrDefault(m => m.Modifier == modifier);
	}
	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="instance"></param>
	/// <returns></returns>
	public bool TryGetInstance(StatModifier modifier, out StatModifierInstance instance)
	{
		instance = GetInstance(modifier);
		return instance != null;
	}

	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <returns></returns>
	public IEnumerable<StatModifierInstance> GetInstances(StatModifier modifier)
	{
		return m_Modifiers.Where(m => m.Modifier == modifier);
	}
	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="instances"></param>
	/// <returns></returns>
	public bool TryGetInstances(StatModifier modifier, out IEnumerable<StatModifierInstance> instances)
	{
		instances = GetInstances(modifier);
		return instances.Any();
	}


    /// <summary>
    /// Applies all of this stat's modifiers to the input in order of priority
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
