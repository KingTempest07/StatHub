using Godot;

namespace StatHub;

/// <summary>
/// A basic implementation of a stat with a consistent yet editable base value
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/SimpleStat.png")]
public partial class SimpleStat : Stat
{
	/// <summary>
	/// The value of the stat before modifiers are applied
	/// </summary>
	[Export]
	public float BaseValue { 
		get => _baseValue;
		set {
			_baseValue = value;
			IsDirty = true;
		} 
	}
	private float _baseValue;
    protected override float GetBaseValue() => _baseValue;
}
