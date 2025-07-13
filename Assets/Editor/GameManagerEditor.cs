using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GUIStyle boxStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        GameManager gm = (GameManager)target;
        if (gm.Players != null && gm.Players.Length > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("🧍 Players Attributes Overview", EditorStyles.boldLabel);

            for (int i = 0; i < gm.Players.Length; i++)
            {
                var player = gm.Players[i];
                if (player == null) continue;

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField($"Player {i + 1}", EditorStyles.boldLabel);
                DrawDictionary("🔹 Current Attributes", player.currentAttbs);
                DrawDictionary("🔸 Required Attributes", player.requiredAttbs);
                DrawDictionary("🔻 Sinking Attributes", player.sinkingAttbs);
                DrawDictionary("💰 Bidding Attributes", player.biddingAttbs);

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        // Ensure inspector refreshes in Edit Mode
        if (!Application.isPlaying)
            EditorApplication.QueuePlayerLoopUpdate();
    }

    private void DrawDictionary(string label, Dictionary<Attributes, int> dict)
    {
        if (dict == null || dict.Count == 0)
        {
            EditorGUILayout.LabelField(label + ": (Empty)", EditorStyles.miniLabel);
            return;
        }

        EditorGUILayout.LabelField(label, EditorStyles.miniBoldLabel);
        EditorGUI.indentLevel++;
        foreach (var kvp in dict)
        {
            EditorGUILayout.LabelField(kvp.Key.ToString(), kvp.Value.ToString());
        }
        EditorGUI.indentLevel--;
    }
}
