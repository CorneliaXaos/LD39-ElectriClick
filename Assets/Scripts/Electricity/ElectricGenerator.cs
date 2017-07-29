using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu (fileName = "Generator", menuName = "LD39/Electric Generator", order = 1)]
public class ElectricGenerator : ScriptableObject
{

	#region Scriptable Data

	[Header ("Basic Properties")]
	public string generatorName = "Electric Generator";
	public Animation idleAnimation;
	public Animation activeAnimation;
	public float yearAvailable = 0F;
	// available immediately if 0

	[Header ("Economics")]
	public float baseCost = 10F;
	// in digital dollars
	public float upkeepCost = 1F;
	// cost per year
	public float operationalCost = 0.25F;
	// cost per click
	public float initialDollarsPerWattYear = 25F;
	// i.e.  $$$ / (W / year)

	[Header ("Energy")]
	public float wattsPerYear = 2F;
	// i.e. how much energy does this produce per year
	public bool continuous = false;
	// Will this run without needing to be clicked?
	public float runtimePerClick = 1F;
	// how many seconds to queue up per click
	public float maxRuntime = 30F;
	// maximum queued runtime

	#endregion

	#region Unity Lifecycle

	private void Start ()
	{
		// We'll validate some things here to make sure data is "correct"

		Assert.AreNotEqual<string> (generatorName, "", "Name cannot be empty!");
		Assert.IsNotNull (idleAnimation, "An idle animation is required!");
		Assert.IsNotNull (activeAnimation, "An active animation is required!");
		Assert.IsTrue (yearAvailable >= 0F, "Year available must be greater than zero!");

		Assert.IsTrue (baseCost > 0F, "Cost must be greater than zero!");
		Assert.IsTrue (upkeepCost >= 0F, "Upkeep cost can't be negative!");
		Assert.IsTrue (operationalCost >= 0F, "Operational cost can't be negative!");
		Assert.IsTrue (initialDollarsPerWattYear > 0F, "Profits per watt must be positive!");

		Assert.IsTrue (wattsPerYear > 0F, "Watts produced must be positive!");
		// no need to assert on the "continuous" value, it's a bool
		Assert.IsTrue (runtimePerClick >= 0F, "Gained runtime must not be negative!");
		Assert.IsTrue (maxRuntime >= 0F, "Max runtime must not be negative!");
	}

	#endregion
}
