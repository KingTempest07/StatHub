using Godot;

namespace StatHub;

/// <summary>
/// Based on its settings and internal rules, a modifier edits then returns an 
/// input value.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatModifier.png")]
public abstract partial class StatModifier : Resource
{
	/// <summary>
	/// The matcher used to determine applicable containers when used as a 
	/// global modifier.
	/// </summary>
	/// <remarks>
	/// If unassigned, this will not match with any containers and cannot be 
	/// used as a global modifier.
	/// </remarks>
	[Export]
	public TagMatcher ContainerTagMatcher { get; private set; }
	/// <summary>
	/// The matcher used to determine applicable stats when used as a global modifier.
	/// </summary>
	/// <remarks>
	/// If unassigned, this will not match with any stats and cannot be used as a global modifier.
	/// </remarks>
	[Export]
	public TagMatcher StatTagMatcher { get; private set; }


	/// <summary>
	/// Defines when this modifier will be applied compared to other modifiers 
	/// attached to the same stat. 
	/// </summary>
	/// <remarks>
	/// Higher priority modifiers are applied first. Modifiers of the same 
	/// priority may be sorted inconsistently, so it is best to avoid overlap.
	/// </remarks>
	[Export]
	public int Priority { get; private set; }


	/// <summary>
	/// Whether or not the modifier should continue to attach to newly created 
	/// stats when used as a global modifier.
	/// </summary>
	[Export]
	public bool PersistentIfGlobal { get; private set; }


	/// <summary>
	/// Defaults to the resource's name if not specified.
	/// </summary>
	[Export]
	public string DebugName { 
		get => _debugName == ""
			? ResourceName
			: _debugName; 
		private set => _debugName = value; 
	}
	private string _debugName;


	/// <summary>
	/// Modifies the input value based on this modifier and an instance of it.
	/// </summary>
	/// <param name="instance">The instance used to modify the input.</param>
	/// <param name="input">The value to be modified.</param>
	/// <returns>The newly modified value</returns>
    public abstract float Modify(StatModifierInstance instance, float input);
}
