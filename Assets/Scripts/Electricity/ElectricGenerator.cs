using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu (fileName = "Generator", menuName = "LD39/Electric Generator", order = 1)]
public class ElectricGenerator : ScriptableObject
{

	#region Scriptable Data

	[Header ("Basic Properties")]
	public string generatorName = "Electric Generator";
	public RuntimeAnimatorController controller;
	/// <summary>
	/// The year available.  Available immeidately if 0.
	/// </summary>
	public float yearAvailable = 0F;

	[Header ("Economics")]
	public float baseCost = 10F;
	/// <summary>
	/// Cost per year.
	/// </summary>
	public float upkeepCost = 1F;
	/// <summary>
	/// Cost per click.
	/// </summary>
	public float operationalCost = 0.25F;

	[Header ("Energy")]
	/// <summary>
	/// The watts per year.
	/// i.e. how much energy does this produce per year
	/// </summary>
	public float wattsPerYear = 2F;
	/// <summary>
	/// Will this run without needing to be clicked?
	/// </summary>
	public bool continuous = false;
	/// <summary>
	/// How many seconds of runtime to queue up per click.
	/// </summary>
	public float runtimePerClick = 1F;
	/// <summary>
	/// Maximum queued runtime.
	/// </summary>
	public float maxRuntime = 30F;

	#endregion

	#region Unity Lifecycle

	private void Start ()
	{
		// We'll validate some things here to make sure data is "correct"

		Assert.AreNotEqual<string> (generatorName, "", "Name cannot be empty!");
		Assert.IsNotNull (controller, "An animation controller is required!");
		Assert.IsTrue (yearAvailable >= 0F, "Year available must be greater than zero!");

		Assert.IsTrue (baseCost > 0F, "Cost must be greater than zero!");
		Assert.IsTrue (upkeepCost >= 0F, "Upkeep cost can't be negative!");
		Assert.IsTrue (operationalCost >= 0F, "Operational cost can't be negative!");

		Assert.IsTrue (wattsPerYear > 0F, "Watts produced must be positive!");
		// no need to assert on the "continuous" value, it's a bool
		Assert.IsTrue (runtimePerClick >= 0F, "Gained runtime must not be negative!");
		Assert.IsTrue (maxRuntime >= 0F, "Max runtime must not be negative!");
	}

	#endregion
}
