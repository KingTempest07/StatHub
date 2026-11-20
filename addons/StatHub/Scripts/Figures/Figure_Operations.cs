using Godot;

namespace StatHub;

public partial class Figure
{
	public void SetBypassed(float value)
	{
		Value = value;
	}


	#region Add
	/// <summary>
	/// Adds the input <c>addend</c> to the figure's value with the change 
	/// being affected by modifiers.
	/// </summary>
	/// <param name="addend"></param>
	/// <param name="editor">DOC</param>
	/// <returns>The effective change in amount</returns>
	public float Add(float addend, FigureValueEditor editor = null)
	{
		float __modified = ApplyModifiers(addend, editor);
		return AddBypassed(__modified);
	}
	/// <summary>
	/// Adds the input <c>addend</c> to the figure's value, bypassing any 
	/// modifiers.
	/// </summary>
	/// <param name="addend">The amount to add to the value</param>
	/// <returns>The effective change in amount</returns>
	public float AddBypassed(float addend)
	{
		float __newValue = Value + addend;
		Value = GetClamped(__newValue);
		return addend - (__newValue - Value);
	}

	/// <summary>
	/// Adds <c>addend</c> to the figure's value via <c>Add(...)</c> with a 
	/// <c>null</c> <c>editor</c>.
	/// </summary>
	/// <returns>The newly updated figure</returns>
	public static Figure operator +(Figure figure, float addend) { 
		figure.Add(addend);
		return figure;
	}
	/// <summary>
	/// Adds <c>1</c> to the figure's value via <c>Add(...)</c> with a 
	/// <c>null</c> <c>editor</c>.
	/// </summary>
	/// <returns>The newly updated figure</returns>
	public static Figure operator ++(Figure figure) { 
		figure.Add(1);
		return figure;
	}
	#endregion


	#region Subtract
	/// <summary>
	/// Subtracts the input <c>subtrahend</c> from the figure's value with the 
	/// change being affected by modifiers.
	/// </summary>
	/// <param name="subtrahend">The amount to subtract from the value</param>
	/// <param name="editor">DOC</param>
	/// <returns>The effective change in amount</returns>
	public float Subtract(float subtrahend, FigureValueEditor editor = null)
	{
		return Add(-subtrahend, editor);
	}
	/// <summary>
	/// Subtracts the input <c>subtrahend</c> from the figure's value, bypassing 
	/// any modifiers.
	/// </summary>
	/// <param name="subtrahend">The amount to subtract from the value</param>
	/// <returns>The effective change in amount</returns>
	public float SubtractBypassed(float subtrahend)
	{
		return AddBypassed(-subtrahend);
	}

	/// <summary>
	/// Subtracts <c>subtrahend</c> from the figure's value via 
	/// <c>Subtract(...)</c> with a <c>null</c> <c>editor</c>.
	/// </summary>
	/// <returns>The newly updated figure</returns>
	public static Figure operator -(Figure figure, float subtrahend) { 
		figure.Subtract(subtrahend);
		return figure;
	}
	/// <summary>
	/// Subtracts <c>1</c> from the figure's value via <c>Subtract(...)</c> with 
	/// a <c>null</c> <c>editor</c>.
	/// </summary>
	/// <returns>The newly updated figure</returns>
	public static Figure operator --(Figure figure) { 
		figure.Subtract(1);
		return figure;
	}
	#endregion


	#region Multiply
	/// <summary>
	/// Multiplies the figure's value by the input <c>multiplier</c>, with the 
	/// change being affected by modifiers.
	/// </summary>
	/// <param name="multiplier">The amount to multiply the value by</param>
	/// <param name="editor">DOC</param>
	/// <returns>The effective change in amount</returns>
	public float Multiply(float multiplier, FigureValueEditor editor = null)
	{
		float __addend = (Value * multiplier) - Value;
		float __modifiedAddend = ApplyModifiers(__addend, editor);
		return AddBypassed(__modifiedAddend);
	}
	/// <summary>
	/// Multiplies the figure's value by the input <c>multiplier</c>, bypassing 
	/// any modifiers.
	/// </summary>
	/// <param name="multiplier">The amount to multiply the value by</param>
	/// <returns>The effective change in amount</returns>
	public float MultiplyBypassed(float multiplier)
	{
		float __addend = (Value * multiplier) - Value;
		return AddBypassed(__addend);
	}

	/// <summary>
	/// Multiplies the figure's value by <c>multiplier</c> via 
	/// <c>Multiply(...)</c> with a <c>null</c> <c>editor</c>.
	/// </summary>
	/// <returns>The newly updated figure</returns>
	public static Figure operator *(Figure figure, float multiplier) { 
		figure.Multiply(multiplier);
		return figure;
	}
	#endregion


	#region Divide
	/// <summary>
	/// Divides the figure's value by the input <c>divisor</c>, with the change 
	/// being affected by modifiers.
	/// </summary>
	/// <param name="divisor">The amount to divide the value by</param>
	/// <param name="editor">DOC</param>
	/// <returns>The effective change in amount</returns>
	public float Divide(float divisor, FigureValueEditor editor = null) 
	{
		float __addend = (Value / divisor) - Value;
		float __modifiedAddend = ApplyModifiers(__addend, editor);
		return AddBypassed(__modifiedAddend);
	}
	/// <summary>
	/// Divides the figure's value by the input <c>divisor</c>, bypassing any 
	/// modifiers.
	/// </summary>
	/// <param name="divisor">The amount to divide the value by</param>
	/// <returns>The effective change in amount</returns>
	public float DivideBypassed(float divisor)
	{
		float __addend = (Value / divisor) - Value;
		return AddBypassed(__addend);
	}

	/// <summary>
	/// Divides the figure's value by <c>divisor</c> via <c>Divide(...)</c> with 
	/// a <c>null</c> <c>editor</c>.
	/// </summary>
	/// <returns>The newly updated figure</returns>
	public static Figure operator /(Figure figure, float divisor) { 
		figure.Divide(divisor);
		return figure;
	}
	#endregion
	

	#region Clamp
    /// <summary>
	/// Clamps the input <c>value</c> between this figure's minimum and maximum 
	/// values.
    /// </summary>
    /// <param name="value">The value to clamp</param>
    /// <returns>
	/// The input <c>value</c> clamped between this figure's minimum and maximum 
	/// values
	/// </returns>
    public float GetClamped(float value)
	{
		if (value > Maximum)
		{
			value = Maximum;
		}
		else if (value < Minimum)
		{
			value = Minimum;
		}
		return value;
	}
	#endregion


	#region Round
	/// <summary>
	/// Gets the figure's value rounded to the nearest integer.
	/// </summary>
	public int Rounded() => Mathf.RoundToInt(Value);
	/// <summary>
	/// Gets the figure's value rounded upwards to the nearest integer.
	/// </summary>
	public int RoundedUp() => Mathf.CeilToInt(Value);
	/// <summary>
	/// Gets the figure's value rounded downwards to the nearest integer.
	/// </summary>
	public int RoundedDown() => Mathf.FloorToInt(Value);
	#endregion


	#region Implicit Value Operators
	/// <summary>
	/// Returns the <c>Value</c> of the figure.
	/// </summary>
	public static implicit operator float(Figure figure) => figure.Value;


	/// <summary>
	/// Returns the <c>Value</c> of the figure rounded to the nearest integer.
	/// </summary>
	public static implicit operator int(Figure figure) => figure.Rounded();
	#endregion
}
