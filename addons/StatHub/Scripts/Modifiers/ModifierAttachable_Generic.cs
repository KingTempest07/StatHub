using System.Collections.Generic;
using System.Linq;

namespace StatHub;

/// <summary>
/// The base class for objects that make use of modifiers.
/// </summary>
/// <typeparam name="TModifier">
/// The type of modifier that can be attached.
/// </typeparam>
public abstract partial class ModifierAttachable<TModifier> : ModifierAttachable where TModifier : Modifier
{
    public override bool IsValidModifier(Modifier modifier)
    {
        return modifier.GetType() == typeof(TModifier);
    }


    /// <summary>
    /// Creates and adds a new modifier instance to the <c>Modifiers</c> 
    /// collection
    /// </summary>
    /// <param name="modifier">
    /// The modifier to create an instance of and add to the collection
    /// </param>
    public ModifierInstance AttachModifier(TModifier modifier)
	{
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
	public ModifierInstance AttachModifier(TModifier modifier, float level = 1)
	{
		ModifierInstance __instance = new(modifier, level);
		TryAttachModifierInstance(__instance);
		return __instance;
	}


	/// <summary>
	/// Detaches any and all instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier of the instances to detach</param>
	/// <returns>The amount of instances detached</returns>
	public int TryDetachModifier(TModifier modifier)
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
	public ModifierInstance GetInstance(TModifier modifier)
	{
		return Modifiers.FirstOrDefault(m => m.Modifier == modifier);
	}
	/// <summary>
	/// Gets the first found instance of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find an instance of</param>
	/// <param name="instance">The first found instance of the modifier</param>
	/// <returns>Whether or not an instance was found</returns>
	public bool TryGetInstance(TModifier modifier, out ModifierInstance instance)
	{
		instance = GetInstance(modifier);
		return instance != null;
	}

	/// <summary>
	/// Gets any attached instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find instances of</param>
	/// <returns>Any found instances</returns>
	public IEnumerable<ModifierInstance> GetInstances(TModifier modifier)
	{
		return Modifiers.Where(m => m.Modifier == modifier);
	}
	/// <summary>
	/// Gets any attached instances of the input <c>modifier</c>.
	/// </summary>
	/// <param name="modifier">The modifier to find instances of</param>
	/// <param name="instances">Any found instances of the modifier</param>
	/// <returns>Whether or not any instances were found</returns>
	public bool TryGetInstances(TModifier modifier, out IEnumerable<ModifierInstance> instances)
	{
		instances = GetInstances(modifier);
		return instances.Any();
	}
}
