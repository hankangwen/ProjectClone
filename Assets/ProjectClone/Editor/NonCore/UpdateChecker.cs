using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProjectClone.NonCore
{
    /// <summary>
    /// A simple update checker
    /// </summary>
    public class UpdateChecker
    {
        private static string _localVersionText = string.Empty;
        [MenuItem("ProjectClone/Check for update", priority = 20)]
        static void CheckForUpdate()
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                try
                {
                    string latestVersionText = client.DownloadString(ExternalLinks.RemoteVersionURL);
                    Debug.Log("latest version text got: " + latestVersionText);
                    string localVersionText = GetLocalVersionText();
                    Debug.Log("Local version text : " + localVersionText);
                    string messageBody = "Current Version: " + localVersionText +"\n"
                                         +"Latest Version: " + latestVersionText + "\n";
                    var latestVersion = new Version(latestVersionText);
                    var localVersion = new Version(localVersionText);

                    if (latestVersion > localVersion)
                    {
                        Debug.Log("There's a newer version");
                        messageBody += "There's a newer version available";
                        if(EditorUtility.DisplayDialog("Check for update.", messageBody, "Get latest release", "Close"))
                        {
                            Application.OpenURL(ExternalLinks.Releases);
                        }
                    }
                    else
                    {
                        Debug.Log("Current version is up-to-date.");
                        messageBody += "Current version is up-to-date.";
                        EditorUtility.DisplayDialog("Check for update.", messageBody,"OK");
                    }
                    
                }
                catch (Exception exp)
                {
                    Debug.LogError("Error with checking update. Exception: " + exp);
                    EditorUtility.DisplayDialog("Update Error","Error with checking update. \nSee console for more details.",
                     "OK"
                    );
                }
            }
        }

        
        /// <summary>
        /// GetLocalVersionText
        /// </summary>
        /// <returns></returns>
        private static string GetLocalVersionText()
        {
            if (_localVersionText == string.Empty)
            {
                var scriptPath = CallerManager.GetCaller().SourceFilePath;
                string separator = @"\";
                List<string> pathArray = scriptPath.Split(separator).ToList<string>();
                int count = 0;
                for (int i = pathArray.Count()-1; i > 0; i--)
                {
                    count++;
                    if(pathArray[i] == "Editor") break;
                }

                List<string> result = new List<string>();
                for (int i = 0; i < pathArray.Count() - count; i++)
                    result.Add(pathArray[i]);
                string rootPath = string.Join(separator, result.ToArray());
            
                _localVersionText = File.ReadAllText(rootPath + @"\VERSION.txt");
            }
            
            return _localVersionText;
        }
    }
}