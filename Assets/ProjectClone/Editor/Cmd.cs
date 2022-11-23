using System.Diagnostics;

namespace ProjectClone
{
    public class Cmd
    {
        private static Cmd _instance;

        public static Cmd Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Cmd();
                return _instance;
            }
        }

        private Process proc = null;

        private Cmd()
        {
            proc = new Process();
        }

        /// <summary>
        /// 执行CMD语句
        /// </summary>
        /// <param name="cmd">要执行的CMD命令</param>
        public string RunCmd(string cmd)
        {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.WriteLine("exit");
            string outStr = proc.StandardOutput.ReadToEnd();
            proc.Close();
            return outStr;
        }
        
        /// <summary>
        /// 打开软件并执行命令
        /// </summary>
        /// <param name="programName">软件路径加名称（.exe文件）</param>
        /// <param name="cmd">要执行的命令</param>
        public void RunProgram(string programName, string cmd)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = programName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (cmd.Length != 0)
            {
                proc.StandardInput.WriteLine(cmd);
            }
            proc.Close();
        }
        
        /// <summary>
        /// 打开软件
        /// </summary>
        /// <param name="programName">软件路径加名称（.exe文件）</param>
        public void RunProgram(string programName)
        {
            this.RunProgram(programName, "");
        }
    }
}