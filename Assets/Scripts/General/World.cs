using UnityEngine;
using UnityEngine.Assertions;

public class World : MonoBehaviour
{

	#region Unity Serialization

	[Header ("Population")]
	[SerializeField]
	private long initialPopulation = 10000L;
	// persons
	[SerializeField]
	private float populationGrowthRate = 1.68F;
	// in percent

	[Header ("Energy Demand")]
	[SerializeField]
	private double initialDemandPerPerson = 0.015D;
	// Watts / year
	[SerializeField]
	private float demandGrowthRate = 0.5F;
	// in percent

	[Header ("Economics")]
	[SerializeField]
	private float baseInflation = 1F;
	// multiplier for values
	[SerializeField]
	private float inflationGrowthRate = 1F;
	// in percent

	[Header ("Timing")]
	[SerializeField]
	private float secondsPerYear = 5F;
	// i.e. how many seconds go by causes a change in "years"

	#endregion

	#region Internals

	private double population;
	private double currentDemand;
	private double inflationRate;
	private float elapsedTime;
	private bool paused;

	#endregion

	#region Accessors

	public double Population {
		get { return population; }
	}

	public double DemandRaw { // this might not get used
		get { return currentDemand; }
	}

	public double DemandTotal { // i.e. sum of demand applied to all persons
		get { return population * currentDemand; }
	}

	public double InflationRate {
		get { return inflationRate; }
	}

	public float CurrentTimeRaw { // this might not get used
		get { return elapsedTime; }
	}

	public float CurrentTimeYears {
		get { return elapsedTime / secondsPerYear; }
	}

	public bool Paused { // can be used to pause the world.  Useful for adding in a "pause menu" etc.
		get { return paused; }
		set { paused = value; }
	}

	#endregion

	#region Unity Lifecycle Methods

	private void Start ()
	{
		// Some quick validation
		Assert.IsTrue (initialPopulation > 0L, "Initial population must be positive!");
		Assert.IsTrue (populationGrowthRate > 0F, "Population growth rate must be positive!");

		Assert.IsTrue (initialDemandPerPerson > 0D, "Demand per person must be positive!");
		Assert.IsTrue (demandGrowthRate > 0F, "Demand growth rate must be positive!");

		Assert.IsTrue (baseInflation >= 1F, "Base inflation must be greater or equal to 1!");
		Assert.IsTrue (inflationGrowthRate > 0F, "Inflation growth rate must be positive!");

		Assert.IsTrue (secondsPerYear > 0F, "Seconds per yer must be positive!");

		// And then initialization
		reset ();
	}

	private void Update ()
	{ // we'll try per frame updating of "Time"
		if (!paused) {
			elapsedTime += Time.deltaTime;

			// Extrapolate our world-based values
			population = extrapolate (initialPopulation, populationGrowthRate / 100F);
			currentDemand = extrapolate (initialDemandPerPerson, demandGrowthRate / 100F);
			inflationRate = extrapolate (baseInflation, inflationGrowthRate / 100F);
		}
	}

	#endregion

	#region Public Methods

	public void reset ()
	{ // just so we can start over..
		population = initialPopulation;
		currentDemand = initialDemandPerPerson;
		inflationRate = baseInflation;

		// un-needed.. but it makes me feel good to "initialize" everything
		elapsedTime = 0.0F;
		paused = false;
	}

	#endregion

	#region Private Methods

	// this static one almost aught to be an extension method of Mathf
	private static double extrapolate (double principal, float rate, float time)
	{
		return principal * Mathf.Exp (rate * time);
	}

	private double extrapolate (double principal, float rate)
	{
		return extrapolate (principal, rate, elapsedTime / secondsPerYear);
	}

	#endregion
}
