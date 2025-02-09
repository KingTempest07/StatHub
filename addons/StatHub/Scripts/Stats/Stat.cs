using System;
using Godot;

namespace StatHub;

/// <summary>
/// A stat provides a base value and returns a modified value based on any 
/// attached modifiers.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/Stat.png")]
public abstract partial class Stat : Node
{
	public Stat()
	{
		Modifiers = m_Modifiers.AsReadOnly();
	}


	/// <summary>
	/// The tag holder to match for this stat.
	/// </summary>
	/// <value></value>
	[Export]
	public TagHolder TagHolder { get; private set; }


	public override void _Ready()
    {
		TagHolder?.__Init();

        base._Ready();

		if (ValueUpdateOption == ValueUpdateOptions.ON_DIRTIED)
		{
			ValueDirtied += UpdateValue;
		}

		ModifiersChanged += () => IsDirty = true;
		IsDirty = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (ValueUpdateOption == ValueUpdateOptions.ON_PROCESS)
		{
			UpdateValue();
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if (ValueUpdateOption == ValueUpdateOptions.ON_PHYSICS_PROCESS)
		{
			UpdateValue();
		}
    }


    /// <summary>
    /// Gets the container of this stat.
    /// </summary>
    public StatContainer GetContainer() => StatHub.GetContainer(this);


    public override string ToString() => Value.ToString();


	/// <summary>
	/// Gets the stat's value rounded to the nearest integer.
	/// </summary>
	public int Rounded() => Mathf.RoundToInt(Value);
	/// <summary>
	/// Gets the stat's value rounded upwards to the nearest integer.
	/// </summary>
	public int RoundedUp() => Mathf.CeilToInt(Value);
	/// <summary>
	/// Gets the stat's value rounded downwards to the nearest integer.
	/// </summary>
	public int RoundedDown() => Mathf.FloorToInt(Value);


    public static implicit operator float(Stat stat) => stat.Value;
	public static implicit operator int(Stat stat) => stat.Rounded();
}
