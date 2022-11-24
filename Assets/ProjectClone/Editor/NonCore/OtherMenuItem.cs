using UnityEditor;
using UnityEngine;

namespace ProjectClone.NonCore
{
    public class OtherMenuItem
    {
        [MenuItem("ProjectClone/GitHub/View this project on GitHub", priority = 10)]
        private static void OpenGitHub()
        {
            Application.OpenURL(ExternalLinks.GitHubHome);
        }

        [MenuItem("ProjectClone/GitHub/View FAQ", priority = 11)]
        private static void OpenFAQ()
        {
            Application.OpenURL(ExternalLinks.FAQ);
        }

        [MenuItem("ProjectClone/GitHub/View Issues", priority = 12)]
        private static void OpenGitHubIssues()
        {
            Application.OpenURL(ExternalLinks.GitHubIssue);
        }
    }
}