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

	public BuildingUi BuildingUi;

	protected bool isOnMove;

	public void ToggleMenu(bool showMenu)
	{
		BuildingUi.ToggleMenu(showMenu);
		isOnMove = false;
	}

	public bool IsOnMove { get => isOnMove; set => isOnMove = value; }
}
