#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelPreview))]
public class LevelPreviewEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		LevelPreview lp = (LevelPreview) target;
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Load", GUILayout.Width(120))) {
			lp.Reset();
			lp.Load();
		}
		GUILayout.EndHorizontal();
	}
}
#endif