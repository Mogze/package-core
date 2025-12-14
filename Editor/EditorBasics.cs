using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Mogze.Core
{
    [InitializeOnLoad]
    public class Startup
    {
        static Startup()
        {
            if (PlayerPrefs.HasKey(EditorBasics.StartScenePathKey))
            {
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(PlayerPrefs.GetString(EditorBasics.StartScenePathKey));
                EditorSceneManager.playModeStartScene = sceneAsset;
            }
        }
    }

    public class EditorBasics : EditorWindow
    {
        public const string StartScenePathKey = "StartScenePathKey";

        [MenuItem("Mogze/Start Scene Picker")]
        static void Init()
        {
            GetWindow<EditorBasics>().Show();
            if (PlayerPrefs.HasKey(StartScenePathKey))
            {
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(PlayerPrefs.GetString(StartScenePathKey));
                EditorSceneManager.playModeStartScene = sceneAsset;
            }
        }

        void OnGUI()
        {
            var sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Start Scene"), EditorSceneManager.playModeStartScene, typeof(SceneAsset), false);
            EditorSceneManager.playModeStartScene = sceneAsset;
            var startScenePath = AssetDatabase.GetAssetPath(sceneAsset);

            if (GUILayout.Button("Save"))
            {
                if (!string.IsNullOrEmpty(startScenePath))
                {
                    PlayerPrefs.SetString(StartScenePathKey, startScenePath);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.DeleteKey(StartScenePathKey);
                    PlayerPrefs.Save();
                }
            }
        }

        // This should be somewhere else
        [MenuItem("Mogze/Clear All")]
        static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}