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
        private static Object _startScene;
        public const string StartScenePathKey = "StartScenePathKey";
        private static Object _testScene;
        public const string TestScenePathKey = "TestScenePathKey";

        [MenuItem("Mogze/Start Scene Picker")]
        static void Init()
        {
            GetWindow<EditorBasics>().Show();
            if (PlayerPrefs.HasKey(StartScenePathKey))
            {
                _startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(PlayerPrefs.GetString(StartScenePathKey));
            }

            if (PlayerPrefs.HasKey(TestScenePathKey))
            {
                _testScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(PlayerPrefs.GetString(TestScenePathKey));
            }
        }

        void OnGUI()
        {
            var sceneAsset = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Start Scene"), _startScene, typeof(SceneAsset), false);
            _startScene = sceneAsset;
            var startScenePath = AssetDatabase.GetAssetPath(sceneAsset);
            
            var testSceneAsset = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Test Scene"), _testScene, typeof(SceneAsset), false);
            _testScene = testSceneAsset;
            var testScenePath = AssetDatabase.GetAssetPath(testSceneAsset);

            if (GUILayout.Button("Save"))
            {
                if (!string.IsNullOrEmpty(startScenePath))
                {
                    PlayerPrefs.SetString(StartScenePathKey, startScenePath);
                }
                else
                {
                    PlayerPrefs.DeleteKey(StartScenePathKey);
                }

                if (!string.IsNullOrEmpty(testScenePath))
                {
                    PlayerPrefs.SetString(TestScenePathKey, testScenePath);
                }
                else
                {
                    PlayerPrefs.DeleteKey(TestScenePathKey);
                }
                PlayerPrefs.Save();
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
    
    [InitializeOnLoad]
    public class TestSceneButton
    {
        static TestSceneButton()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
    
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
        }
    
        [MenuItem("Mogze/Play Test Scene &t")] // Alt+T shortcut
        private static void PlayTestScene()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
        
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(PlayerPrefs.GetString(EditorBasics.TestScenePathKey));
                EditorSceneManager.playModeStartScene = sceneAsset;
                EditorApplication.isPlaying = true;
            }
        }
    }
}