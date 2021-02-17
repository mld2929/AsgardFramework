using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AsgardFramework.WoWAPI
{
    internal class Logger
    {
        private readonly string m_file;
        private static Logger m_instance;
        internal static Logger Instance => m_instance ??= new Logger();

        private Logger() {
            m_file = $"Log_{DateTime.Now.ToFileTime()}.txt";
        }

        internal void Debug(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null) {
            File.AppendAllText(m_file, $"{DateTime.Now} {path}:{member} - {message}{Environment.NewLine}");
        }
    }
}
