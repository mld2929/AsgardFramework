using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace AsgardFramework.Logger
{
    public class Logger
    {
        private readonly string m_file;
        private static Logger m_instance;
        private readonly object m_lock = new object();
        public static Logger Instance => m_instance ??= new Logger();

        private Logger() {
            m_file = $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now.ToFileTime()}.txt";
        }
        public void Write(string message, [CallerFilePath] string path = null, [CallerMemberName] string member = null) {
            var desc = $"{DateTime.Now} {path}:{member}{Environment.NewLine}";
            var separator = $"{Environment.NewLine}".PadLeft(desc.Length, '/');
            lock (m_lock) {
                File.AppendAllText(m_file, $"{desc}{separator}{message}{Environment.NewLine}{separator}{Environment.NewLine}");
            }
        }
    }
}
