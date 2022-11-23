using UnityEditor;
using UnityEngine;

namespace ProjectClone.NonCore
{
    /// <summary>
    /// A simple script to display feedback/star dialog after certain time of project being opened/re-compiled.
    /// Will only pop-up once unless "Remind me next time" are chosen.
    /// Removing this file from project wont effect any other functions.
    /// </summary>
    [InitializeOnLoad]
    public class AskFeedbackDialog
    {
        const int ShowLoadCount = 20;
        const string InitializeOnLoadCountKey = "ProjectClone_InitOnLoadCount", StopShowingKey = "ProjectClone_StopShowFeedBack";
        static AskFeedbackDialog()
        {            
            if (EditorPrefs.HasKey(StopShowingKey)) { return; }

            int initializeOnLoadCount = EditorPrefs.GetInt(InitializeOnLoadCountKey, 0);
            if (initializeOnLoadCount > ShowLoadCount)
            {
                ShowDialog();
            }
            else
            {
                EditorPrefs.SetInt(InitializeOnLoadCountKey, initializeOnLoadCount + 1);
            }
        }

        // [MenuItem("ProjectClone/(Debug)Show AskFeedbackDialog ")]
        private static void ShowDialog()
        {
            int option = EditorUtility.DisplayDialogComplex("Do you like " + ProjectClone.ClonesManager.ProjectName + "?",
                   "Do you like " + ProjectClone.ClonesManager.ProjectName + "?\n" +
                   "If so, please don't hesitate to star it on GitHub and contribute to the project!",
                   "Star on GitHub",
                   "Close",
                   "Remind me next time"
               );

            switch (option)
            {
                // First parameter.
                case 0:
                    Debug.Log("AskFeedbackDialog: Star on GitHub selected");
                    EditorPrefs.SetBool(StopShowingKey, true);
                    EditorPrefs.DeleteKey(InitializeOnLoadCountKey);
                    Application.OpenURL(ExternalLinks.GitHubHome);
                    break;
                // Second parameter.
                case 1:
                    Debug.Log("AskFeedbackDialog: Close and never show again.");
                    EditorPrefs.SetBool(StopShowingKey, true);
                    EditorPrefs.DeleteKey(InitializeOnLoadCountKey);
                    break;
                // Third parameter.
                case 2:
                    Debug.Log("AskFeedbackDialog: Remind me next time");
                    EditorPrefs.SetInt(InitializeOnLoadCountKey, ShowLoadCount - 1);
                    break;
                default:
                    //Debug.Log("Close windows.");
                    break;
            }
        }

        // /// <summary>
        // /// For debug purpose
        // /// </summary>
        // [MenuItem("ProjectClone/(Debug)Delete AskFeedbackDialog keys")]
        // private static void DebugDeleteAllKeys()
        // {
        //     EditorPrefs.DeleteKey(InitializeOnLoadCountKey);
        //     EditorPrefs.DeleteKey(StopShowingKey);
        //     Debug.Log("AskFeedbackDialog keys deleted");
        // }
    }
}