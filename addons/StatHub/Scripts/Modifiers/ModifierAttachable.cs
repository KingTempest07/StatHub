using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

namespace StatHub;

/// <summary>
/// The base class for objects that make use of modifiers. Prefer overriding the 
/// generic base, instead.
/// </summary>
public abstract partial class ModifierAttachable : Node
{
	public ModifierAttachable()
	{
		Modifiers = m_Modifiers.AsReadOnly();
	}


	/// <summary>
	/// Invoked when this object's list of modifiers is changed.
	/// </summary>
	[Signal]
	public delegate void ModifiersChangedEventHandler();


	/// <summary>
	/// The tag holder to match for this attachable.
	/// </summary>
	/// <value></value>
	[Export]
	public TagHolder TagHolder { get; private set; }


    /// <summary>
	/// A collection of all the attached modifiers.
	/// </summary>
	/// <remarks>
	/// Always sorted in order of highest to lowest priority.
	/// </remarks>
	public readonly ReadOnlyCollection<ModifierInstance> Modifiers;
	private readonly List<ModifierInstance> m_Modifiers = new();


    public override void _Ready()
    {
		TagHolder?.__Init();

        base._Ready();
    }


    /// <summary>
    /// Checks whether or not the input <c>modifier</c> is allowed to be 
	/// attached to this object.
    /// </summary>
    /// <param name="modifier">The modifier to validate.</param>
    /// <returns>
	/// <c>true</c> if the input <c>modifier</c> is valid, otherwise false.
	/// </returns>
    public abstract bool IsValidModifier(Modifier modifier);


	/// <summary>
	/// Adds an existing modifier instance to the <c>Modifiers</c> 
	/// collection.
	/// </summary>
	/// <param name="instance">
	/// The modifier instance to add to the collection.
	/// </param>
	public virtual void TryAttachModifierInstance(ModifierInstance instance)
	{
		if (!IsValidModifier(instance.Modifier))
		{
			return; 
		}

		// insert in order of highest to lowest priority
		// credit: https://stackoverflow.com/a/12172412/19321997
		var index = m_Modifiers.BinarySearch(
			instance, 
			new ModifierInstance.PriorityComparer()
		);
		if (index < 0) 
		{
			index = ~index;
		}
		m_Modifiers.Insert(index, instance);
		EmitSignal(SignalName.ModifiersChanged);
	}

	/// <summary>
    /// Creates and adds a new modifier instance to the <c>Modifiers</c> 
    /// collection
    /// </summary>
    /// <param name="modifier">
    /// The modifier to create an instance of and add to the collection
    /// </param>
    public ModifierInstance TryAttachModifier(Modifier modifier)
	{
		if (!IsValidModifier(modifier))
		{
			return null;
		}
		ModifierInstance __instance = new(modifier, 1);
		TryAttachModifierInstance(__instance);
		return __instance;
	}
	/// <summary>
	/// Creates and adds a new modifier instance to the <c>Modifiers</c> 
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
	public ModifierInstance TryAttachModifier(Modifier modifier, float level = 1)
	{
		if (!IsValidModifier(modifier))
		{
			return null;
		}
		ModifierInstance __instance = new(modifier, level);
		TryAttachModifierInstance(__instance);
		return __instance;
	}


	/// <summary>
	/// Detaches the input <c>instance</c>, if attached.
	/// </summary>
	/// <param name="instance">The instance to detach</param>
	/// <returns>Whether or not the instance was detached</returns>
	public bool TryDetachModifierInstance(ModifierInstance instance)
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
	/// Detaches any and all instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier of the instances to detach</param>
	/// <returns>The amount of instances detached</returns>
	public int TryDetachModifier(Modifier modifier)
    {
		int __count = 0;
        foreach (var __instance in TryGetInstances(modifier).ToArray())
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
	public ModifierInstance TryGetInstance(Modifier modifier)
	{
		if (!IsValidModifier(modifier))
		{
			return null;
		}
		return Modifiers.FirstOrDefault(m => m.Modifier == modifier);
	}
	/// <summary>
	/// Gets the first found instance of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find an instance of</param>
	/// <param name="instance">The first found instance of the modifier</param>
	/// <returns>Whether or not an instance was found</returns>
	public bool TryGetInstance(Modifier modifier, out ModifierInstance instance)
	{
		if (!IsValidModifier(modifier))
		{
			instance = null;
			return false;
		}
		instance = TryGetInstance(modifier);
		return instance != null;
	}

	/// <summary>
	/// Gets any attached instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find instances of</param>
	/// <returns>Any found instances</returns>
	public IEnumerable<ModifierInstance> TryGetInstances(Modifier modifier)
	{
		if (!IsValidModifier(modifier))
		{
			return null;
		}
		return Modifiers.Where(m => m.Modifier == modifier);
	}
	/// <summary>
	/// Gets any attached instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find instances of</param>
	/// <param name="instances">Any found instances of the modifier</param>
	/// <returns>Whether or not any instances were found</returns>
	public bool TryGetInstances(Modifier modifier, out IEnumerable<ModifierInstance> instances)
	{
		if (!IsValidModifier(modifier))
		{
			instances = null;
			return false;
		}
		instances = TryGetInstances(modifier);
		return instances.Any();
	}


    /// <summary>
    /// Applies all attached modifiers to <c>input</c> in order of priority.
    /// </summary>
    public abstract float ApplyModifiers(float input);
}
