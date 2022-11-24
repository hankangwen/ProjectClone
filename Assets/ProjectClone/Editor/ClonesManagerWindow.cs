using System.IO;
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
            ClonesManagerWindow window = (ClonesManagerWindow) EditorWindow.GetWindow(typeof(ClonesManagerWindow));
            window.titleContent = new GUIContent("Clones Manager");
            window.Show();
        }

        /// <summary>
        /// For storing the scroll position of clones list
        /// </summary>
        Vector2 _clonesScrollPos;

        private void OnGUI()
        {
            // If it is a clone project...
            if (ClonesManager.IsClone())
            {
                //Find out the original project name and show the help box
                string originalProjectPath = ClonesManager.GetOriginalProjectPath();
                if (originalProjectPath == string.Empty)
                {
                    // If original project cannot be found, display warning message.
                    EditorGUILayout.HelpBox(
                        "This project is a clone, but the link to the original seems lost.\nYou have to manually open the original and create a new clone instead of this one.\n",
                        MessageType.Warning);
                }
                else
                {
                    // If original project is present, display some usage info.
                    EditorGUILayout.HelpBox(
                        "This project is a clone of the project '" + Path.GetFileName(originalProjectPath) + "'.\nIf you want to make changes the project files or manage clones, please open the original project through Unity Hub.",
                        MessageType.Info);
                }

                //Clone project custom argument.
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Arguments", GUILayout.Width(70));
                if (GUILayout.Button("?", GUILayout.Width(20)))
                {
                    Application.OpenURL(ExternalLinks.CustomArgumentHelpLink);
                }
                GUILayout.EndHorizontal();

                string argumentFilePath = Path.Combine(ClonesManager.GetCurrentProjectPath(), ClonesManager.ArgumentFileName);
                //Need to be careful with file reading / writing since it will effect the deletion of
                //  the clone project(The directory won't be fully deleted if there's still file inside being read or write).
                //The argument file will be deleted first at the beginning of the project deletion process
                //to prevent any further being read and write.
                //Will need to take some extra cautious if want to change the design of how file editing is handled.
                if (File.Exists(argumentFilePath))
                {
                    string argument = File.ReadAllText(argumentFilePath, System.Text.Encoding.UTF8);
                    string argumentTextAreaInput = EditorGUILayout.TextArea(argument,
                        GUILayout.Height(50),
                        GUILayout.MaxWidth(300)
                    );
                    File.WriteAllText(argumentFilePath, argumentTextAreaInput, System.Text.Encoding.UTF8);
                }
                else
                {
                    EditorGUILayout.LabelField("No argument file found.");
                }
            }
            else // If it is an original project...
            {
                var cloneProjectsPath = ClonesManager.GetCloneProjectsPath();
                if (cloneProjectsPath.Count > 0) //Is clone created.
                {
                    GUILayout.BeginVertical("HelpBox"); //HelpBox
                    GUILayout.Label("Clones of this Project");

                    //List all clones
                    _clonesScrollPos = EditorGUILayout.BeginScrollView(_clonesScrollPos);
                    for (int i = 0; i < cloneProjectsPath.Count; i++)
                    {
                        GUILayout.BeginVertical("GroupBox"); //GroupBox
                        string cloneProjectPath = cloneProjectsPath[i];
                        
                        bool isOpenInAnotherInstance = ClonesManager.IsProjectRunning(cloneProjectPath);
                        
                        if (isOpenInAnotherInstance == true)
                            EditorGUILayout.LabelField("Clone " + i + " (Running)", EditorStyles.boldLabel);
                        else
                            EditorGUILayout.LabelField("Clone " + i);
                        
                        
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.TextField("Clone project path", cloneProjectPath, EditorStyles.textField);
                        if (GUILayout.Button("View Folder", GUILayout.Width(80)))
                        {
                            ClonesManager.OpenProjectInFileExplorer(cloneProjectPath);
                        }
                        GUILayout.EndHorizontal();
                        
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Arguments", GUILayout.Width(70));
                        if (GUILayout.Button("?", GUILayout.Width(20)))
                        {
                            Application.OpenURL(ExternalLinks.CustomArgumentHelpLink);
                        }
                        GUILayout.EndHorizontal();
                        
                        string argumentFilePath = Path.Combine(cloneProjectPath, ClonesManager.ArgumentFileName);
                        //Need to be careful with file reading/writing since it will effect the deletion of
                        //the clone project(The directory won't be fully deleted if there's still file inside being read or write).
                        //The argument file will be deleted first at the beginning of the project deletion process 
                        //to prevent any further being read and write.
                        //Will need to take some extra cautious if want to change the design of how file editing is handled.
                        if (File.Exists(argumentFilePath))
                        {
                            string argument = File.ReadAllText(argumentFilePath, System.Text.Encoding.UTF8);
                            string argumentTextAreaInput = EditorGUILayout.TextArea(argument,
                                GUILayout.Height(50),
                                GUILayout.MaxWidth(300)
                            );
                            File.WriteAllText(argumentFilePath, argumentTextAreaInput, System.Text.Encoding.UTF8);
                        }
                        else
                        {
                            EditorGUILayout.LabelField("No argument file found.");
                        }
                        
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        
                        
                        EditorGUI.BeginDisabledGroup(isOpenInAnotherInstance);
                        
                        if (GUILayout.Button("Open in New Editor"))
                        {
                            ClonesManager.OpenProject(cloneProjectPath);
                        }
                        
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Delete"))
                        {
                            bool delete = EditorUtility.DisplayDialog(
                                "Delete the clone?",
                                "Are you sure you want to delete the clone project '" + ClonesManager.GetCurrentProject().name + "_clone'?",
                                "Delete",
                                "Cancel");
                            if (delete)
                            {
                                ClonesManager.DeleteClone(cloneProjectPath);
                            }
                        }
                        
                        GUILayout.EndHorizontal();
                        EditorGUI.EndDisabledGroup();
                        GUILayout.EndVertical(); //GroupBox
                        
                    }

                    EditorGUILayout.EndScrollView();

                    if (GUILayout.Button("Add new clone"))
                    {
                        ClonesManager.CreateCloneFromCurrent();
                    }
                    
                    GUILayout.EndVertical(); //HelpBox
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    // If no clone created yet, we must create it.
                    EditorGUILayout.HelpBox("No project clones found. Create a new one!", MessageType.Info);
                    if (GUILayout.Button("Create new clone"))
                    {
                        ClonesManager.CreateCloneFromCurrent();
                    }
                }
            }


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
        }
    }
}