using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsController : MonoBehaviour
{
	public GridController Grid;

	public Transform PlayArea;

	private BuildingController selectedBuilding;

	private BuildingController heldBuilding;

	private Vector3 deltaFromObject;

	private bool isMovingObject;

	private Dictionary<BuildingController, Vector3> buildingGridPositions;

	protected void Start()
	{
		selectedBuilding = null;
		heldBuilding = null;

		deltaFromObject = Vector3.zero;
		isMovingObject = false;

		Grid.gameObject.SetActive(false);

		buildingGridPositions = new Dictionary<BuildingController, Vector3>();
	}
	public void AddBuilding(GameObject buildingPrefab)
	{
		Grid.gameObject.SetActive(true);

		GameObject newBuildingObject = Instantiate<GameObject>(buildingPrefab);
		Vector3 spawnScreenPosition = Input.mousePosition - new Vector3(0, 20, 0);
		newBuildingObject.transform.position = this.PointedPosition(spawnScreenPosition);
		newBuildingObject.transform.SetParent(PlayArea);

		BuildingController newBuilding = newBuildingObject.GetComponentInParent<BuildingController>();
		newBuilding.HighlightAsNeutral();
		this.UnselectAllBuildingMenues();

		if (newBuilding is TowerController newTower)
		{
			newTower.SellButton.onClick.AddListener(() =>
			{
				this.SellTower(newTower);
			});
		}

		selectedBuilding = newBuilding;

		if (newBuilding is TowerController tower)
		{
			tower.UpgradeTowerBase("Neutral/");
			tower.UpgradeTowerCore("Neutral/");
			tower.UpgradeTowerHead("Neutral/");
		}

		this.StartMove(newBuildingObject);
	}

	private void SellTower(TowerController towerToRemove)
	{
		// Retribute
		this.RemoveBuilding(towerToRemove);
	}

	public void RemoveBuilding(BuildingController building)
	{
		buildingGridPositions.Remove(building);
		this.DestroyBuilding(building);
	}

	public void StartMove(GameObject objectToMove)
	{
		heldBuilding = objectToMove.GetComponentInParent<BuildingController>();

		Vector3 objectScreenPosition = Camera.main.WorldToScreenPoint(heldBuilding.transform.position);
		Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		deltaFromObject = objectScreenPosition - mouseScreenPosition;

		this.UnselectAllBuildingMenues();
		Grid.gameObject.SetActive(true);

		isMovingObject = true;
	}

	public void refreshPosition()
	{
		Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		Vector3 newObjectScreenPosition = mouseScreenPosition + deltaFromObject;

		Vector3 newObjectPosition = this.PointedPosition(newObjectScreenPosition);

		if (newObjectPosition.x != float.NegativeInfinity)
		{
			Vector3 buildingGridPosition = Grid.FreePositionToGridPosition(newObjectPosition);
			heldBuilding.transform.position = buildingGridPosition;

			if (this.IsBuildingColliding(heldBuilding))
			{
				heldBuilding.HighlightAsInvalid();
			}
			else
			{
				heldBuilding.HighlightAsNeutral();
			}
		}
	}

	public void EndMove()
	{
		Vector3 objectGridPosition = Grid.FreePositionToGridPosition(heldBuilding.transform.position);

		if (!this.IsBuildingColliding(heldBuilding))
		{
			this.PlaceBuilding(objectGridPosition);
		}
		else
		{
			this.CancelPlacement();
		}

		Grid.gameObject.SetActive(false);
		isMovingObject = false;
	}

	private void PlaceBuilding(Vector3 objectGridPosition)
	{
		if (!buildingGridPositions.ContainsKey(heldBuilding))
		{
			buildingGridPositions.Add(heldBuilding, objectGridPosition);
		}
		else
		{
			buildingGridPositions[heldBuilding] = objectGridPosition;
		}
	}

	public void CancelPlacement()
	{
		if (buildingGridPositions.ContainsKey(heldBuilding))
		{
			heldBuilding.transform.position = buildingGridPositions[heldBuilding];
		}
		else
		{
			this.DestroyBuilding(heldBuilding);
		}
	}

	private void DestroyBuilding(BuildingController building)
	{
		DestroyImmediate(building.gameObject);
		heldBuilding = null;
	}

	private Vector3 PointedPosition(Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		RaycastHit[] hits = Physics.RaycastAll(ray);

		if (hits.Length > 0)
		{
			RaycastHit matchingHit = Array.Find(hits, (RaycastHit hit) =>
				hit.transform == Grid.transform
			);

			if (matchingHit.transform != null)
			{
				return matchingHit.point;
			}

			return Vector3.negativeInfinity;
		}
		else
		{
			return Vector3.negativeInfinity;
		}
	}

	public bool IsMovingObject()
	{
		return isMovingObject;
	}

	public void SelectBuildingMenu(BuildingController building)
	{
		this.UnselectAllBuildingMenues();
		building.HighlightAsSelected();
		building.ToggleMenu(true);
		selectedBuilding = building;
	}

	public void UnselectAllBuildingMenues()
	{
		foreach (BuildingController building in buildingGridPositions.Keys)
		{
			building.HideHighlight();
			building.ToggleMenu(false);
		}

		selectedBuilding = null;
	}

	private bool IsBuildingColliding(BuildingController building)
	{
		bool isColliding = false;

		IEnumerator<BuildingController> placedBuildings = buildingGridPositions.Keys.GetEnumerator();

		while (placedBuildings.MoveNext() && !isColliding)
		{
			BuildingController placedBuilding = placedBuildings.Current;
			Vector3 buildingGridPosition = buildingGridPositions[placedBuilding];

			if (building != placedBuilding)
			{
				Vector3 positionOther = building.transform.position;
				int rowCountOther = building.RowCount;
				int colCountOther = building.ColCount;
				BuildingFootprintRow[] buildingFootprintRowsOther = building.FootprintRows;

				Vector3 positionPlaced = placedBuilding.transform.position;
				int rowCountPlaced = placedBuilding.RowCount;
				int colCountPlaced = placedBuilding.ColCount;
				BuildingFootprintRow[] buildingFootprintRowsPlaced = placedBuilding.FootprintRows;

				for (int rowPlaced = 0; rowPlaced < rowCountPlaced && !isColliding; rowPlaced++)
				{
					for (int colPlaced = 0; colPlaced < colCountPlaced && !isColliding; colPlaced++)
					{
						bool isFilledPlaced = buildingFootprintRowsPlaced[rowPlaced].cells[colPlaced];

						if (isFilledPlaced)
						{
							Vector3 cellPositionPlaced = positionPlaced + new Vector3(colPlaced, 0, -rowPlaced);

							for (int rowOther = 0; rowOther < rowCountOther && !isColliding; rowOther++)
							{
								for (int colOther = 0; colOther < colCountOther && !isColliding; colOther++)
								{
									bool isFilledOther = buildingFootprintRowsOther[rowOther].cells[colOther];

									if (isFilledOther)
									{
										Vector3 cellPositionOther = positionOther + new Vector3(colOther, 0, -rowOther);

										if (cellPositionOther.Equals(cellPositionPlaced))
										{
											isColliding = true;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		return isColliding;
	}

	public BuildingController SelectedBuilding { get => selectedBuilding; set => selectedBuilding = value; }
	public BuildingController HeldBuilding { get => heldBuilding; set => heldBuilding = value; }
}
