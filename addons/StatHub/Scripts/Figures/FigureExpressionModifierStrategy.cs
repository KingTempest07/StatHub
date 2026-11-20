using System.Linq;
using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// Modifies a figure's requested change in value based on a set custom 
/// expression.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/FigureExpressionModifierStrategy.png")]
public sealed partial class FigureExpressionModifierStrategy : FigureModifierStrategy
{
	/// <summary>
	/// A custom expression that defines how a change in a figure's value is 
	/// modified. This string will be parsed into the type <c>Expression</c>, so 
	/// for usage, refer to the docs:  
	/// https://docs.godotengine.org/en/stable/classes/class_expression.html
	/// https://docs.godotengine.org/en/stable/tutorials/scripting/evaluating_expressions.html
	/// </summary>
	/// <remarks>
	/// Use the word "change" for the change in value, "value" for the value 
	/// before modification, "editorTags" for the figure value editor's tags, 
	/// and "level" for the modifier instance's level.
	/// </remarks> 
	[Export(PropertyHint.Expression)]
	public string CustomExpression { get; private set; }
	private Expression m_parsedExpression = null;
	private Error ParseExpression() 
	{
		m_parsedExpression = new();

		var __keys = new Godot.Collections.Array<string> { "change", "value", "editorTags", "level" };
		__keys.AddRange(_expressionTagInputs.Keys);

		return m_parsedExpression.Parse(CustomExpression, __keys.ToArray());
	}
	
	
	/// <summary>
    /// Stores every tag used by <c>CustomExpression</c> and their corresponding
    /// keys to be parsed.
    /// </summary>
    [Export]
    private Godot.Collections.Dictionary<string, Tag> _expressionTagInputs;


    public override float Modify(ModifierInstance instance, float change, float value, FigureValueEditor editor)
    {
        if (m_parsedExpression == null && ParseExpression() != Error.Ok) 
		{
			PushError($"A stat expression modifier strategy's expression failed to parse for the modifier \"{instance.Modifier.DebugName}\"! This modification will be ignored. Error: {m_parsedExpression.GetErrorText()}");
		}

		Godot.Collections.Array<Tag> __editorTags = editor == null
			? new()
			: new(editor.TagHolder.Tags);

		Print(__editorTags);

		var __inputs = new Godot.Collections.Array() { 
			change, value, __editorTags, instance.Level
		};
		__inputs.AddRange(_expressionTagInputs.Values);

		Variant output = m_parsedExpression.Execute(__inputs, baseInstance: this, showError: false);

		if (m_parsedExpression.HasExecuteFailed())
		{
			PushError($"A stat expression modifier strategy's expression failed to execute for the modifier \"{instance.Modifier.DebugName}\"! This modification will be ignored. Error: {m_parsedExpression.GetErrorText()}");
			return change;
		}

		return output.As<float>(); 
    }
}
