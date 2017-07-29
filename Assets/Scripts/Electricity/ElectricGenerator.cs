using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "Generator", menuName = "LD39/Electric Generator", order = 1)]
public class ElectricGenerator : ScriptableObject {

	#region Scriptable Data

	[Header("Basic Properties")]
	public string generatorName = "Electric Generator";
	public Animation idleAnimation;
	public Animation activeAnimation;
	public float yearAvailable = 0F; // available immediately if 0

	[Header("Economics")]
	public float baseCost = 10F; // in digital dollars
	public float upkeepCost = 1F; // cost per year
	public float operationalCost = 0.25F; // cost per click
	public float initialDollarsPerWattYear = 25F; // i.e.  $$$ / (W / year)

	[Header("Energy")]
	public float wattsPerYear = 2F; // i.e. how much energy does this produce per year
	public bool continuous = false; // Will this run without needing to be clicked?
	public float runtimePerClick = 1F; // how many seconds to queue up per click
	public float maxRuntime = 30F; // maximum queued runtime

	#endregion

	#region Unity Lifecycle

	private void Start() {
		// We'll validate some things here to make sure data is "correct"

		Assert.AreNotEqual<string>(generatorName, "");
		// we can't assert on these or unity will flip out when we initially create one of these
		// scriptable objects.. so we just test and fire a warning
		if (idleAnimation == null) {
			Debug.LogWarning ("Electric Generator (" + generatorName + "): Idle Animation is null!", idleAnimation);
		}
		if (activeAnimation == null) {
			Debug.LogWarning ("Electric Generator (" + generatorName + "): Active Animation is null!", activeAnimation);
		}
		Assert.IsTrue (yearAvailable >= 0F);

		Assert.IsTrue (baseCost > 0F);
		Assert.IsTrue (upkeepCost >= 0F); // can be zero
		Assert.IsTrue (operationalCost >= 0F); // can be zero
		Assert.IsTrue (initialDollarsPerWattYear > 0F);

		Assert.IsTrue (wattsPerYear > 0F);
		// no need to assert on the "continuous" value, it's a bool
		Assert.IsTrue (runtimePerClick >= 0F);
		Assert.IsTrue (maxRuntime >= 0F);
	}

	#endregion
}
