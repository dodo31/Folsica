using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUi : MonoBehaviour
{
	public Canvas ContextualUi;

	public StageSection SectionBase;
	public StageSection SectionCore;
	public StageSection SectionHead;

	public GameObject UpgradePanelBase;
	public GameObject UpgradePanelCore;
	public GameObject UpgradePanelHead;

	protected void Start()
	{
		SectionBase.SelectUpgradeButton.onClick.AddListener(this.ToggleUpgradePanelBase);
		SectionCore.SelectUpgradeButton.onClick.AddListener(this.ToggleUpgradePanelCore);
		SectionHead.SelectUpgradeButton.onClick.AddListener(this.ToggleUpgradePanelHead);
	}

	public void ToggleMenu(bool showMenu)
	{
		if (!showMenu)
		{
			this.HiddeUpgradePanels();
		}

		ContextualUi.gameObject.SetActive(showMenu);
	}

	public void ToggleUpgradePanelBase()
	{
		this.HiddeUpgradePanels();
		UpgradePanelBase.SetActive(true);
	}

	public void ToggleUpgradePanelCore()
	{
		this.HiddeUpgradePanels();
		UpgradePanelCore.SetActive(true);
	}

	public void ToggleUpgradePanelHead()
	{
		this.HiddeUpgradePanels();
		UpgradePanelHead.SetActive(true);
	}

	private void HiddeUpgradePanels()
	{
		UpgradePanelBase.SetActive(false);
		UpgradePanelCore.SetActive(false);
		UpgradePanelHead.SetActive(false);
	}
}
