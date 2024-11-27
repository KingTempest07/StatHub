using Godot;
using static Godot.GD;

namespace StatHub.Examples.ConsoleOutput;

public partial class ConsoleOutput : Node
{
	#region Exports
	// EXAMPLE 1
	[Export]
	public Godot.Collections.Array<SimpleStat> Ex1SimpleStats { get; private set; }


	// EXAMPLE 2
	[Export]
	public Godot.Collections.Array<SimpleStat> Ex2SimpleStats { get; private set; }


	// EXAMPLE 3
	[Export]
	public Godot.Collections.Array<SimpleStat> Ex3SimpleStats { get; private set; }


	// EXAMPLE 4
	[Export]
	public ExpressionStat Ex4ExpressionStat1 { get; private set; }
	[Export]
	public ExpressionStat Ex4ExpressionStat2 { get; private set; }
	[Export]
	public ExpressionStat Ex4ExpressionStat3 { get; private set; }
	[Export]
	public SimpleStat Ex4SimpleStat1 { get; private set; }


	// EXAMPLE 5
	[Export]
	public StatContainer Ex5StatContainer1 { get; private set; }
	[Export]
	public StatContainer Ex5StatContainer2 { get; private set; }

	[Export]
	public StatModifier Ex5Modifier1 { get; private set; }
	[Export]
	public StatModifier Ex5Modifier2 { get; private set; }
	[Export]
	public StatModifier Ex5Modifier3 { get; private set; }


	// SHARED
	[Export]
	public SimpleModifier SimpleModifier1 { get; private set; }
	[Export]
	public SimpleModifier SimpleModifier2 { get; private set; }

	[Export]
	public ExpressionModifier ExpressionModifier1 { get; private set; }
	[Export]
	public ExpressionModifier ExpressionModifier2 { get; private set; }
	[Export]
	public ExpressionModifier ExpressionModifier3 { get; private set; }
	#endregion


    public override void _Ready()
    {
        base._Ready();

		Print("\n\n\n<<<=====Begin examples=====>>>");

		Example1();
		Example2();
		Example3();
		Example4();
		Example5();

		Print("\n\n\n<<<=====End examples=====>>>");
    }


	private void Example1()
	{
		Print("\n\n\n=====Running example #1 (modifier application/removal)=====");

		foreach (var __stat in Ex1SimpleStats)
		{
            PrintValue($"\nStarting on a new stat, \"{__stat.Name}!\"", __stat);

			StatModifierInstance __simpleMod1Instance = __stat.AttachModifier(SimpleModifier1);
            PrintValue($"Added SimpleModifier1 to the stat with a level of 1!", __stat);

			__stat.AttachModifier(SimpleModifier2);
            PrintValue($"Added SimpleModifier2 to the stat with a level of 1!", __stat);

			__stat.TryDetachModifierInstance(__simpleMod1Instance);
            PrintValue($"Removed the SimpleModifier1 instance!", __stat);
		}

		Print("\n=====End example #1!=====");
	}


	private void Example2() 
	{
		Print("\n\n\n=====Running example #2 (reusing an instance; SimpleStatModifier leveling)=====");

		StatModifierInstance __instance = new(SimpleModifier1);
		Print("\nCreated a new instance of SimpleModifier1 at level 1!");

		foreach (var __stat in Ex2SimpleStats) 
		{
            PrintValue($"\nStarting on a new stat, \"{__stat.Name}!\"", __stat);

			__stat.AttachModifierInstance(__instance);
            PrintValue($"Applied the instance to \"{__stat.Name}!\"", __stat);
		}

		__instance.Level = 2.5f;
		Print("\n--Set the instance's level to 2.5!");

		foreach (var __stat in Ex2SimpleStats) 
		{
            PrintValue($"\nRevisiting a stat, \"{__stat.Name}!\"", __stat);
		}

		Print("\nAll stats with the instance have now updated!");
		Print("\n=====End example #2!=====");
	}


	private void Example3()
	{
		Print("\n\n\n=====Running example #3 (ExpressionStatModifier)=====");

		Print("\n\n--Running example using expression mod 1...");
		__RunWithInstance(ExpressionModifier1);

		Print("\n\n--Running example using expression mod 2...");
		__RunWithInstance(ExpressionModifier2);

		Print("\n\n--Running example using expression mod 3...");
		__RunWithInstance(ExpressionModifier3);

		Print("\n=====End example #3!=====");


		void __RunWithInstance(ExpressionModifier modifier)
		{
			foreach(var __stat in Ex3SimpleStats)
			{
				PrintValue($"\nStarting on a clean stat, \"{__stat.Name}!\"", __stat);

				StatModifierInstance __instance = new(modifier);
				Print("Created a new instance of the expression mod!");

				__stat.AttachModifierInstance(__instance);
				PrintValue($"Applied the instance to \"{__stat.Name}!\"", __stat);

				__instance.Level = 5;
				PrintValue("Changed the instance's level to 5!", __stat);

				__instance.Level = 13;
				PrintValue("Changed the instance's level to 13!", __stat);

				__stat.TryDetachModifierInstance(__instance);
				Print("Removed the instance from the stat!");
			}
		}
	}


	private void Example4()
	{
		Print("\n\n\n=====Running example #4 (ExpressionStat)=====");

		PrintValues(
			"\n",
			Ex4ExpressionStat1,
			Ex4ExpressionStat2,
			Ex4ExpressionStat3
		);

		Ex4SimpleStat1.AttachModifier(SimpleModifier1);
		PrintValues(
			"\nAttached SimpleModifier1 to Ex4SimpleStat1!",
			Ex4SimpleStat1,
			Ex4ExpressionStat1,
			Ex4ExpressionStat2,
			Ex4ExpressionStat3
		);

		Ex4ExpressionStat1.AttachModifier(SimpleModifier2);
		PrintValues(
			"\nAttached SimpleModifier2 to Ex4ExpressionStat1!",
			Ex4ExpressionStat1,
			Ex4ExpressionStat2,
			Ex4ExpressionStat3
		);

		Print("\n=====End example #4!=====");
	}


	private void Example5()
	{
		Print("\n\n\n=====Running example #5 (GlobalModifier)=====\n");

		__PrintContainers();

		GlobalModifier __mod1 = StatHub.CreateAndAddGlobalModifier(Ex5Modifier1);
		Print("\nAdded new global modifier of Ex5Modifier1!");
		__PrintContainers();
		StatHub.RemoveGlobalModifier(__mod1);
		Print("Removed the global modifier!");

		GlobalModifier __mod2 = StatHub.CreateAndAddGlobalModifier(Ex5Modifier2);
		Print("\nAdded new global modifier of Ex5Modifier2!");
		__PrintContainers();
		StatHub.RemoveGlobalModifier(__mod2);
		Print("Removed the global modifier!");

		GlobalModifier __mod3 = StatHub.CreateAndAddGlobalModifier(Ex5Modifier3);
		Print("\nAdded new global modifier of Ex5Modifier3!");
		__PrintContainers();
		StatHub.RemoveGlobalModifier(__mod3);
		Print("Removed the global modifier!");

		Print("\n=====End example #5!=====");


        void __PrintContainers()
        {
            PrintValues("Container 1:", Ex5StatContainer1);
			PrintValues("Container 2:", Ex5StatContainer2);
        }
    }


	#region Helpers
	private static void PrintValues(string context, StatContainer container)
	{
		Print(context);
		PrintValues(container);
	}
	private static void PrintValues(StatContainer container)
	{
		foreach (var __stat in container)
		{
			PrintValue(__stat);
		}
	}
	private static void PrintValues(string context, params Stat[] stats)
	{
		Print(context);
		PrintValues(stats);
	}
	private static void PrintValues(params Stat[] stats)
	{
		foreach (var __stat in stats)
		{
			PrintValue(__stat);
		}
	}
	private static void PrintValue(string context, Stat stat)
	{
		Print(context);
		PrintValue(stat);
	}
	private static void PrintValue(Stat stat)
	{
		Print($"> Stat \"{stat.Name}\" value: {stat}");
	}
	#endregion
}
