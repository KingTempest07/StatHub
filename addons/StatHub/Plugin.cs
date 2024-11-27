#if TOOLS
using Godot;

namespace StatHub;

[Tool]
public partial class Plugin : EditorPlugin
{
	private string STATHUB_AUTOLOAD_NAME = "StatHubInstance";

	public override void _EnterTree()
	{
		AddAutoloadSingleton(STATHUB_AUTOLOAD_NAME, "res://addons/StatHub/Scripts/Hub/StatHub.cs");
	}

	public override void _ExitTree()
	{
		RemoveAutoloadSingleton(STATHUB_AUTOLOAD_NAME);
	}
}
#endif
