using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

public class Land : MonoBehaviour
{

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
	// i.e. if 2: total land is 4, if 3: 9, etc.
	[SerializeField]
	private float landCostMultiplier = 1.5F;
	// Multiplied by level to determine land costs

	[Header ("UI References")]
	[SerializeField]
	private GameObject content;

	#endregion

	#region Internals

	private List<GeneratorInstance> generators = new List<GeneratorInstance> ();
	//private List<GameObject> displayObjects = new List<GameObject> ();
	//private List<GeneratorDisplay> displays = new List<GeneratorDisplay> ();
	private int landSize;

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

	#region Unity Lifecycle Methods

	private void Start ()
	{
		// Data Validation
		Assert.IsFalse (generatorArchtypes.Length == 0, "There must be at least one generator.");
		Assert.IsTrue (initialGenerators.Length <= initialLandSize,
			"Number of initial generators will not fit on initial land!");
		Assert.IsNotNull (generatorDisplayPrefab, "Prefab is required!");

		Assert.IsTrue (baseLandCost > 0D, "Cost of land must be positive!");
		Assert.IsTrue (initialLandSize >= 0, "Negative land is impossible!");
		Assert.IsTrue (landCostMultiplier > 1F, "Land must become more expensive!");

		Assert.IsNotNull (content, "Content object required for display prefabs.");
	}

	private void Update ()
	{
		
	}

	#endregion

	#region Public Methods

	public void Sync () // used to sync up generators and displays
	{
		for (int index = 0; index < landSize; index++) {
			//displays[index].Bind (generators[index]);
		}
	}

	#endregion

	#region Private Methods

	private void Reset ()
	{
		landSize = initialLandSize;
		generators = new List<GeneratorInstance> ();
		/*
		foreach (GameObject object in displayObjects) {
			Destroy(object);
		}
		displayObjects = new List<GameObject>();
		displays = new List<GeneratorDisplay>();
		*/
		foreach (ElectricGenerator generator in initialGenerators) {
			generators.Add (new GeneratorInstance (generator));
			/* 
			GameObject displayObject = Instantiate (generatorDisplayPrefab, content.transform, false);
			displayObjects.Add(displayObject);
			GeneratorDisplay display = displayObject.GetComponent<GeneratorDisplay> ();
			displays.Add(display);
			display.Bind(generator);
			*/
		}

	}

	#endregion
}
