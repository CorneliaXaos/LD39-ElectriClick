using UnityEngine;
using UnityEngine.Assertions;

public class World : MonoBehaviour
{

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

	public double CompetitionChargeRate {
		get { return competitionChargeRate * InflationRate; }
	}

	public float CurrentTimeRaw { // this might not get used
		get { return elapsedTime; }
	}

	public float CurrentTimeYears {
		get { return elapsedTime / secondsPerYear + 1F; }
	}

	public bool Paused { // can be used to pause the world.  Useful for adding in a "pause menu" etc.
		get { return paused; }
		set { paused = value; }
	}

	#endregion

	#region Unity Serialization

	[Header ("Population")]
	[SerializeField]
	/// <summary>
	/// The initial population.
	/// </summary>
	private long initialPopulation = 10000L;
	[SerializeField]
	/// <summary>
	/// The population growth rate in percent.
	/// </summary>
	private float populationGrowthRate = 1.68F;

	[Header ("Energy Demand")]
	[SerializeField]
	/// <summary>
	/// The initial demand per person. (Watts / person / year)
	/// </summary>
	private double initialDemandPerPerson = 0.015D;
	[SerializeField]
	/// <summary>
	/// The demand growth rate in percent.
	/// </summary>
	private float demandGrowthRate = 0.5F;

	[Header ("Economics")]
	[SerializeField]
	/// <summary>
	/// The base inflation value.
	/// Multiplied by dollars to determine differential over time.
	/// </summary>
	private float baseInflation = 1F;
	[SerializeField]
	/// <summary>
	/// The inflation growth rate in percent.
	/// </summary>
	private float inflationGrowthRate = 1F;
	[SerializeField]
	/// <summary>
	/// The base energy costs.
	/// Or.. how much does the competition charge on average.
	/// </summary>
	private float baseEnergyCosts = 25F;
	[SerializeField]
	/// <summary>
	/// Energy Depreciation Rate, value is in percentage.
	/// It's a "growth" rate.. but really it only mimics competition growth.  It fakes it
	/// by decreasing how much energy costs are.  So.. if you play the game long enough
	/// your energy is worth nothing as the value of energy depreciates over time.
	/// </summary>
	private float competitionGrowthRate = -5F;

	[Header ("Timing")]
	[SerializeField]
	/// <summary>
	/// The seconds per year.
	/// i.e. how many seconds go by causes a change in "years"
	/// </summary>
	private float secondsPerYear = 5F;

	#endregion

	#region Internals

	private double population;
	private double currentDemand;
	private double inflationRate;
	private double competitionChargeRate;
	private float elapsedTime;
	private bool paused;

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
		Reset ();
	}

	private void Update ()
	{ // we'll try per frame updating of "Time"
		if (!paused) {
			elapsedTime += Time.deltaTime;

			// Extrapolate our world-based values
			population = Extrapolate (initialPopulation, populationGrowthRate / 100F);
			currentDemand = Extrapolate (initialDemandPerPerson, demandGrowthRate / 100F);
			inflationRate = Extrapolate (baseInflation, inflationGrowthRate / 100F);
			competitionChargeRate = Extrapolate (baseEnergyCosts, competitionGrowthRate / 100F);
		}
	}

	#endregion

	#region Public Methods

	public void Reset ()
	{ // just so we can start over..
		population = initialPopulation;
		currentDemand = initialDemandPerPerson;
		inflationRate = baseInflation;
		competitionChargeRate = baseEnergyCosts;

		// un-needed.. but it makes me feel good to "initialize" everything
		elapsedTime = 0.0F;
		paused = false;
	}

	#endregion

	#region Private Methods

	// this static one almost aught to be an extension method of Mathf
	private static double Extrapolate (double principal, float rate, float time)
	{
		return principal * Mathf.Exp (rate * time);
	}

	private double Extrapolate (double principal, float rate)
	{
		return Extrapolate (principal, rate, elapsedTime / secondsPerYear);
	}

	#endregion
}
