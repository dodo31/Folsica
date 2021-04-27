using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dummiesman;

public class MainController : MonoBehaviour
{
	public InputController InputController;

	public HudController HudController;
	public CameraController CameraController;

	public TimeController TimeController;

	public BuildingsController BuildingsController;
	public EnemiesController EnemiesController;

	public GlobalHealthController GlobalHealthController;

	public int tileIndex = 1;

	protected void Start()
	{
		InputController.OnBorderHit += CameraController.MoveLaterally;
		InputController.OnZoom += CameraController.SetZoom;
		InputController.OnZoom += (int direction) =>
		{
			BuildingsController.UnselectAllBuildingMenues();
		};

		InputController.OnRightClick += () =>
		{
			if (!BuildingsController.IsMovingObject())
			{
				this.BuildingsController.AddBuilding();
			}
		};

		InputController.OnObjectHovered += this.HoveredObjectLogger;
		InputController.OnObjectClicked += this.DispatchObjectClicked;
		InputController.OnObjectBeginDrag += this.DispatchBeginDrag;
		InputController.OnObjectDragged += this.DispatchDragging;
		InputController.OnObjectEndDrag += this.DispatchEndDrag;

		EnemiesController.OnNewWave += HudController.SetDay;
		EnemiesController.OnNewWave += this.RegisterBestScore;

		EnemiesController.OnEnemyReachedDigger += this.HitDigger;
	}

	protected void Update()
	{
		if (BuildingsController.IsMovingObject())
		{
			BuildingsController.refreshPosition();
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
			BuildingsController.SelectBuildingMenu(buildingController);
			break;
		case "Ground":
			this.ManageBackgroundClicking();
			break;
		}

		if (BuildingsController.IsMovingObject())
		{
			BuildingsController.SelectBuildingMenu(BuildingsController.HeldBuilding);
			BuildingsController.EndMove();
		}
	}

	private void ManageBackgroundClicking()
	{
		if (BuildingsController.SelectedBuilding != null)
		{
			Canvas buildingCanvas = BuildingsController.SelectedBuilding.BuildingUi.ContextualUi;

			if (!InputController.IsCanvasPointed(buildingCanvas))
			{
				BuildingsController.UnselectAllBuildingMenues();
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
			BuildingsController.StartMove(InputController.ActiveObject);
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
			BuildingsController.EndMove();
			break;
		}
	}

	private void HitDigger(EnemyController soruceEnemy)
	{
		GlobalHealthController.DecreaseHealth(soruceEnemy.Health);
	}

	public void RegisterBestScore(int newDay)
	{
		int currentBestScore = PlayerPrefs.GetInt("MOST_SURVIVED_DAYS");
		int lastDay = newDay - 1;

		if (lastDay > currentBestScore)
		{
			PlayerPrefs.SetInt("MOST_SURVIVED_DAYS", lastDay);
		}
	}
}
