using Godot;

namespace StatHub;

/// <summary>
/// A stat provides a base value and returns that value applied with any 
/// attached modifiers.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/Stat.png")]
public abstract partial class Stat : ModifierAttachable<StatModifier>
{
    public override void _Ready()
    {
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
    public HubContainer GetContainer() => StatHub.GetContainer(this);


    public override void TryAttachModifierInstance(ModifierInstance instance)
    {
        base.TryAttachModifierInstance(instance);
		instance.LevelChanged += (_, _) => IsDirty = true;
    }

    public override float ApplyModifiers(float input)
    {
		foreach (var __instance in Modifiers)
		{
			input = (__instance.Modifier as StatModifier).Modify(
				__instance, 
				input
			);
		}
		return input;
    }


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
