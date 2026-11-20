using Godot;
using static Godot.GD;

namespace StatHub.Examples.ConsoleOutput;

public partial class ConsoleOutput : Node
{
	#region Exports
	#region Ex1
	[Export]
	public Godot.Collections.Array<SimpleStat> Ex1SimpleStats { get; private set; }
	#endregion


	#region Ex2
	[Export]
	public Godot.Collections.Array<SimpleStat> Ex2SimpleStats { get; private set; }
	#endregion


	#region Ex3
	[Export]
	public Godot.Collections.Array<SimpleStat> Ex3SimpleStats { get; private set; }
	#endregion


	#region Ex4
	[Export]
	public ExpressionStat Ex4ExpressionStat1 { get; private set; }
	[Export]
	public ExpressionStat Ex4ExpressionStat2 { get; private set; }
	[Export]
	public ExpressionStat Ex4ExpressionStat3 { get; private set; }
	[Export]
	public SimpleStat Ex4SimpleStat1 { get; private set; }
	#endregion


	#region Ex5
	[Export]
	public HubContainer Ex5Container1 { get; private set; }
	[Export]
	public HubContainer Ex5Container2 { get; private set; }

	[Export]
	public StatModifier Ex5StatModifier1 { get; private set; }
	[Export]
	public StatModifier Ex5StatModifier2 { get; private set; }
	[Export]
	public StatModifier Ex5StatModifier3 { get; private set; }
	#endregion


	#region Ex6
	[Export]
	public Figure Ex6Figure { get; private set; }

	[Export]
	public FigureValueEditor Ex6FigureValueEditor1 { get; private set; }
	[Export]
	public FigureValueEditor Ex6FigureValueEditor2 { get; private set; }
	#endregion


	#region Shared
	[Export]
	public StatModifier StatSimpleModifier1 { get; private set; }
	[Export]
	public StatModifier StatSimpleModifier2 { get; private set; }

	[Export]
	public StatModifier StatExpressionModifier1 { get; private set; }
	[Export]
	public StatModifier StatExpressionModifier2 { get; private set; }
	[Export]
	public StatModifier StatExpressionModifier3 { get; private set; }


	[Export]
	public FigureModifier FigureModifier1 { get; private set; }
	[Export]
	public FigureModifier FigureModifier2 { get; private set; }
	#endregion
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
		Example6();

		Print("\n\n\n<<<=====End examples=====>>>");
    }


	private void Example1()
	{
		Print($"\n\n\n=====Running example #1 ({nameof(Stat)}s; {nameof(Modifier)} attachment/detachment)=====");

		Print($"\n({nameof(Stat)}s inherit from {nameof(ModifierAttachable)}, so they can have {nameof(Modifier)}s attached to them--specifically for {nameof(Stat)}s, {nameof(StatModifier)}s!)");

		foreach (var __stat in Ex1SimpleStats)
		{
            PrintValue($"\nStarting on a new stat, \"{__stat.Name}!\"", __stat);

			ModifierInstance __simpleMod1Instance = __stat.AttachModifier(StatSimpleModifier1);
            PrintValue($"Added {nameof(StatSimpleModifier1)} to the stat with a level of 1!", __stat);

			__stat.AttachModifier(StatSimpleModifier2);
            PrintValue($"Added {nameof(StatSimpleModifier2)} to the stat with a level of 1!", __stat);

			__stat.TryDetachModifierInstance(__simpleMod1Instance);
            PrintValue($"Removed the {nameof(StatSimpleModifier1)} instance!", __stat);
		}

		Print("\n=====End example #1!=====");
	}


	private void Example2() 
	{
		Print($"\n\n\n=====Running example #2 (reusing an instance; {nameof(StatFlatAdditiveModifierStrategy)} and {nameof(StatPercentAdditiveModifierStrategy)} leveling)=====");

		ModifierInstance __instance = new(StatSimpleModifier1);
		Print($"\nCreated a new instance of {nameof(StatSimpleModifier1)} at level 1!");

		foreach (var __stat in Ex2SimpleStats) 
		{
            PrintValue($"\nStarting on a new stat, \"{__stat.Name}!\"", __stat);

			__stat.TryAttachModifierInstance(__instance);
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
		Print($"\n\n\n=====Running example #3 ({nameof(StatExpressionModifierStrategy)})=====");

		Print($"\n\n--Running example using {nameof(StatExpressionModifier1)}...");
		__RunWithInstance(StatExpressionModifier1);

		Print($"\n\n--Running example using {nameof(StatExpressionModifier2)}...");
		__RunWithInstance(StatExpressionModifier2);

		Print($"\n\n--Running example using {nameof(StatExpressionModifier3)}...");
		__RunWithInstance(StatExpressionModifier3);

		Print("\n=====End example #3!=====");


		void __RunWithInstance(StatModifier modifier)
		{
			foreach(var __stat in Ex3SimpleStats)
			{
				PrintValue($"\nStarting on a clean stat, \"{__stat.Name}!\"", __stat);

				ModifierInstance __instance = new(modifier);
				Print("Created a new instance of the expression mod!");

				__stat.TryAttachModifierInstance(__instance);
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
		Print($"\n\n\n=====Running example #4 ({nameof(ExpressionStat)})=====");

		PrintValues(
			"\n",
			Ex4ExpressionStat1,
			Ex4ExpressionStat2,
			Ex4ExpressionStat3
		);

		Ex4SimpleStat1.AttachModifier(StatSimpleModifier1);
		PrintValues(
			$"\nAttached {nameof(StatSimpleModifier1)} to {nameof(Ex4SimpleStat1)}!",
			Ex4SimpleStat1,
			Ex4ExpressionStat1,
			Ex4ExpressionStat2,
			Ex4ExpressionStat3
		);

		Ex4ExpressionStat1.AttachModifier(StatSimpleModifier2);
		PrintValues(
			$"\nAttached {nameof(StatSimpleModifier2)} to {nameof(Ex4ExpressionStat1)}!",
			Ex4ExpressionStat1,
			Ex4ExpressionStat2,
			Ex4ExpressionStat3
		);

		Print("\n=====End example #4!=====");
	}


	private void Example5()
	{
		Print($"\n\n\n=====Running example #5 ({nameof(GlobalModifier)})=====\n");

		__PrintContainers();

		GlobalModifier __mod1 = StatHub.CreateAndAddGlobalModifier(Ex5StatModifier1);
		Print($"\nAdded new global modifier of {nameof(Ex5StatModifier1)}!");
		__PrintContainers();
		StatHub.RemoveGlobalModifier(__mod1);
		Print("Removed the global modifier!");

		GlobalModifier __mod2 = StatHub.CreateAndAddGlobalModifier(Ex5StatModifier2);
		Print($"\nAdded new global modifier of {nameof(Ex5StatModifier2)}!");
		__PrintContainers();
		StatHub.RemoveGlobalModifier(__mod2);
		Print("Removed the global modifier!");

		GlobalModifier __mod3 = StatHub.CreateAndAddGlobalModifier(Ex5StatModifier3);
		Print($"\nAdded new global modifier of {nameof(Ex5StatModifier3)}!");
		__PrintContainers();
		StatHub.RemoveGlobalModifier(__mod3);
		Print("Removed the global modifier!");

		Print("\n=====End example #5!=====");


        void __PrintContainers()
        {
            PrintValues("Container 1:", Ex5Container1);
			PrintValues("Container 2:", Ex5Container2);
        }
    }


	private void Example6()
	{
		Print($"\n\n\n=====Running example #6 ({nameof(Figure)})=====\n");

		PrintValue(Ex6Figure);

		Ex6Figure += 2;
		PrintValue($"Attempted to add 2 to {nameof(Ex6Figure)}!", Ex6Figure);

		Ex6Figure *= -5000;
		PrintValue($"Attempted to multiply {nameof(Ex6Figure)} by -5000!", Ex6Figure);

		Ex6Figure.Default();
		PrintValue($"Reset {nameof(Ex6Figure)} to its default value!", Ex6Figure); 


		Ex6Figure.AttachModifier(FigureModifier1);
		PrintValue($"\nAttached {nameof(FigureModifier1)} to {nameof(Ex6Figure)}! No value change should occur yet!", Ex6Figure);

		Ex6Figure++;
		PrintValue($"Attempted to add 1 to {nameof(Ex6Figure)}!", Ex6Figure);

		Ex6Figure--;
		PrintValue($"Attempted to subtract 1 from {nameof(Ex6Figure)}!", Ex6Figure);

		Ex6Figure.TryDetachModifier(FigureModifier1);
		PrintValue($"Removed the instance of {nameof(FigureModifier1)} from {nameof(Ex6Figure)}! No change should occur yet!", Ex6Figure);


		Ex6Figure.AttachModifier(FigureModifier2);
		PrintValue($"\nAttached {nameof(FigureModifier2)} to {nameof(Ex6Figure)}! No value change should occur yet!", Ex6Figure);

		Ex6Figure.Add(1, Ex6FigureValueEditor1);
		PrintValue($"Attempted to add 1 to {nameof(Ex6Figure)} with an editor of {nameof(Ex6FigureValueEditor1)}!", Ex6Figure);

		Ex6Figure--;
		PrintValue($"Attempted to subtract 1 from {nameof(Ex6Figure)}!", Ex6Figure);

		Ex6Figure.Subtract(1, Ex6FigureValueEditor2);
		PrintValue($"Attempted to subtract 1 from {nameof(Ex6Figure)} with an editor of {nameof(Ex6FigureValueEditor2)}!", Ex6Figure);

		Print("\n=====End example #6!=====");
	}


	#region Helpers
	private static void PrintValues(string context, HubContainer container)
	{
		Print(context);
		PrintValues(container);
	}
	private static void PrintValues(HubContainer container)
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


	private static void PrintValue(string context, Figure figure)
	{
		Print(context);
		PrintValue(figure);
	}
	private static void PrintValue(Figure figure)
	{
		Print($"> Figure \"{figure.Name}\" value: {figure} (bounds: {figure.Minimum}, {figure.Maximum})");
	}
	#endregion
}
