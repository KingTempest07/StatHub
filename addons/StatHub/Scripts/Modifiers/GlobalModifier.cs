using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// A global modifier is an object used to automatically attach a modifier to 
/// applicable stats.
/// </summary>
/// <remarks>
/// This class has no functionality on its own and should be used in tandem with 
/// the Hub and its helpers.
/// </remarks>
[GlobalClass]
public sealed partial class GlobalModifier : RefCounted
{
	/// <summary>
	/// Creates a new global modifier of the input <c>modifier</c>.
	/// </summary>
	/// <remarks>
	/// There is often no real reason to create this directly. Go through the 
	/// helper <c>StatHub.CreateAndAddGlobalModifier(...)</c>, instead.
	/// </remarks>
	/// <param name="modifier">
	/// The modifier to make global
	/// </param>
	/// <param name="persistent">
	/// Whether or not the modifier will continue to attach to newly created 
	/// stats after first being created
	/// </param>
	public GlobalModifier(StatModifier modifier, bool persistent)
	{
		Modifier = modifier;
		Persistent = persistent;
	}


	/// <summary>
	/// The modifier this globalizes.
	/// </summary>
	public readonly StatModifier Modifier;


	/// <summary>
	/// Whether or not the modifier will continue to attach to newly created 
	/// stats after first being created.
	/// </summary>
	public readonly bool Persistent;


	/// <summary>
	/// A collection of all stats this modifier is attached to.
	/// </summary>
	public readonly HashSet<Stat> AttachedStats = new();
}
