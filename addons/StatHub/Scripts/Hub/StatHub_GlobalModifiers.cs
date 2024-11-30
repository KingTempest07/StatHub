using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace StatHub;

public partial class StatHub : Node
{
	/// <summary>
	/// Emitted when a global modifier is added via the Hub.
	/// </summary>
	/// <param name="globalModifier">The newly added global modifier</param>
	[Signal]
	public delegate void AddedGlobalModifierEventHandler(GlobalModifier globalModifier);
	/// <summary>
	/// Emitted when a global modifier is removed via the Hub.
	/// </summary>
	/// <param name="globalModifier">The newly removed global modifier</param>
	[Signal]
	public delegate void RemovedGlobalModifierEventHandler(GlobalModifier globalModifier);


	/// <summary>
	/// A collection of all active global modifiers created by the Hub
	/// </summary>
	public static readonly ReadOnlyCollection<GlobalModifier> GlobalModifiers;
	private static readonly List<GlobalModifier> m_GlobalModifiers = new();


	/// <summary>
	/// Creates a new global modifier using the input <c>modifier</c> then 
	/// activates it
	/// </summary>
	/// <param name="modifier">The modifier to make global</param>
	/// <returns>The newly created global modifier</returns>
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
	/// Activates the input <c>globalModifier</c>
	/// </summary>
	/// <remarks>
	/// This will work even if the modifier is already activated, adding a 
	/// second time
	/// </remarks>
	/// <param name="globalModifier">The global modifier to add</param>
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

		Instance.EmitSignal(SignalName.AddedGlobalModifier, globalModifier);
	}


	/// <summary>
	/// Deactivates and removes the input <c>globalModifier</c>
	/// </summary>
	/// <param name="globalModifier">The global modifier to remove</param>
	public static void RemoveGlobalModifier(GlobalModifier globalModifier)
	{
		foreach (Stat __stat in globalModifier.AttachedStats)
		{
			__stat.TryDetachModifier(globalModifier.Modifier);
		}

		m_GlobalModifiers.Remove(globalModifier);

		Instance.EmitSignal(SignalName.RemovedGlobalModifier, globalModifier);
	}


	private static void TryAttachGlobalModifiers(StatContainer statContainer)
	{
		foreach (GlobalModifier __globalModifier in GlobalModifiers)
		{
			foreach (Stat __stat in statContainer.Stats)
			{
				TryAttachGlobalModifiers(__stat);
			}
		}
	}
	private static void TryAttachGlobalModifiers(Stat stat)
	{
		foreach (GlobalModifier __globalModifier in GlobalModifiers)
		{
			stat.TryDetachModifier(__globalModifier.Modifier);
			TryAttachGlobalModifier(stat, __globalModifier, true);
		}
	}

	private static void TryAttachGlobalModifier(Stat stat, GlobalModifier globalModifier, bool stale)
	{
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
	/// Get all stats recognized by the Hub that match the given tag matchers
	/// </summary>
	/// <param name="containerTagMatcher">The container matcher to use</param>
	/// <param name="statTagMatcher">The stat matcher to use</param>
	/// <returns>Any found matching stats</returns>
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
