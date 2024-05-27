using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationCreator))]
public class AnimationCreatorEditor : Editor
{
    #region Override GUI Methods
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AnimationCreator animationCreator = target as AnimationCreator;

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Animations"))
            animationCreator.CreateAllAnimations();
    }
    #endregion
}
