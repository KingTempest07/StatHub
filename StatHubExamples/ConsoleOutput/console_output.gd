extends Node
class_name ConsoleOutput


#region EXPORTS
@export var ex1_simple_stats : Array[SimpleStat]
@export var ex2_simple_stats : Array[SimpleStat]
@export var ex3_simple_stats : Array[SimpleStat]

@export var ex4_expression_stat_1 : ExpressionStat
@export var ex4_expression_stat_2 : ExpressionStat
@export var ex4_expression_stat_3 : ExpressionStat
@export var ex4_simple_stat_1 : SimpleStat

@export var ex5_stat_container_1 : StatContainer
@export var ex5_stat_container_2 : StatContainer
@export var ex5_modifier_1 : StatModifier
@export var ex5_modifier_2 : StatModifier
@export var ex5_modifier_3 : StatModifier

#region SHARED
@export var simple_modifier_1 : SimpleModifier
@export var simple_modifier_2 : SimpleModifier

@export var expression_modifier_1 : ExpressionModifier
@export var expression_modifier_2 : ExpressionModifier
@export var expression_modifier_3 : ExpressionModifier
#endregion

#endregion


func _ready():
	print("\n\n\n<<<=====Begin examples=====>>>")

	example1()
	example2()
	example3()
	example4()
	example5()

	print("\n\n\n<<<=====End examples=====>>>")


func example1():
	print("\n\n\n=====Running example no. 1 (modifier application/removal)=====")

	for stat in ex1_simple_stats:
		print_value(stat, "\nStarting on a new stat, \"%s!\"" % stat)

		var simple_mod_1_instance = stat.AttachModifier(simple_modifier_1)
		print_value(stat, "Added simple_modifier_1 to the stat with a level of 1!")

		stat.AttachModifier(simple_modifier_2)
		print_value(stat, "Added simple_modifier_2 to the stat with a level of 1!")

		stat.TryDetachModifierInstance(simple_mod_1_instance)
		print_value(stat, "Removed the simple_modifier_1 instance!")

	print("\n=====End example no. 1!=====")


func example2():
	print("\n\n\n=====Running example no. 2 (reusing an instance; SimpleStatModifier leveling)=====")

	var instance = StatModifierInstance.Create(simple_modifier_1)
	print("\nCreated a new instance of simple_modifier_1 at level 1!")

	for stat in ex2_simple_stats:
		print_value(stat, "\nStarting on a new stat, \"%s!\"" % stat.name)

		stat.AttachModifierInstance(instance)
		print_value(stat, "Applied the instance to \"%s!\"" % stat.name)

	instance.Level = 2.5
	print("\n--Set the instance's level to 2.5!")

	for stat in ex2_simple_stats:
		print_value(stat, "\nRevisiting a stat, \"%s!\"" % stat.name)

	print("\nAll stats with the instance have now been updated!")
	print("\n=====End example no. 2!=====")


func example3():
	var run_with_instance = func (mod : ExpressionModifier):
		for stat in ex3_simple_stats:
			print_value(stat, "\nStarting on a clean stat, \"%s!\"" % stat.name)

			var instance = StatModifierInstance.Create(mod)
			print("Created a new instance of the expression mod!")

			stat.AttachModifierInstance(instance)
			print_value(stat, "Applied the instance to \"%s!\"" % stat.name)

			instance.Level = 5
			print_value(stat, "Changed the instance's level to 5!")

			instance.Level = 13
			print_value(stat, "Changed the instance's level to 13!")

			stat.TryDetachModifierInstance(instance)
			print("Removed the instance from the stat!")
	
	print("\n\n\n=====Running example no. 3 (ExpressionStatModifier)=====")

	print("\n\n--Running example with expression mod 1...")
	run_with_instance.call(expression_modifier_1)

	print("\n\n--Running example with expression mod 2...")
	run_with_instance.call(expression_modifier_2)

	print("\n\n--Running example with expression mod 3...")
	run_with_instance.call(expression_modifier_3)

	print("\n=====End example no. 3!=====")


func example4():
	print("\n\n\n=====Running example no. 4 (ExpressionStat)=====")

	print_stat_values(
		[
			ex4_expression_stat_1, 
			ex4_expression_stat_2, 
			ex4_expression_stat_3
		],
		"\n"
	)

	ex4_simple_stat_1.AttachModifier(simple_modifier_1)
	print_stat_values(
		[
			ex4_simple_stat_1, 
			ex4_expression_stat_1, 
			ex4_expression_stat_2, 
			ex4_expression_stat_3
		],
		"\nAttached simple_modifier_1 to ex4_simple_stat_1!"
	)

	ex4_expression_stat_1.AttachModifier(simple_modifier_2)
	print_stat_values(
		[
			ex4_simple_stat_1, 
			ex4_expression_stat_1, 
			ex4_expression_stat_2, 
			ex4_expression_stat_3
		],
		"\nAttached simple_modifier_2 to ex4_expression_stat_1!"
	)

	print("\n=====End example no. 4!=====")


func example5():
	var print_containers = func():
		print_container_values(ex5_stat_container_1, "Container 1:")
		print_container_values(ex5_stat_container_2, "Container 2:")

	print("\n\n\n=====Running example no. 5 (GlobalModifier)=====\n") 

	print_containers.call()

	var mod1 = StatHubInstance.CreateAndAddGlobalModifier(ex5_modifier_1)
	print("\nAdded new global modifier of Ex5Modifier1!")
	print_containers.call()
	StatHubInstance.RemoveGlobalModifier(mod1)
	print("Removed the global modifier!")

	var mod2 = StatHubInstance.CreateAndAddGlobalModifier(ex5_modifier_2)
	print("\nAdded new global modifier of Ex5Modifier2!")
	print_containers.call()
	StatHubInstance.RemoveGlobalModifier(mod2)
	print("Removed the global modifier!")

	var mod3 = StatHubInstance.CreateAndAddGlobalModifier(ex5_modifier_3)
	print("\nAdded new global modifier of Ex5Modifier3!")
	print_containers.call()
	StatHubInstance.RemoveGlobalModifier(mod3)
	print("Removed the global modifier!")

	print("\n=====End example no. 5!=====")


#region HELPERS
static func print_container_values(container : StatContainer, context : String = ""):
	print_context(context)
	for stat in container.GetStats():
		print_value(stat)

static func print_stat_values(stats : Array[Stat], context : String = ""):
	print_context(context)
	for stat in stats:
		print_value(stat)

static func print_value(stat : Stat, context : String = ""):
	print_context(context)
	print("> Stat \"%s\" value: %s" % [stat.name, stat])

static func print_context(context : String):
	if context != "":
			print(context)
#endregion
