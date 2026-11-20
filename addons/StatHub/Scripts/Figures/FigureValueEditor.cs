using Godot;

namespace StatHub;

/// <summary>
/// Contains context for figure value change requests.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/FigureValueEditor.png")]
public sealed partial class FigureValueEditor : Resource
{
	/// <summary>
	/// The tags used to identify the context of an edit.
	/// </summary>
    [Export]
	public TagHolder TagHolder { 
		get {
			if (m_firstTagHolderRequest)
			{
				_tagHolder.__Init();
				m_firstTagHolderRequest = false;
			}
			return _tagHolder;
		}
		private set => _tagHolder = value; 
	}
	private TagHolder _tagHolder;
	private bool m_firstTagHolderRequest = true;
}
