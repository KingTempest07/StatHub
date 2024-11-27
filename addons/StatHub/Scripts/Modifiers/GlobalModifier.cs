using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public sealed partial class GlobalModifier : RefCounted
{
	/// <summary>
	/// There is often no real reason to create this directly. Go through the 
	/// helper <c>StatHub.AddGlobalModifier(...)</c>, instead.
	/// </summary>
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
	/// The modifier that this makes global
	/// </summary>
	public readonly StatModifier Modifier;


	/// <summary>
	/// Whether or not the modifier will continue to attach to newly created 
	/// stats after first being created
	/// </summary>
	public readonly bool Persistent;


	public readonly HashSet<Stat> AttachedStats = new();
}
