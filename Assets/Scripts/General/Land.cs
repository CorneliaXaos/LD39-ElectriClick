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

	private List<ElectricGenerator> generators;
	//private List<GeneratorDisplay> displays;
	private int landSize;

	#endregion

	#region Accessors

	public double LandCost {
		get { return baseLandCost * landSize * landCostMultiplier; }
	}

	public int LandSize {
		get { return landSize; }
	}

	public ReadOnlyCollection<ElectricGenerator> Generators {
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

	#region Private Methods

	/*
	private GeneratorDisplay createDisplay() {
		GameObject instance = Instantiate (generatorDisplayPrefab, content.transform, false);
		return instance.GetComponent<GeneratorDisplay> ();
	}
	*/
	private void reset ()
	{
		landSize = initialLandSize;
		generators = new List<ElectricGenerator> ();
		//displays = new List<GeneratorDisplay>();
		foreach (ElectricGenerator generator in initialGenerators) {
			generators.Add (generator);
			/* 
			GeneratorDisplay display = createDisplay();
			display.Bind(generator);
			displays.Add(display);
			*/
		}

	}

	#endregion
}
