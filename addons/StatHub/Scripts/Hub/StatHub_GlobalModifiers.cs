using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
	public static GlobalModifier CreateAndAddGlobalModifier(Modifier modifier)
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

		Modifier __mod = globalModifier.Modifier;

		IEnumerable<ModifierAttachable> __matchingAttachables = GetMatchingAttachables(
			__mod.ContainerTagMatcher, 
			__mod.GetAttachableTagMatcher()
		);

		foreach (Stat __stat in __matchingAttachables.Cast<Stat>())
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
		foreach (ModifierAttachable __attached in globalModifier.AttachedObjects)
		{
			__attached.TryDetachModifier(globalModifier.Modifier);
		}

		m_GlobalModifiers.Remove(globalModifier);

		Instance.EmitSignal(SignalName.RemovedGlobalModifier, globalModifier);
	}


	private static void TryAttachGlobalModifiers(HubContainer statContainer)
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

		Modifier __mod = globalModifier.Modifier;

		if (!stat.IsValidModifier(__mod))
		{
			return;
		}

		TagMatcher __attachableTagMatcher = __mod.GetAttachableTagMatcher();

		if (__attachableTagMatcher == null || __mod.ContainerTagMatcher == null)
		{
			GD.PushWarning($"The modifier named \"{__mod.DebugName}\" does not have an assigned attachable tag matcher and/or container tag matcher, yet it is trying to be used as a global modifier.");
			return;
		}

		// try init just in case
		__mod.ContainerTagMatcher.TagFilter?.__Init();
		__attachableTagMatcher.TagFilter?.__Init();

		if (!__attachableTagMatcher.Matches(stat.TagHolder))
		{
			return;
		}

		HubContainer __container = stat.GetContainer();
		if (!__mod.ContainerTagMatcher.Matches(__container.TagHolder))
		{
			return;
		}

		AttachGlobalModifier(stat, globalModifier);
	}
	private static void AttachGlobalModifier(Stat stat, GlobalModifier globalModifier)
	{
		stat.TryAttachModifier(globalModifier.Modifier);

		globalModifier.AttachedObjects.Add(stat);
		stat.TreeExited += () =>
        {
            globalModifier.AttachedObjects.Remove(stat);
        };
	}


	private static readonly HashSet<ModifierAttachable> _attachableMatches = new();
	/// <summary>
	/// Get all attachables recognized by the Hub that match the given tag matchers
	/// </summary>
	/// <param name="containerTagMatcher">The container matcher to use</param>
	/// <param name="attachableTagMatcher">The attachable matcher to use</param>
	/// <returns>Any found matching attachables</returns>
	public static IEnumerable<ModifierAttachable> GetMatchingAttachables(TagMatcher containerTagMatcher, TagMatcher attachableTagMatcher)
	{
		_attachableMatches.Clear();

		if (attachableTagMatcher == null || containerTagMatcher == null)
		{
			GD.PushWarning($"Attempted to get matching stats with a null tag matcher!");
			return _attachableMatches;
		}

		// try init just in case
		containerTagMatcher.TagFilter?.__Init();
		attachableTagMatcher.TagFilter?.__Init();

		foreach (HubContainer __container in m_ActiveContainers)
		{
			if (!containerTagMatcher.Matches(__container.TagHolder))
			{
				continue;
			}

			foreach (ModifierAttachable __attachable in __container)
			{
				if (!attachableTagMatcher.Matches(__attachable.TagHolder))
				{
					continue;
				}

				_attachableMatches.Add(__attachable);
			}
		}

		return _attachableMatches;
	}
}
