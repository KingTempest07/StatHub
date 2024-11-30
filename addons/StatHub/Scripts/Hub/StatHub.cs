using System.Collections.Generic;
using Godot;

namespace StatHub;

/// <summary>
/// The Hub is the central manager of the rest of the plugin and provides 
/// various helpers and functionality. It is an AutoLoad for use in GDScript, 
/// but most functionality is <c>static</c> to be easily used in C#.
/// </summary>
[GlobalClass, Icon("res://addons/StatHub/Assets/StatHub.png")]
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

		StatContainer.onLoadedContainer += OnLoadedContainer;
		StatContainer.onUnloadedContainer += OnUnloadedContainer;
	}


	public static StatHub Instance { get; private set; }


	private static readonly HashSet<StatAndContainer> m_StatsAndContainers = new();


	/// <summary>
	/// Used to make sure the static constructor runs before certain events 
	/// occur and ensures the instance is set. This has no functionality aside 
	/// from that and has no use for the end user.
	/// </summary>
	public static void __TryInit()
	{
		Instance ??= ((SceneTree) Engine.GetMainLoop()).Root
			.GetNode<StatHub>(Plugin.STATHUB_AUTOLOAD_NAME);
	}


    public override void _Ready()
    {
		Instance = this;
        base._Ready();
    }
}
