using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace StatHub;

/// <summary>
/// A container used to store each <c>Stat</c> owned by a particular source. 
/// Stats must be children of a <c>StatContainer</c> in order for them to be 
/// recognized by the <c>StatHub</c> and used with most of its functionality.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatContainer.png")]
public sealed partial class StatContainer : Node, IEnumerable<Stat>
{
	public StatContainer() 
	{
		Stats = m_Stats.AsReadOnly();
	}


	public delegate void ContainerLoaded(StatContainer container);
	/// <summary>
	/// DOC
	/// </summary>
	public static event ContainerLoaded onContainerLoaded;

	public delegate void ContainerUnloaded(StatContainer container);
	/// <summary>
	/// DOC
	/// </summary>
	public static event ContainerUnloaded onContainerUnloaded;


	/// <summary>
	/// DOC
	/// </summary>
	/// <value></value>
	[Export]
	public TagHolder TagHolder { get; private set; }


	#region Stats
	/// <summary>
	/// Contains all <c>Stat</c> children of this <c>StatContainer</c>.
	/// </summary>
	/// <remarks>
	/// Call <c>UpdateStatsList()</c> to manually update the collection.
	/// </remarks> 
    public readonly ReadOnlyCollection<Stat> Stats;
	private readonly List<Stat> m_Stats = new();
	private readonly Godot.Collections.Array<Stat> m_GodotStats = new();

	/// <summary>
	/// Gets the stats of this container as a Godot array. This is often 
	/// unnecessary in C#, and you should try accessing the <c>Stats</c> 
	/// collection, instead.
	/// </summary>
	/// <returns>The stats of this container as a Godot array</returns>
	public Godot.Collections.Array<Stat> GetStats()
	{
		return m_GodotStats;
	}

	/// <summary>
	/// Searches through this <c>StatContainer</c>'s children (recursively) for 
	/// any <c>Stat</c>s. All found <c>Stat</c>s are sent to the <c>Stats</c> 
	/// collection.
	/// </summary>
	public void UpdateStatsList()
	{
		m_Stats.Clear();
		m_GodotStats.Clear();
		SearchForStats(this);
	}

	private void SearchForStats(Node parent)
	{
		if (parent is Stat __stat)
		{
			m_Stats.Add(__stat);
			m_GodotStats.Add(__stat);
		}

		foreach (Node __child in parent.GetChildren())
		{
			SearchForStats(__child);
		}
	}
	#endregion


    public override void _Ready()
    {
		// This needs to be called or else the Hub may not hear when the 
		// container loads, thereby not adding it to the list of containers
		StatHub.__TryInit();

		TagHolder?.__Init();

        base._Ready();

		onContainerLoaded?.Invoke(this);

		UpdateStatsList();
    }


    public override void _ExitTree()
    {
		onContainerUnloaded?.Invoke(this);

        base._ExitTree();
    }


    public IEnumerator<Stat> GetEnumerator()
    {
        return Stats.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
