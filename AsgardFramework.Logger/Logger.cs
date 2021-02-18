using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AsgardFramework.Logger
{
    public class Logger
    {
        private readonly string m_file;
        private static Logger m_instance;
        public static Logger Instance => m_instance ??= new Logger();

        private Logger() {
            m_file = $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now.ToFileTime()}.txt";
        }
        public void Write(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null) {
            File.AppendAllText(m_file, $"{DateTime.Now} {path}:{member} - {message}{Environment.NewLine}");
        }
    }
}
