using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

namespace StatHub;

public partial class StatHub : Node
{
	public delegate void ContainerAdded(StatContainer container);
	/// <summary>
	/// DOC
	/// </summary>
	public static event ContainerAdded onContainerAdded;

	public delegate void ContainerRemoved(StatContainer container);
	/// <summary>
	/// DOC
	/// </summary>
	public static event ContainerRemoved onContainerRemoved;


	/// <summary>
	/// DOC
	/// </summary>
	public static readonly ReadOnlyCollection<StatContainer> ActiveContainers;
	private static readonly List<StatContainer> m_ActiveContainers = new();

	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="stat"></param>
	/// <returns></returns>
	public static StatContainer GetContainer(Stat stat)
	{
		StatAndContainer? __match = m_StatsAndContainers.FirstOrDefault(x => x.Stat == stat);
		return __match?.Container;
	}


	private static void OnContainerLoaded(StatContainer container)
	{
		m_ActiveContainers.Add(container);
		foreach (var __stat in container.Stats)
		{
			m_StatsAndContainers.Add(new(container, __stat));
		}

		TryAttachGlobalModifiersToContainer(container);

		onContainerAdded?.Invoke(container);
	}
	private static void OnContainerUnloaded(StatContainer container)
	{
		m_ActiveContainers.Remove(container);
		onContainerRemoved?.Invoke(container);
	}
}