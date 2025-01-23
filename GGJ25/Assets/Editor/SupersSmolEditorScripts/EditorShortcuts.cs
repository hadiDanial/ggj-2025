using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorShortcuts
{

    //Hotkey for saving changes to prefab without having to click on "Apply"
    [MenuItem("Shortcuts/Save prefab %&s", false)]
    static void ApplyPrefabChanges()
    {
        Debug.Log("Applying changes");

        Object prefabParent = PrefabUtility.GetCorrespondingObjectFromSource(Selection.activeGameObject);

        GameObject gameObject = PrefabUtility.FindValidUploadPrefabInstanceRoot(Selection.activeGameObject);

        PrefabUtility.ReplacePrefab(gameObject, prefabParent, ReplacePrefabOptions.ConnectToPrefab);

    }

    [MenuItem("Shortcuts/Save prefab %&s", true)]
    static bool ValidateApplyPrefabChanges()
    {
        return Selection.activeGameObject != null && PrefabUtility.GetCorrespondingObjectFromSource(Selection.activeGameObject);
    }
    

}
