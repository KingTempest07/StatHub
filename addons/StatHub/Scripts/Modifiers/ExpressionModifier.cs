using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/ExpressionModifier.png")]
public partial class ExpressionModifier : StatModifier
{
	/// <summary>
	/// A custom expression that defines how the input value is modified. This 
	/// string will be parsed into the type <c>Expression</c>, so for usage, 
	/// refer to the docs:  
	/// https://docs.godotengine.org/en/stable/classes/class_expression.html
	/// </summary>
	/// <remarks>
	/// Use the word "input" for the input value and "level" for the modifier 
	/// instance's level.
	/// </remarks> 
	[Export]
	public string CustomExpression { get; private set; }
	private Expression m_parsedExpression = null;
	private Error ParseExpression() 
	{
		m_parsedExpression = new();

		return m_parsedExpression.Parse(
			CustomExpression, 
			new string[] { "input", "level" }
		);
	}


    public override float Modify(StatModifierInstance instance, float input)
    {
		if (instance.Modifier != this)
		{
			PushError($"The instance given is not from the same parent modifier! (actual: \"{instance.Modifier.DebugName}\", expected: \"{DebugName}\" This modification will be ignored.");
			return input;
		}

		if (m_parsedExpression == null && ParseExpression() != Error.Ok) 
		{
			PushError($"The expression modifier \"{DebugName}'s\" expression failed to parse! This modification will be ignored. Error: {m_parsedExpression.GetErrorText()}");
		}

		Variant output = m_parsedExpression.Execute(
			new Godot.Collections.Array() { input, instance.Level }
		);

		if (m_parsedExpression.HasExecuteFailed())
		{
			PushError($"The expression modifier \"{DebugName}'s\" expression failed to execute! This modification will be ignored. Error: {m_parsedExpression.GetErrorText()}");
			return input;
		}

		return output.As<float>(); 
	}
}
