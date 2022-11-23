using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ProjectClone
{
    public class ClonesManager
    {
        /// <summary>
        /// Name used for an identifying file created in the clone project directory.
        /// </summary>
        /// <remarks>
        /// (!) Do not change this after the clone was created, because then connection will be lost.
        /// </remarks>
        public const string CloneFileName = ".clone";
        
        /// <summary>
        /// The maximum number of clones
        /// </summary>
        public const int MaxCloneProjectCount = 10;
        
        /// <summary>
        /// Suffix added to the end of the project clone name when it is created.
        /// </summary>
        /// <remarks>
        /// (!) Do not change this after the clone was created, because then connection will be lost.
        /// </remarks>
        public const string CloneNameSuffix = "_clone";
        
        public const string ProjectName = "ProjectClone";
        
        /// <summary>
        /// Get the path to the current unityEditor project folder's info
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentProjectPath()
        {
            return Application.dataPath.Replace("/Assets", "");
        }

        public static string RunCmd(string cmd)
        {
            string result = Cmd.Instance.RunCmd(cmd);
            Debug.Log(result);
            return result;
        }
        
        
        #region Managing clones
        
        /// <summary>
        /// Creates clone from the project currently open in Unity Editor.
        /// </summary>
        /// <returns></returns>
        public static Project CreateCloneFromCurrent()
        {
            if (IsClone())
            {
                Debug.LogError("This project is already a clone. Cannot clone it.");
                return null;
            }
        
            string currentProjectPath = ClonesManager.GetCurrentProjectPath();
            return ClonesManager.CreateCloneFromPath(currentProjectPath);
        }
        
        /// <summary>
        /// Creates clone of the project located at the given path.
        /// </summary>
        /// <param name="sourceProjectPath"></param>
        /// <returns></returns>
        public static Project CreateCloneFromPath(string sourceProjectPath)
        {
            Project sourceProject = new Project(sourceProjectPath);

            string cloneProjectPath = null;

            //Find available clone suffix id
            for (int i = 0; i < MaxCloneProjectCount; i++)
            {
                string originalProjectPath = ClonesManager.GetCurrentProject().projectPath;
                string possibleCloneProjectPath = originalProjectPath + ClonesManager.CloneNameSuffix + "_" + i;

                if (!Directory.Exists(possibleCloneProjectPath))
                {
                    cloneProjectPath = possibleCloneProjectPath;
                    break;
                }
            }

            if (string.IsNullOrEmpty(cloneProjectPath))
            {
                Debug.LogError("The number of cloned projects has reach its limit. Limit: " + MaxCloneProjectCount);
                return null;
            }

            Project cloneProject = new Project(cloneProjectPath);

            Debug.Log("Start cloning project, original project: " + sourceProject + "\n, clone project: \n" + cloneProject);

            ClonesManager.CreateProjectFolder(cloneProject);
            
            //Copy Folders           
            // Debug.Log("Library copy: " + cloneProject.libraryPath);
            // ClonesManager.CopyDirectoryWithProgressBar(sourceProject.libraryPath, cloneProject.libraryPath, 
            //     "Cloning Project Library '" + sourceProject.name + "'. ");
            // Debug.Log("Packages copy: " + cloneProject.libraryPath);
            // ClonesManager.CopyDirectoryWithProgressBar(sourceProject.packagesPath, cloneProject.packagesPath,
            //   "Cloning Project Packages '" + sourceProject.name + "'. ");
            //
            //
            // //Link Folders
            // ClonesManager.LinkFolders(sourceProject.assetPath, cloneProject.assetPath);
            // ClonesManager.LinkFolders(sourceProject.projectSettingsPath, cloneProject.projectSettingsPath);
            // ClonesManager.LinkFolders(sourceProject.autoBuildPath, cloneProject.autoBuildPath);
            // ClonesManager.LinkFolders(sourceProject.localPackages, cloneProject.localPackages);
            //
            // ClonesManager.RegisterClone(cloneProject);

            return cloneProject;
        }
        
        #endregion
        
        #region Creating project folders

        /// <summary>
        /// Creates an empty folder using data in the given Project object
        /// </summary>
        /// <param name="project"></param>
        public static void CreateProjectFolder(Project project)
        {
            string path = project.projectPath;
            Debug.Log("Creating new empty folder at: " + path);
            Directory.CreateDirectory(path);
        }

        // /// <summary>
        // /// Copies the full contents of the unity library. We want to do this to avoid the lengthy re-serialization of the whole project when it opens up the clone.
        // /// </summary>
        // /// <param name="sourceProject"></param>
        // /// <param name="destinationProject"></param>
        // [System.Obsolete]
        // public static void CopyLibraryFolder(Project sourceProject, Project destinationProject)
        // {
        //     if (Directory.Exists(destinationProject.libraryPath))
        //     {
        //         Debug.LogWarning("Library copy: destination path already exists! ");
        //         return;
        //     }
        //
        //     Debug.Log("Library copy: " + destinationProject.libraryPath);
        //     ClonesManager.CopyDirectoryWithProgressBar(sourceProject.libraryPath, destinationProject.libraryPath,
        //         "Cloning project '" + sourceProject.name + "'. ");
        // }

        #endregion
        
        #region Utility methods

        private static bool? isCloneFileExistCache = null;
        
        /// <summary>
        /// Returns true if the project currently open in Unity Editor is a clone.
        /// </summary>
        /// <returns></returns>
        public static bool IsClone()
        {
            if (isCloneFileExistCache == null)
            {
                /// The project is a clone if its root directory contains an empty file named ".clone".
                string cloneFilePath = Path.Combine(ClonesManager.GetCurrentProjectPath(), ClonesManager.CloneFileName);
                isCloneFileExistCache = File.Exists(cloneFilePath);
            }

            return (bool)isCloneFileExistCache;
        }
        
        /// <summary>
        /// Return a project object that describes all the paths we need to clone it.
        /// </summary>
        /// <returns></returns>
        public static Project GetCurrentProject()
        {
            string pathString = ClonesManager.GetCurrentProjectPath();
            return new Project(pathString);
        }
        

        #endregion
    }
}