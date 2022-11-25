using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace ProjectClone
{
    public class CloneConfirmWindow : EditorWindow
    {
        List<string> _defaultCopyFolders = new List<string>(2) { "Library", "Packages" };
        List<string> _defaultLinkFolders = new List<string>(4) { "Assets", "ProjectSettings", "AutoBuild", "LocalPackages" };

        List<string> _copyFolders;
        List<string> _linkFolders;

        List<string> copyFolders
        {
            get
            {
                if (_copyFolders == null)
                {
                    _copyFolders = new List<string>(_defaultCopyFolders.Count);
                    foreach (var item in _defaultCopyFolders)
                        copyFolders.Add(sourceProjectPath + item);
                }
                return _copyFolders;
            }
        }

        List<string> linkFolders
        {
            get
            {
                if (_linkFolders == null)
                {
                    _linkFolders = new List<string>(_defaultLinkFolders.Count);
                    foreach (var item in _defaultLinkFolders)
                        _linkFolders.Add(sourceProjectPath + item);
                }
                return _linkFolders;
            }
        }

        string _sourceProjectPath = string.Empty;
        string sourceProjectPath
        {
            get
            {
                if(_sourceProjectPath == string.Empty)
                    _sourceProjectPath = ClonesManager.GetCurrentProjectPath() + @"/";
                return _sourceProjectPath;
            }
        }
        
        public static void InitWindow()
        {
            CloneConfirmWindow window = (CloneConfirmWindow) EditorWindow.GetWindow(typeof(CloneConfirmWindow));
            window.titleContent = new GUIContent("Clones Confirm");
            window.Show();
        }

        const string CopyFoldersName = "Copy Folders";
        const string LinkFoldersName = "Link Folders";
        Vector2 _copyFolderScrollPos;
        Vector2 _linkFolderScrollPos;
        
        private void OnGUI()
        {
            OnFoldersGUI(CopyFoldersName, copyFolders, ref _copyFolderScrollPos);
            OnFoldersGUI(LinkFoldersName, linkFolders, ref _linkFolderScrollPos);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
            }
            if (GUILayout.Button("Confirm"))
            {
                ClonesManager.CreateCloneWithParams(sourceProjectPath, copyFolders, linkFolders);
                this.Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        void OnFoldersGUI(string n, List<string> folders, ref Vector2 scrollPos)
        {
            GUILayout.BeginVertical("HelpBox");
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(n, GUILayout.Width(70));
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical("GroupBox");
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                    for (int i = 0; i < folders.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            // folders[i] = EditorGUILayout.TextField(folders[i]);
                            folders[i] = GUILayout.TextField(folders[i]);
                            if (GUILayout.Button("Select", GUILayout.Width(80)))
                            {
                                folders[i] = SelectFileInFileExplorer();
                            }
                            if (GUILayout.Button("-", GUILayout.Width(80)))
                            {
                                folders.RemoveAt(i);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("+"))
                    {
                        folders.Add("");
                    }
                    if (GUILayout.Button("Reset to default"))
                    {
                        if (n == CopyFoldersName)
                        {
                            _copyFolders = null;
                        }
                        else if(n == LinkFoldersName)
                        {
                            _linkFolders = null;
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }

        string SelectFileInFileExplorer()
        {
            string fileName = EditorUtility.OpenFolderPanel("选择要copy的文件夹", sourceProjectPath, string.Empty);
            if (fileName == string.Empty || fileName.StartsWith(sourceProjectPath))
            {
                return fileName;    
            }
            else
            {
                string message = string.Format("只能选择{0}下的目录，重新选择？", sourceProjectPath);
                if (EditorUtility.DisplayDialog("提示框", message, "确认", "取消"))
                {
                    return SelectFileInFileExplorer();
                }
            }
            return string.Empty;
        }
    }
}