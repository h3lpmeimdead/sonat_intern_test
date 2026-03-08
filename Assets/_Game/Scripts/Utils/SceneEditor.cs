using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneOpenerWindow : EditorWindow
{
    [MenuItem("Tools/Scene Opener %#o")] // Shortcut Ctrl/Cmd + Shift + O
    public static void ShowWindow()
    {
        GetWindow<SceneOpenerWindow>("Scene Opener");
    }

    private Vector2 scrollPos;

    private void OnGUI()
    {
        GUILayout.Label("Scenes in Build Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(sceneName, GUILayout.Height(25)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }

                if (!scene.enabled)
                {
                    GUILayout.Label("⚠ Disabled", GUILayout.Width(80));
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}