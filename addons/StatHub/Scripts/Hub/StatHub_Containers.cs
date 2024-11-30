using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

namespace StatHub;

public partial class StatHub : Node
{
	/// <summary>
	/// Emitted when a container is loaded and added to the Hub's collection
	/// </summary>
	/// <param name="container">The newly added container</param>
	[Signal]
	public delegate void AddedContainerEventHandler(StatContainer container);
	/// <summary>
	/// Emitted when a container is unloaded and removed from the Hub's 
	/// collection
	/// </summary>
	/// <param name="container">The newly removed container</param>
	[Signal]
	public delegate void RemovedContainerEventHandler(StatContainer container);


	/// <summary>
	/// A collection of all active stat containers recognized by the Hub
	/// </summary>
	public static readonly ReadOnlyCollection<StatContainer> ActiveContainers;
	private static readonly List<StatContainer> m_ActiveContainers = new();

	/// <summary>
	/// Gets the container of the input <c>stat</c>
	/// </summary>
	/// <param name="stat">The stat to find the container of</param>
	/// <returns>The container of the input <c>stat</c></returns>
	public static StatContainer GetContainer(Stat stat)
	{
		StatAndContainer? __match = m_StatsAndContainers.FirstOrDefault(x => x.Stat == stat);
		return __match?.Container;
	}


	private static void OnLoadedContainer(StatContainer container)
	{
		m_ActiveContainers.Add(container);
		foreach (var __stat in container.Stats)
		{
			m_StatsAndContainers.Add(new(container, __stat));
		}

		TryAttachGlobalModifiers(container);

		Instance.EmitSignal(SignalName.AddedContainer, container);
	}
	private static void OnUnloadedContainer(StatContainer container)
	{
		m_ActiveContainers.Remove(container);

		Instance.EmitSignal(SignalName.RemovedContainer, container);
	}
}