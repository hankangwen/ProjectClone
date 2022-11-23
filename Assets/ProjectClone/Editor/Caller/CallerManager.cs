using UnityEngine;
using System.Runtime.CompilerServices;

namespace ProjectClone
{
    public struct Caller
    {
        public string MemberName;
        public string SourceFilePath;
        public int SourceLineNumber;
    }
    
    /// <summary>
    /// Thanks for https://stackoverflow.com/questions/10960071/how-to-find-path-to-cs-file-by-its-type-in-c-sharp
    /// </summary>
    public static class CallerManager
    {
        public static Caller GetCaller(
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug.LogFormat("member name: {0}, source file path: {1}, source line number: {2}", 
                memberName, sourceFilePath, sourceLineNumber);
            
            Caller caller = new Caller();
            caller.MemberName = memberName;
            caller.SourceFilePath = sourceFilePath;
            caller.SourceLineNumber = sourceLineNumber;
            return caller;
        }
    }
}