using Godot;

namespace StatHub;

public abstract partial class Stat
{
	public delegate void ValueDirtied();
	public event ValueDirtied onValueDirtied;

	public delegate void ValueUpdated(float previous, float current);
	public event ValueUpdated onValueUpdated;


	/// <summary>
	/// The current value of the stat
	/// </summary>
	public float Value { get => GetCurrentValue(); }
	protected float m_valueCached;

	/// <value>
	/// True if the value is due for an update, otherwise false
	/// </value>
	public bool IsDirty { 
		get => _isDirty;
		set {
			bool __wasDirty = _isDirty;
			_isDirty = value;

			if (!__wasDirty && _isDirty)
			{
				onValueDirtied?.Invoke();
			}
		}
	}
	private bool _isDirty;

	public enum ValueUpdateOptions
	{
		/// <summary>
		/// The value attempts to update once it's requested. 
		/// </summary>
		/// <remarks>
		/// This is optimal and should not be changed unless necessary.
		/// </remarks>
		ON_REQUEST_VALUE = 0, 
		/// <summary>
		/// The value is updated as soon as it is marked as dirty. This is 
		/// essentially an instant update, but may cause performance downsides 
		/// if overused.
		/// </summary>
		/// <remarks>
		/// This is especially useful in scenarios where you don't directly 
		/// request the value but would still like to know when it is updated,
		/// such as when using event-based logic.
		/// </remarks>
		ON_DIRTIED, 
		/// <summary>
		/// The value attempts to update on every call to this <c>Node</c>'s 
		/// <c>_Process(...)</c> function.
		/// </summary>
		/// <remarks>
		/// This may reduce unnecessary updates compared to <c>ON_DIRTIED</c>,
		/// however it may also be prone to inconsistencies when requested 
		/// directly.
		/// </remarks>
		ON_PROCESS,
		/// <summary>
		/// The value attempts to update on every call to this <c>Node</c>'s 
		/// <c>_PhysicsProcess(...)</c> function.
		/// </summary>
		/// <remarks>
		/// This may reduce unnecessary updates compared to <c>ON_DIRTIED</c>,
		/// however it may also be prone to inconsistencies when requested 
		/// directly.
		/// </remarks>
		ON_PHYSICS_PROCESS,
		/// <summary>
		/// The value does not update at all unless manually requested. Call 
		/// <c>UpdateValue()</c> to update it when needed.
		/// </summary>
		/// <remarks>
		/// This is not recommended for 99.999% of scenarios, but it certainly 
		/// is an option, I suppose.
		/// </remarks>
		MANUAL,
	}
	[Export]
	public ValueUpdateOptions ValueUpdateOption { get; private set; }


	protected float GetCurrentValue()
    {
        if (IsDirty && ValueUpdateOption == ValueUpdateOptions.ON_REQUEST_VALUE)
		{
			UpdateValue();
		}
		
		return m_valueCached;
    }

	/// <summary>
	/// Resets the value to the base value and reapplies its modifiers
	/// </summary>
	public virtual void UpdateValue()
	{
		float __previousValue = m_valueCached;

		m_valueCached = ApplyModifiers(GetBaseValue());

		IsDirty = false;

		onValueUpdated?.Invoke(__previousValue, m_valueCached);
	}
	/// <returns>
	/// The current value of the stat before modifiers are applied
	/// </returns>
	protected abstract float GetBaseValue();
}
