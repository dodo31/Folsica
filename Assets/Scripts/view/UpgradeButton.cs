using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
	public string StagePath;

	public void Test()
	{
		GameObject baseStagePrefab = Resources.Load<GameObject>("Models/Buildings/Towers/Alien/Heavy/Base");
        GameObject baseStage = Instantiate<GameObject>(baseStagePrefab);

		baseStage.transform.position = Vector3.zero;
		baseStage.transform.localScale = Vector3.one;
	}
}
