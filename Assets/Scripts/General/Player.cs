using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{

	#region Constants

	public static readonly float RANGE_HIGH = 1.25F;
	public static readonly float RANGE_LOW = 0.25F;

	#endregion

	#region Accessors

	public int DollarsPerWattYear {
		get { return dollarsPerWattYear; }
		set {
			if (value > 0)
				dollarsPerWattYear = value;
		}
	}

	public double Finances {
		get { return finances; }
	}

	public float Reputation {
		// value should be [-1, 1]
		get { return reputation; }
	}

	public double SatisfactionRaw {
		get {
			double supply = CalculateSupply ();
			double demand = CalculateDemand ();

			if (supply > demand) {
				return supply / demand;
			} else if (supply == demand) {
				return 0;
			} else {
				return -demand / supply;
			}
		}
	}

	public double SatisfactionClamped {
		get {
			double satisfaction = SatisfactionRaw;
			if (satisfaction > 1D) {
				satisfaction = 1D;
			} else if (satisfaction < -1D) {
				satisfaction = -1D;
			}
			return satisfaction;
		}
	}

	#endregion

	#region Unity Serializations

	[Header ("Finances")]
	[SerializeField]
	private long initialBank = 10000L;
	[SerializeField]
	private int initialDollarsPerWattYear = 1;

	[Header ("Reputation")]
	[SerializeField]
	/// <summary>
	/// The amount to modify reputation by per second.
	/// </summary>
	private float reputationDelta = 0.02F;
	[SerializeField]
	/// <summary>
	/// Used to determine the effects of charging above or below the "competition's"
	/// rate.  This is specifically used to clamp the calculations to a specific range.
	/// </summary>
	private float competitionDifferential = 15F;

	[Header ("Other Game Components")]
	[SerializeField]
	private World world;
	[SerializeField]
	private Land land;

	#endregion

	#region Internals

	private int dollarsPerWattYear;
	private double finances;
	private float reputation;
	private int yearCounter;

	#endregion

	#region Unity Lifecycle Methods

	private void Start ()
	{
		Assert.IsTrue (initialBank >= 0L, "Player can't start with debt!");
		Assert.IsNotNull (world, "World can't be null!");
		Assert.IsNotNull (land, "Land can't be null!");

		Reset ();
	}

	private void Update ()
	{
		CalculateGains (Time.deltaTime);
		CalculateReputation (Time.deltaTime);
	}

	#endregion

	#region Public Methods

	public bool CanMakePurchase (double cost)
	{
		return finances - cost > 0D;
	}

	public bool MakePurchase (double cost)
	{
		if (CanMakePurchase (cost)) {
			finances -= cost;
			return true; // purchase successful
		} else {
			return false; // purchase not made
		}
	}

	public void Reset ()
	{
		finances = initialBank;
		reputation = 0.95F;
		yearCounter = 0;
	}

	#endregion

	#region Private Methods

	private void CalculateGains (float elapsedTime)
	{
		double supply = CalculateSupply ();
		double demand = CalculateDemand ();

		double gains = 0F;
		if (supply >= demand) {
			gains = demand * DollarsPerWattYear;
		} else {
			gains = supply * DollarsPerWattYear;
		}

		// Subtract upkeep costs if any are due at this moment
		if (yearCounter < (int)(world.CurrentTimeYears)) {
			yearCounter++;
			foreach (GeneratorInstance instance in land.Generators) {
				if (instance == null)
					continue;

				gains -= instance.Archtype.upkeepCost * world.InflationRate;
			}
		}

		// finally, add gains, whether positive or negative.
		finances += gains;
	}

	private double CalculateSupply ()
	{
		float supply = 0F;
		foreach (GeneratorInstance instance in land.Generators) {
			if (instance == null)
				continue;
			
			// is the generator running?
			if (instance.Archtype.continuous || instance.Runtime > 0F) {
				supply += instance.Archtype.wattsPerYear * Time.deltaTime;
			}
		}
		return supply;
	}

	private double CalculateDemand ()
	{
		return world.DemandTotal * Time.deltaTime;
	}

	private void CalculateReputation (float elapsedTime)
	{
		float delta = 0F;
		//// First, determine how user costs will affect reputation
		// We need to convert the user's rate to a value within the differential
		double ratio = DollarsPerWattYear / world.CompetitionChargeRate;
		ratio -= (1D - competitionDifferential);
		ratio /= 2D * competitionDifferential;
		float clamped = 1F - Mathf.Clamp01 ((float)ratio);

		// The ratio is now guaranteed to be in the range 0->1, i.e.
		// if the player had costs set really below the competition, it should be 1
		// if the player had costs set really above the competition, it should be 0
		// we can now use this to determine what the player's expected reputation should be
		float expectedReputation = Mathf.Lerp (RANGE_LOW, RANGE_HIGH, clamped);

		// Finally, we can add or subtract a bit of delta depending on where the player falls
		if (reputation > expectedReputation) {
			delta -= reputationDelta;
		} else if (reputation < expectedReputation) {
			delta += reputationDelta;
		}

		//// Determine if user is meeting demand, and affect reputation accordingly
		// we should only consider population met by reputation, as the number of customers
		// is directly related to reputation.
		float populationModifier = ((reputation + 1F) / 2F);
		float diff = CalculateSupply () - CalculateDemand () * populationModifier;
		if (diff > 0F) {
			delta += 2F * reputationDelta;
		} else if (diff < 0F) {
			delta -= 2F * reputationDelta;
		}

		//// Finally, apply reputation delta
		reputation += delta * elapsedTime;
		if (reputation > 1F)
			reputation = 1F;
		if (reputation <= 0F) {
			reputation = 0F;
			////
			/// TODO:  FAIL STATE HERE.. GAME OVER SON..
			/// 
		}
	}

	#endregion
}
