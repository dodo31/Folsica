using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingController), true)]
public class BuildingFootprintEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		BuildingController buildingController = target as BuildingController;

		if (buildingController.RowCount >= 0
		 && buildingController.ColCount >= 0)
		{
			SerializedProperty FootprintRowProperty = serializedObject.FindProperty("FootprintRows");
			FootprintRowProperty.arraySize = buildingController.RowCount;

			for (int rowIndex = 0; rowIndex < buildingController.RowCount; rowIndex++)
			{
				SerializedProperty footPrintRowProperty = FootprintRowProperty.GetArrayElementAtIndex(rowIndex);
				SerializedProperty footPrintRowArrayProperty = footPrintRowProperty.FindPropertyRelative("cells");
				footPrintRowArrayProperty.arraySize = buildingController.ColCount;

				GUILayout.BeginHorizontal();

				for (int colIndex = 0; colIndex < buildingController.ColCount; colIndex++)
				{
					SerializedProperty footprintCellProperty = footPrintRowArrayProperty.GetArrayElementAtIndex(colIndex);
					EditorGUILayout.PropertyField(footprintCellProperty, GUIContent.none, GUILayout.Width(18));
				}

				GUILayout.EndHorizontal();
			}
		}

		serializedObject.ApplyModifiedProperties();
	}
}