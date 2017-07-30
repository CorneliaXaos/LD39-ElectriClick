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

	public double LandCost {
		get { return baseLandCost * landSize * landCostMultiplier; }
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

	[Header ("UI References")]
	[SerializeField]
	private GameObject content;

	#endregion

	#region Internals

	private List<GeneratorInstance> generators = new List<GeneratorInstance> ();
	private List<GameObject> displayObjects = new List<GameObject> ();
	private List<GeneratorDisplay> displays = new List<GeneratorDisplay> ();
	private int landSize;

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
	}

	private void Update ()
	{
		// We need to "Tick" the things we're managing.. Namely, all the GeneratorInstances
		foreach (GeneratorInstance instance in generators) {
			instance.Tick (Time.deltaTime);
		}
	}

	#endregion

	#region Public Methods

	public void BuyLand ()
	{
		landSize++;
		// Populate additional lands with displays
		for (int index = 0; index < LAND_DIM; index++) {
			generators.Add (null);
			CreateDisplay (null);
		}
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
	}

	public void Sync () // used to sync up generators and displays
	{
		for (int index = 0; index < landSize * LAND_DIM; index++) {
			displays [index].Bind (generators [index]);
		}
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
	}

	#endregion
}
