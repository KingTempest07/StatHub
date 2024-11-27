using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public sealed partial class TagHolder : Resource, IEnumerable<Tag>
{
	public TagHolder()
	{
		Tags = m_tags.AsReadOnly();
	}


	/// <summary>
	/// DOC
	/// </summary>
	public readonly ReadOnlyCollection<Tag> Tags;
	private List<Tag> m_tags = new();
	[Export]
	private Godot.Collections.Array<Tag> _tags = new();


	private bool m_init;
	public void __Init()
	{
		if (m_init)
		{
			return;
		}
		m_tags = _tags.ToList();
		m_init = true;
	}


    public IEnumerator<Tag> GetEnumerator()
    {
        return m_tags.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
