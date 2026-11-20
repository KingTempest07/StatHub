using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// Modifies an input value based on a set custom expression.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatExpressionModifierStrategy.png")]
public sealed partial class StatExpressionModifierStrategy : StatModifierStrategy
{
	/// <summary>
	/// A custom expression that defines how an input value is modified. This 
	/// string will be parsed into the type <c>Expression</c>, so for usage, 
	/// refer to the docs:  
	/// https://docs.godotengine.org/en/stable/classes/class_expression.html
	/// https://docs.godotengine.org/en/stable/tutorials/scripting/evaluating_expressions.html
	/// </summary>
	/// <remarks>
	/// Use the word "input" for the input value and "level" for the modifier 
	/// instance's level.
	/// </remarks> 
	[Export(PropertyHint.Expression)]
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


    public override float Modify(ModifierInstance instance, float input)
    {
        if (m_parsedExpression == null && ParseExpression() != Error.Ok) 
		{
			PushError($"A stat expression modifier strategy's expression failed to parse for the modifier \"{instance.Modifier.DebugName}\"! This modification will be ignored. Error: {m_parsedExpression.GetErrorText()}");
		}

		Variant output = m_parsedExpression.Execute(
			new Godot.Collections.Array() { input, instance.Level }
		);

		if (m_parsedExpression.HasExecuteFailed())
		{
			PushError($"A stat expression modifier strategy's expression failed to execute for the modifier \"{instance.Modifier.DebugName}\"! This modification will be ignored. Error: {m_parsedExpression.GetErrorText()}");
			return input;
		}

		return output.As<float>(); 
    }
}
