using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainController : MonoBehaviour
{
	public InputController InputController;

	public CameraController CameraController;

	public TimeController TimeController;

	public BuildingsController buildingsController;

	protected void Start()
	{
		InputController.OnBorderHit += CameraController.MoveLaterally;
		InputController.OnZoom += CameraController.SetZoom;
		InputController.OnZoom += (int direction) =>
		{
			buildingsController.UnselectAllBuildingMenues();
		};

		InputController.OnObjectHovered += this.HoveredObjectLogger;
		InputController.OnObjectClicked += this.DispatchObjectClicked;
		InputController.OnObjectBeginDrag += this.DispatchBeginDrag;
		InputController.OnObjectDragged += this.DispatchDragging;
		InputController.OnObjectEndDrag += this.DispatchEndDrag;
	}

	protected void Update()
	{
		if (buildingsController.IsMovingObject())
		{
			buildingsController.refreshPosition();
		}
	}

	private void HoveredObjectLogger(GameObject hoveredObject)
	{
		// Debug.Log(hoveredObject.name);
	}

	private void DispatchObjectClicked(GameObject clickedObject)
	{
		EventSystem eventSystem = EventSystem.current;

		switch (clickedObject.tag)
		{
		case "Building":
			BuildingController buildingController = clickedObject.GetComponentInParent<BuildingController>();

			if (buildingsController.IsMovingObject())
			{
				buildingController.HideHighlight();
				buildingsController.EndMove();
			}

			buildingsController.SelectBuildingMenu(buildingController);
			break;
		case "Ground":
			this.ManageBackgroundClicking();
			break;
		case "Grid":
			this.ManageBackgroundClicking();
			break;
		}
	}

	private void ManageBackgroundClicking()
	{
		if (buildingsController.SelectedBuilding != null)
		{
			Canvas buildingCanvas = buildingsController.SelectedBuilding.BuildingUi.ContextualUi;

			if (!InputController.IsCanvasPointed(buildingCanvas))
			{
				buildingsController.UnselectAllBuildingMenues();
			}
		}
	}

	private void DispatchBeginDrag(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			BuildingController buildingController = draggedObject.GetComponentInParent<BuildingController>();
			buildingController.HighlightAsNeutral();
			buildingsController.StartMove(InputController.ActiveObject);
			break;
		}
	}

	private void DispatchDragging(GameObject draggedObject)
	{

	}

	private void DispatchEndDrag(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			BuildingController buildingController = draggedObject.GetComponentInParent<BuildingController>();
			buildingController.HideHighlight();
			buildingsController.EndMove();
			break;
		}
	}
}
