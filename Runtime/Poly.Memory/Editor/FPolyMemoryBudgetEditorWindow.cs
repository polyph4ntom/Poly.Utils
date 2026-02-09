using System;
using UnityEditor;
using UnityEngine;

namespace Poly.Memory.Editor
{
	public class FPolyMemoryBudgetEditorWindow : EditorWindow
	{
		private Vector2 scroll;
		
		[MenuItem("Tools/Polyphantom/Memory Pool Viewer")]
		public static void Open()
		{
			var window = GetWindow<FPolyMemoryBudgetEditorWindow>("Memory Pools");
			window.Show();
		}
		
		private void OnGUI()
	    {
	        EditorGUILayout.Space();

	        if (!EditorApplication.isPlaying)
	        {
	            EditorGUILayout.HelpBox("Enter Play Mode to see live memory usage.", MessageType.Warning);
	        }

	        EditorGUILayout.Space();

	        scroll = EditorGUILayout.BeginScrollView(scroll);

	        foreach (var pool in FPolyMemoryTracker.GetAllPools())
	        {
	            if (pool.Parent == null)
	            {
	                DrawPoolRecursive(pool, 0);
	            }
	        }

	        EditorGUILayout.EndScrollView();
	    }

	    private void DrawPoolRecursive(FPolyMemoryPool def, int indent)
	    {
	        var budgetMb = def.BudgetBytes / (1024f * 1024f);
	        var usedMb = def.UsedBytes / (1024f * 1024f);
	        var ratio = Mathf.Clamp01(def.UsageRatio);

	        Color barColor = ratio < 0.8f ? Color.darkOliveGreen : (ratio < 1f ? Color.softYellow : Color.indianRed);

	        EditorGUILayout.BeginVertical("box");
	        EditorGUILayout.BeginHorizontal();
	        GUILayout.Space(indent * 16);

	        GUILayout.Label(def.DisplayName, GUILayout.Width(200));

	        Rect barRect = GUILayoutUtility.GetRect(150, 18);
	        EditorGUI.DrawRect(barRect, Color.gray);
	        EditorGUI.DrawRect(new Rect(barRect.x, barRect.y, barRect.width * ratio, barRect.height), barColor);
	        GUI.Label(barRect, $"{usedMb:0.0} / {budgetMb:0.0} MB [{def.UsedBytes} B]", EditorStyles.whiteLabel);

	        EditorGUILayout.EndHorizontal();
	        EditorGUILayout.EndVertical();

	        foreach (var child in def.Children)
	        {
	            DrawPoolRecursive(child, indent + 1);
	        }
		}
	    
	    private void Update()
	    {
		    if (EditorApplication.isPlaying)
		    {
			    Repaint(); // Auto-update usage while playing
		    }
	    }
	}
}
