using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// This class is used to interact with Unity UI and render a generator instance
/// </summary>
public class GeneratorDisplay : MonoBehaviour
{

	#region Constants

	public static readonly string ANIM_BOOL = "IsActive";

	#endregion

	#region Unity Serializations

	[Header ("Available Panel")]
	[SerializeField]
	private GameObject availablePanel;

	[SerializeField]
	private Dropdown nameDropdown;

	[SerializeField]
	private Animator previewRenderer;

	[SerializeField]
	private Button buyButton;

	[SerializeField]
	private Text purchaseText;

	[SerializeField]
	private Text upkeepText;

	[SerializeField]
	private Text operationalText;

	[Header ("Active Panel")]
	[SerializeField]
	private GameObject activePanel;

	[SerializeField]
	private Text generatorName;

	[SerializeField]
	private Animator activeAnimator;

	[SerializeField]
	private Button sellButton;

	[SerializeField]
	private Text queuedTime;

	#endregion

	#region Internals

	private Land land;
	private GeneratorInstance instance;

	#endregion

	#region Unity Lifecycle Methods

	private void Start ()
	{
		// Validations, no need to "reset" anything.  Wait to be bound.
		Assert.IsNotNull (availablePanel, "Must provide this field!");
		Assert.IsNotNull (nameDropdown, "Must provide this field!");
		Assert.IsNotNull (previewRenderer, "Must provide this field!");
		Assert.IsNotNull (purchaseText, "Must provide this field!");
		Assert.IsNotNull (upkeepText, "Must provide this field!");
		Assert.IsNotNull (operationalText, "Must provide this field!");

		Assert.IsNotNull (activePanel, "Must provide this field!");
		Assert.IsNotNull (generatorName, "Must provide this field!");
		Assert.IsNotNull (activeAnimator, "Must provide this field!");
		Assert.IsNotNull (queuedTime, "Must provide this field!");
	}

	private void Update ()
	{
		if (instance != null) {
			UpdateActiveDisplay ();
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Sets up some callback things.
	/// </summary>
	/// <param name="land">Land.</param>
	public void Initialize (Land land)
	{
		this.land = land;
	}

	public void SetAvailable (ReadOnlyCollection<ElectricGenerator> generators,
	                          bool[] availability)
	{
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();
		for (int index = 0; index < generators.Count; index++) {
			ElectricGenerator generator = generators [index];

			string text;
			if (availability [index]) {
				text = generator.generatorName;
			} else {
				text = "* Year " + generator.yearAvailable + " *";
			}
			options.Add (new Dropdown.OptionData (text));
		}
		nameDropdown.options = options;
	}

	public void Bind (GeneratorInstance instance)
	{
		this.instance = instance;
		availablePanel.SetActive (instance == null);
		activePanel.SetActive (instance != null);

		if (instance != null) {
			activeAnimator.runtimeAnimatorController = instance.Archtype.controller;
			UpdateActiveDisplay ();
		}
	}

	public void BuyInstance ()
	{
		ElectricGenerator generator = land.Archtypes [nameDropdown.value];
		land.BuyGeneratorOnDisplay (generator, this);
	}

	public void SellInstance ()
	{
		land.SellGeneratorOnDisplay (this);
	}

	public void QueueRuntime ()
	{
		if (!instance.Archtype.continuous) {
			instance.AddRuntime (instance.Archtype.runtimePerClick);
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Used to update the active panel.  The available panel is event driven.
	/// </summary>
	private void UpdateActiveDisplay ()
	{
		generatorName.text = instance.Archtype.generatorName;
		activeAnimator.SetBool (ANIM_BOOL,
			instance.Archtype.continuous || instance.Runtime > 0F);
		string queued = "Queued (s): ";
		if (instance.Archtype.continuous) {
			queued += "∞";
		} else {
			queued += "" + ((int)instance.Runtime);
		}
	}

	#endregion
}
