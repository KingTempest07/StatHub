using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace StatHub;

/// <summary>
/// A container is used to store each <c>Stat</c> owned by a particular source. 
/// Stats must be children of a <c>StatContainer</c> to be recognized by the 
/// <c>StatHub</c> and used with most of its functionality.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatContainer.png")]
public sealed partial class StatContainer : Node, IEnumerable<Stat>
{
	public StatContainer() 
	{
		Stats = m_Stats.AsReadOnly();
	}


	public delegate void LoadedContainer(StatContainer container);
	/// <summary>
	/// Invoked when a container is readied.
	/// </summary>
	/// <remarks>
	/// This is not intended for use, and the Hub’s <c>onAddedContainer</c> 
	/// should be used, instead.
	/// </remarks>
	public static event LoadedContainer onLoadedContainer;

	public delegate void UnloadedContainer(StatContainer container);
	/// <summary>
	/// Invoked when a container has exited the tree.
	/// </summary>
	/// <remarks>
	/// This is not intended for use, and the Hub’s <c>onRemovedContainer</c> 
	/// should be used, instead.
	/// </remarks>
	public static event UnloadedContainer onUnloadedContainer;


	/// <summary>
	/// The tag holder to match for this container.
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
	/// any stats. All found stats are sent to the <c>Stats</c> collection.
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

		onLoadedContainer?.Invoke(this);

		UpdateStatsList();
    }


    public override void _ExitTree()
    {
		onUnloadedContainer?.Invoke(this);

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
