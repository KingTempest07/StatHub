using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// DOC
/// </summary>
[GlobalClass]
public partial class StatHub : Node
{
	private readonly struct StatAndContainer
	{
		public StatAndContainer(StatContainer container, Stat stat)
		{
			Container = container;
			Stat = stat;
		}

		public readonly StatContainer Container;
		public readonly Stat Stat;
	}


    static StatHub() 
	{
		ActiveContainers = m_ActiveContainers.AsReadOnly();
		GlobalModifiers = m_GlobalModifiers.AsReadOnly();

		StatContainer.onContainerLoaded += OnContainerLoaded;
		StatContainer.onContainerUnloaded += OnContainerUnloaded;
	}


	private static readonly HashSet<StatAndContainer> m_StatsAndContainers = new();


	/// <summary>
	/// Used to make sure the static constructor runs before certain events 
	/// occur. This has no functionality aside from that and has no use for the 
	/// end user.
	/// </summary>
	public static void __TryInit()
	{
	}
}
