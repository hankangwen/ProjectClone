using UnityEngine;
using UnityEditor;

namespace ProjectClone
{
    /// <summary>
    ///Clones manager Unity editor window
    /// </summary>
	public class ClonesManagerWindow : EditorWindow
    {
        [MenuItem("ProjectClone/Clones Manager", priority = 0)]
        private static void InitWindow()
        {
            ClonesManagerWindow window = (ClonesManagerWindow)EditorWindow.GetWindow(typeof(ClonesManagerWindow));
            window.titleContent = new GUIContent("Clones Manager");
            window.Show();
        }

        private void OnGUI()
        {
            
            
            // if (GUILayout.Button("Open in New Editor"))
            // {
            //     string createPath = ClonesManager.GetCurrentProjectPath() + "Link"; 
            //     string targetPath = ClonesManager.GetCurrentProjectPath();
            //     // string cmd = string.Format("Tools/junction.exe '{0}' '{1}'", createPath, targetPath);
            //     // string cmd = "Tools/junction.exe 'E:/GitHub/ProjectCloneLink' 'E:/GitHub/ProjectClone'";
            //     
            //     // string cmd = "mklink /j \"E:/GitHub/ProjectCloneLink/Assets\" \"E:/GitHub/ProjectClone/Assets\"";
            //     
            //     Debug.Log("1. Create ProjectCloneLink");
            //     ClonesManager.RunCmd("mklink /j \"E:/GitHub/ProjectCloneLink/Assets\" \"E:/GitHub/ProjectClone/Assets\"");
            //     ClonesManager.RunCmd("mklink /j \"E:/GitHub/ProjectCloneLink/ProjectSettings\" \"E:/GitHub/ProjectClone/ProjectSettings\"");
            //     ClonesManager.RunCmd("mklink /j \"E:/GitHub/ProjectCloneLink/Packages\" \"E:/GitHub/ProjectClone/Packages\"");
            //     // ClonesManager.RunCmd("mklink /j \"E:/GitHub/ProjectCloneLink/output\" \"E:/GitHub/ProjectClone/output\"");
            // }
            
            if (GUILayout.Button("Add new clone"))
            {
                ClonesManager.CreateCloneFromCurrent();
            }
        }
    }
}
