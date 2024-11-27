using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace StatHub;

public partial class StatHub : Node
{
	public delegate void AddedGlobalModifier(GlobalModifier globalModifier);
	public static event AddedGlobalModifier onAddedGlobalModifier;

	public delegate void RemovedGlobalModifier(GlobalModifier globalModifier);
	public static event RemovedGlobalModifier onRemovedGlobalModifier;


	/// <summary>
	/// DOC
	/// </summary>
	public static readonly ReadOnlyCollection<GlobalModifier> GlobalModifiers;
	private static readonly List<GlobalModifier> m_GlobalModifiers = new();


	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="persistent"></param>
	public static GlobalModifier CreateAndAddGlobalModifier(StatModifier modifier)
	{
		if (modifier == null)
		{
			GD.PushError("The input modifier is null!");
			return default;
		}

		var __globalModifier = new GlobalModifier(modifier, modifier.PersistentIfGlobal);
		AddGlobalModifier(__globalModifier);
		return __globalModifier;
	}
	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="globalModifier"></param>
	public static void AddGlobalModifier(GlobalModifier globalModifier)
	{
		m_GlobalModifiers.Add(globalModifier);

		StatModifier __mod = globalModifier.Modifier;

		IEnumerable<Stat> __matchingStats = GetMatchingStats(
			__mod.ContainerTagMatcher, 
			__mod.StatTagMatcher
		);

		foreach (var __stat in __matchingStats)
		{
			AttachGlobalModifier(__stat, globalModifier);
		}

		onAddedGlobalModifier?.Invoke(globalModifier);
	}


	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="globalModifier"></param>
	public static void RemoveGlobalModifier(GlobalModifier globalModifier)
	{
		foreach (Stat __stat in globalModifier.AttachedStats)
		{
			__stat.TryDetachModifier(globalModifier.Modifier);
		}
		m_GlobalModifiers.Remove(globalModifier);
		onRemovedGlobalModifier?.Invoke(globalModifier);
	}


	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="statContainer"></param>
	public static void TryAttachGlobalModifiersToContainer(StatContainer statContainer)
	{
		foreach (GlobalModifier __globalModifier in GlobalModifiers)
		{
			foreach (Stat __stat in statContainer.Stats)
			{
				TryAttachGlobalModifiersToStat(__stat);
			}
		}
	}
	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="stat"></param>
	public static void TryAttachGlobalModifiersToStat(Stat stat)
	{
		GD.PushWarning("Hiiiii");
		foreach (GlobalModifier __globalModifier in GlobalModifiers)
		{
			GD.PushWarning("Looping");
			stat.TryDetachModifier(__globalModifier.Modifier);
			TryAttachGlobalModifier(stat, __globalModifier, true);
		}
	}


	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="globalModifier"></param>
	/// <param name="stale"></param>
	private static void TryAttachGlobalModifier(Stat stat, GlobalModifier globalModifier, bool stale)
	{
		GD.PushWarning("Trying to attach to " + stat.Name);

		if (stale && !globalModifier.Persistent)
		{
			return;
		}

		StatModifier __mod = globalModifier.Modifier;

		if (__mod.StatTagMatcher == null || __mod.ContainerTagMatcher == null)
		{
			GD.PushWarning($"The modifier named \"{__mod.DebugName}\" does not have an assigned stat tag matcher and/or container tag matcher, yet it is trying to be used as a global modifier.");
			return;
		}

		// try init just in case
		__mod.ContainerTagMatcher.TagFilter?.__Init();
		__mod.StatTagMatcher.TagFilter?.__Init();

		if (!__mod.StatTagMatcher.Matches(stat.TagHolder))
		{
			return;
		}

		StatContainer __container = stat.GetContainer();
		if (!__mod.ContainerTagMatcher.Matches(__container.TagHolder))
		{
			return;
		}

		AttachGlobalModifier(stat, globalModifier);
	}

	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="globalModifier"></param>
	private static void AttachGlobalModifier(Stat stat, GlobalModifier globalModifier)
	{
		stat.AttachModifier(globalModifier.Modifier);

		globalModifier.AttachedStats.Add(stat);
		stat.TreeExited += () =>
        {
            globalModifier.AttachedStats.Remove(stat);
        };
	}


	private static readonly HashSet<Stat> _statMatches = new();
	/// <summary>
	/// DOC
	/// </summary>
	/// <param name="containerTagMatcher"></param>
	/// <param name="statTagMatcher"></param>
	/// <returns></returns>
	public static IEnumerable<Stat> GetMatchingStats(TagMatcher containerTagMatcher, TagMatcher statTagMatcher)
	{
		_statMatches.Clear();

		if (statTagMatcher == null || containerTagMatcher == null)
		{
			GD.PushWarning($"Attempted to get matching stats with a null tag matcher!");
			return _statMatches;
		}

		// try init just in case
		containerTagMatcher.TagFilter?.__Init();
		statTagMatcher.TagFilter?.__Init();

		foreach (StatContainer __container in m_ActiveContainers)
		{
			if (!containerTagMatcher.Matches(__container.TagHolder))
			{
				continue;
			}

			foreach (Stat __stat in __container)
			{
				if (!statTagMatcher.Matches(__stat.TagHolder))
				{
					continue;
				}

				_statMatches.Add(__stat);
			}
		}

		return _statMatches;
	}
}
