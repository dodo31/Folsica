using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public InputController InputController;

	public CameraController CameraController;

	protected void Start()
	{
		InputController.OnBorderHit += CameraController.MoveLaterally;
        InputController.OnZoom += CameraController.SetZoom;
	}

	protected void Update()
	{

	}
}
