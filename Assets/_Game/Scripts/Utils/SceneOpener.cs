using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode] // so you can use it in Editor
public class SceneOpener : MonoBehaviour
{
    [Tooltip("Add your scenes by name (make sure they are in Build Settings).")]
    [SerializeField] private List<string> scenes = new List<string>();

    public List<string> Scenes => scenes; // expose for editor
}