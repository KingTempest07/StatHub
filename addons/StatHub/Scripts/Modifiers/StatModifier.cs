using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public abstract partial class StatModifier : Resource
{
	/// <summary>
	/// DOC will not match any if not assigned
	/// </summary>
	/// <value></value>
	[Export]
	public TagMatcher ContainerTagMatcher { get; private set; }
	/// <summary>
	/// DOC will not match any if not assigned
	/// </summary>
	/// <value></value>
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
	/// <value></value>
	[Export]
	public int Priority { get; private set; }


	/// <summary>
	/// DOC
	/// </summary>
	/// <value></value>
	[Export]
	public bool PersistentIfGlobal { get; private set; }


	[Export]
	/// <summary>
	/// Defaults to the resource's name if not specified.
	/// </summary>
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
