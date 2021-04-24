using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public InputController InputController;

	public CameraController CameraController;

	public BuildingPlacementController buildingPlacementController;

	public PlayPauseButton playPauseButton;
	public AccelerateButton accelerateButton;

	private bool isPlaying;
	private bool isAccelerated;

	protected void Start()
	{
		InputController.OnBorderHit += CameraController.MoveLaterally;
		InputController.OnZoom += CameraController.SetZoom;

		InputController.OnObjectHovered += this.HoveredObjectLogger;
		InputController.OnObjectBeginDrag += this.DispatchBeginDrag;
		InputController.OnObjectDragged += this.DispatchDragging;
		InputController.OnObjectEndDrag += this.DispatchEndDrag;

		isPlaying = true;
		isAccelerated = false;
	}

	protected void Update()
	{

	}

	private void HoveredObjectLogger(GameObject hoveredObject)
	{
		// Debug.Log(hoveredObject.name);
	}

	private void DispatchBeginDrag(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			BuildingController buildingController = draggedObject.GetComponent<BuildingController>();
			buildingController.HighlightAsValid();
			buildingPlacementController.StartMove(InputController.ActiveObject);
			break;
		}
	}

	private void DispatchDragging(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			buildingPlacementController.refreshPosition();
			break;
		}
	}

	private void DispatchEndDrag(GameObject draggedObject)
	{
		switch (draggedObject.tag)
		{
		case "Building":
			BuildingController buildingController = draggedObject.GetComponent<BuildingController>();
			buildingController.HideHighlight();
			buildingPlacementController.EndMove();
			break;
		}
	}

	public void TogglePlayPause()
	{
		if (isPlaying)
		{
			isPlaying = false;
			playPauseButton.SetAsPlay();
			accelerateButton.ToggleInterractable(false);
		}
		else
		{
			isPlaying = true;
			playPauseButton.SetAsPause();
			accelerateButton.ToggleInterractable(true);
		}
	}

	public void ToggleAccelerate()
	{
		if (isAccelerated)
		{
			isAccelerated = false;
			accelerateButton.SetAsNormal();
		}
		else
		{
			if (isPlaying)
			{
				isAccelerated = true;
				accelerateButton.SetAsAccelerate();
			}
		}
	}
}
