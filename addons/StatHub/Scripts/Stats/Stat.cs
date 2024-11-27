using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public abstract partial class Stat : Node
{
	public Stat()
	{
		Modifiers = m_Modifiers.AsReadOnly();
	}


	/// <summary>
	/// DOC
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
			onValueDirtied += UpdateValue;
		}

		onModifiersChanged += () => IsDirty = true;
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
    /// DOC
    /// </summary>
    /// <returns></returns>
    public StatContainer GetContainer() => StatHub.GetContainer(this);


    public override string ToString() => Value.ToString();


    public static implicit operator float(Stat stat) => stat.Value;
}
