using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BuildingController : HighlightableController
{
	[SerializeField]
	public int RowCount;
	public int ColCount;

	[HideInInspector]
	[SerializeField]
	public BuildingFootprintRow[] FootprintRows;

	public Canvas ContextualUi;

	public void ToggleMenu(bool showMenu)
	{
		ContextualUi.gameObject.SetActive(showMenu);
	}
}
