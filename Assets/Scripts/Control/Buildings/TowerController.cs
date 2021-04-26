using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : BuildingController
{
	public TowerBaseController Base;
	public TowerCoreController Core;
	public TowerHeadController Head;

	protected void Awake()
	{
		GameObject uiCameraObject = GameObject.FindGameObjectWithTag("UI Camera");
		Camera uiCamera = uiCameraObject.GetComponent<Camera>();
		
		Canvas canvas = this.GetComponentInChildren<Canvas>();
		canvas.worldCamera = uiCamera;
	}

	public void SetBase(GameObject basePrefab)
	{
		this.DestroyStage(Base);

		GameObject newBaseObject = this.CreateStage(basePrefab);
		Base = newBaseObject.GetComponent<TowerBaseController>();

		this.RelocateStages();
	}

	public void SetCore(GameObject corePrefab)
	{
		this.DestroyStage(Core);

		GameObject newCoreObject = this.CreateStage(corePrefab);
		Core = newCoreObject.GetComponent<TowerCoreController>();

		this.RelocateStages();
	}

	public void SetHead(GameObject headPrefab)
	{
		this.DestroyStage(Head);

		GameObject newHeadObject = this.CreateStage(headPrefab);
		Head = newHeadObject.GetComponent<TowerHeadController>();

		this.RelocateStages();
	}

	private void RelocateStages()
	{
		if (Base != null && Core != null && Head != null)
		{
			Base.transform.localPosition = Vector3.zero;

			float corePosY = this.ComputeStagePosition(new TowerStageController[] { Base });
			Core.transform.localPosition = new Vector3(0, corePosY, 0);

			float headPosY = this.ComputeStagePosition(new TowerStageController[] { Base, Core });
			Head.transform.localPosition = new Vector3(0, headPosY, 0);
		}
	}

	public void DestroyStage(TowerStageController stage)
	{
		if (stage != null)
		{
			DestroyImmediate(stage.gameObject, true);
		}
	}

	private GameObject CreateStage(GameObject stageObject)
	{
		stageObject.transform.SetParent(transform);
		return stageObject;
	}

	private float ComputeStagePosition(TowerStageController[] stagesBellow)
	{
		float position = 0;

		foreach (TowerStageController currentStage in stagesBellow)
		{
			if (currentStage != null)
			{
				position += currentStage.Height;
			}
		}

		return position;
	}
}
