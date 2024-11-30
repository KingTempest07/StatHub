using System.Collections.Generic;
using System.Linq;
using Godot;
using static Godot.GD;

namespace StatHub;

/// <summary>
/// An extremely customizable implementation of a stat that produces its base 
/// value from a custom expression utilizing other stats as inputs.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/ExpressionStat.png")]
public partial class ExpressionStat : Stat
{
    /// <summary>
	/// A custom expression that defines how the base value is determined. This 
    /// string will be parsed into the type <c>Expression</c>, so for usage, 
    /// refer to the docs: 
	/// https://docs.godotengine.org/en/stable/classes/class_expression.html
	/// </summary>
	/// <remarks>
	/// Any keys given as inputs may be used to represent their corresponding 
    /// stat's value.
	/// </remarks> 
	[Export]
	public string CustomExpression { get; private set; }
	private Expression m_parsedExpression = null;


    /// <summary>
    /// Stores every stat used by <c>CustomExpression</c> and their corresponding
    /// keys to be parsed.
    /// </summary>
    private readonly Dictionary<string, Stat> m_StatInputs = new();
    [Export]
    private Godot.Collections.Dictionary<string, NodePath> _expressionStatInputs;


    /// <summary>
    /// Stores the last calculated base value.
    /// </summary>
    protected float m_baseValueCached;


    public override void _Ready()
    {
        foreach(var __kvp in _expressionStatInputs)
        {
            m_StatInputs.Add(__kvp.Key, GetNode<Stat>(__kvp.Value));
        }

        __ParseExpression();
        __ListenToInputStats();

        base._Ready();


        void __ParseExpression() 
        {
            m_parsedExpression = new();

            Error __error = m_parsedExpression.Parse(
                CustomExpression, 
                m_StatInputs.Keys.ToArray()
            );

            if (__error != Error.Ok) 
            {
                PushError($"The expression stat \"{Name}'s\" expression failed to parse! Expect bugs! Error: {m_parsedExpression.GetErrorText()}");
            }
        }

        void __ListenToInputStats()
        {
            foreach (Stat __input in m_StatInputs.Values)
            {
                __input.ValueUpdated += (_, _) => IsDirty = true;
            }
        }
    }


    private readonly Godot.Collections.Array m_valueArray = new();
    public override void UpdateValue()
    {
        __UpdateBaseValue();
        base.UpdateValue();


        void __UpdateBaseValue()
        {
            m_valueArray.Clear();
            foreach (Stat __stat in m_StatInputs.Values)
            {
                m_valueArray.Add(__stat.Value);
            }

            Variant output = m_parsedExpression.Execute(m_valueArray);

            if (m_parsedExpression.HasExecuteFailed())
            {
                PushError($"The expression stat \"{Name}'s\" expression failed to execute! Expect bugs! Error: {m_parsedExpression.GetErrorText()}");
                m_baseValueCached = 0;
            }

            m_baseValueCached = output.As<float>(); 
        }
    }


    protected override float GetBaseValue() => m_baseValueCached;
}
