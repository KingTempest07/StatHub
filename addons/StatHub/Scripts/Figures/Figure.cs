using Godot;

namespace StatHub;

/// <summary>
/// A floating-point value with integration into the stat system.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/Figure.png")]
public partial class Figure : ModifierAttachable<FigureModifier>
{
	/// <summary>
	/// The default value of the figure.
	/// </summary>
	/// <remarks>
	/// This is assigned to the <c>Value</c> on ready or on a call to 
	/// <c>Default()</c>.
	/// </remarks>
    [Export]
	public float DefaultValue { get; private set; }


	/// <summary>
	/// The lowermost value that the figure may reach.
	/// </summary>
	[Export]
	public Stat Minimum { get; private set; }

	// DOC


	[Export]
	public bool InstantlyFitToMinimumOnChanged = true;
	
	public enum 
	[Export]
	public bool KeepPercentOnMinimumChanged;


	/// <summary>
	/// The uppermost value that the figure may reach.
	/// </summary>
	[Export]
	public Stat Maximum { get; private set; }

	public enum OnBoundChanged {
		FIT,

		KEEP_PERCENT,


	}


	/// <summary>
	/// The current value of the figure.
	/// </summary>
	public float Value { get; private set; }


    public override void _Ready()
    {
		Default();
		// TODO add functionality for when min/max changes
        base._Ready();
    }


	/// <summary>
	/// Sets the value of the figure to its default.
	/// </summary> 
	/// <remarks>
	/// Modifiers and other changes are retained.
	/// </remarks>
	public void Default()
	{
		Value = GetClamped(DefaultValue);
	}


	private FigureValueEditor m_figureValueEditorTemp = null;
	public override float ApplyModifiers(float input)
    {
		foreach (var __instance in Modifiers)
		{
			input = (__instance.Modifier as FigureModifier).Modify(
				__instance, 
				input, 
				Value, 
				m_figureValueEditorTemp
			);
		}
		return input;
    }
	public float ApplyModifiers(float input, FigureValueEditor editor)
	{
		m_figureValueEditorTemp = editor;
		float __result = ApplyModifiers(input);
		m_figureValueEditorTemp = null;
		return __result;
	}


    public override string ToString() => Value.ToString();
}
