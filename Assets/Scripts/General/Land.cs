using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

public class Land : MonoBehaviour
{

	#region Constants

	public static readonly int LAND_DIM = 2;

	#endregion

	#region Accessors

	public ReadOnlyCollection<ElectricGenerator> Archtypes {
		get { return new List<ElectricGenerator> (generatorArchtypes).AsReadOnly (); }
	}

	public double LandCost {
		get { return baseLandCost * landSize * landCostMultiplier * world.InflationRate; }
	}

	public int LandSize {
		get { return landSize; }
	}

	public ReadOnlyCollection<GeneratorInstance> Generators {
		get { return generators.AsReadOnly (); }
	}

	#endregion

	#region Unity Serializations

	[Header ("Generator Configuration")]
	[SerializeField]
	private ElectricGenerator[] generatorArchtypes;
	[SerializeField]
	private ElectricGenerator[] initialGenerators;
	[SerializeField]
	private GameObject generatorDisplayPrefab;

	[Header ("Land Configuration")]
	[SerializeField]
	private double baseLandCost = 10D;
	[SerializeField]
	private int initialLandSize = 2;
	[SerializeField]
	private float landCostMultiplier = 1.5F;

	[Header ("Finances")]
	[SerializeField]
	/// <summary>
	/// The sell-back rate for generators in percent.
	/// </summary>
	private float generatorSellRate = 65F;

	[Header ("UI References")]
	[SerializeField]
	private GameObject content;

	[Header ("Other Game Components")]
	[SerializeField]
	private World world;
	[SerializeField]
	private Player player;

	#endregion

	#region Internals

	private List<GeneratorInstance> generators = new List<GeneratorInstance> ();
	private List<GameObject> displayObjects = new List<GameObject> ();
	private List<GeneratorDisplay> displays = new List<GeneratorDisplay> ();
	private int landSize;
	private int lastYear;

	#endregion

	#region Unity Lifecycle Methods

	private void Start ()
	{
		// Data Validation
		Assert.IsFalse (generatorArchtypes.Length == 0, "There must be at least one generator.");
		Assert.IsTrue (initialGenerators.Length <= initialLandSize * LAND_DIM,
			"Number of initial generators will not fit on initial land!");
		Assert.IsNotNull (generatorDisplayPrefab, "Prefab is required!");

		Assert.IsTrue (baseLandCost > 0D, "Cost of land must be positive!");
		Assert.IsTrue (initialLandSize >= 0, "Negative land is impossible!");
		Assert.IsTrue (landCostMultiplier > 1F, "Land must become more expensive!");

		Assert.IsNotNull (content, "Content object required for display prefabs.");

		// Set Things Up
		Reset ();
	}

	private void Update ()
	{
		// We need to "Tick" the things we're managing.. Namely, all the GeneratorInstances
		foreach (GeneratorInstance instance in generators) {
			if (instance == null)
				continue;
			instance.Tick (Time.deltaTime);
		}

		// We'll only update things once a year rather than every frame.
		int currentYear = (int)(world.CurrentTimeYears);
		if (currentYear > lastYear) {
			lastYear = currentYear;
			UpdateDisplay ();
		}
	}

	#endregion

	#region Public Methods

	public void BuyLand ()
	{
		if (!player.MakePurchase (LandCost)) {
			return;
		}

		landSize++;
		// Populate additional lands with displays
		for (int index = 0; index < LAND_DIM; index++) {
			generators.Add (null);
			CreateDisplay (null);
		}

		UpdateDisplay ();
	}

	public void BuyGeneratorOnDisplay (ElectricGenerator generator, GeneratorDisplay display)
	{
		for (int index = 0; index < displays.Count; index++) {
			if (displays [index] == display) {
				// First calculate cost of purchasing specific engine
				double cost = generator.baseCost * world.InflationRate;

				if (player.MakePurchase (cost)) {
					generators [index] = new GeneratorInstance (generator);
					display.Bind (generators [index]);
				}
				return;
			}
		}

		Debug.LogWarning ("Attempted to buy generator for bad display.", display);
	}

	public void SellGeneratorOnDisplay (GeneratorDisplay display)
	{
		for (int index = 0; index < displays.Count; index++) {
			if (displays [index] == display) {
				GeneratorInstance instance = generators [index];
				Assert.IsNotNull (instance, "Attempted to sell null generator!");

				double credit = instance.Archtype.baseCost * world.InflationRate *
				                generatorSellRate / 100F;
				player.Credit (credit);
				generators [index] = null;
				display.Bind (null);
				return;
			}
		}

		Debug.LogWarning ("Attempted to sell generator for bad display.", display);
	}

	public void Reset ()
	{
		landSize = initialLandSize;
		generators = new List<GeneratorInstance> ();
		foreach (GameObject obj in displayObjects) {
			Destroy (obj);
		}
		displayObjects = new List<GameObject> ();
		displays = new List<GeneratorDisplay> ();

		int count = 0;
		foreach (ElectricGenerator generator in initialGenerators) {
			GeneratorInstance instance = (generator != null) ? new GeneratorInstance (generator) : null;
			generators.Add (instance);
			CreateDisplay (instance);
			count++;
		}
		while (count++ < landSize * LAND_DIM) {
			generators.Add (null);
			CreateDisplay (null);
		}
		lastYear = 0;
	}

	#endregion

	#region Private Methods

	private void CreateDisplay (GeneratorInstance instance)
	{
		GameObject displayObject = Instantiate (generatorDisplayPrefab, content.transform, false);
		displayObjects.Add (displayObject);
		GeneratorDisplay display = displayObject.GetComponent<GeneratorDisplay> ();
		displays.Add (display);
		display.Bind (instance);
		display.Initialize (this, world);
	}

	/// <summary>
	/// This is mainly used to tell the Display prefabs which Electric Generators are availble.
	/// </summary>
	private void UpdateDisplay ()
	{
		bool[] available = new bool[Archtypes.Count];
		for (int index = 0; index < Archtypes.Count; index++) {
			available [index] = world.CurrentTimeYears >= Archtypes [index].yearAvailable;
		}

		foreach (GeneratorDisplay display in displays) {
			display.SetAvailable (Archtypes, available);
		}
	}

	#endregion
}
