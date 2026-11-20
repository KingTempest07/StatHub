using Godot;

namespace StatHub;

/// <summary>
/// A modifier edits and returns an input value based on its settings and 
/// internal rules.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/Modifier.png")]
public abstract partial class Modifier : Resource
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
	/// Defines when this modifier will be applied compared to other modifiers 
	/// attached to the same stat or figure. 
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


	public abstract TagMatcher GetAttachableTagMatcher();
}
