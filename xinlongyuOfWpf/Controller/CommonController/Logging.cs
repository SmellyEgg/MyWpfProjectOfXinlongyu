using System;
using System.IO;
using xinlongyuOfWpf.Controller.CommonPath;

namespace xinlongyuOfWpf.Controller.CommonController
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Logging
    {
        public static string LogFilePath;

        private static FileStream _fs;
        private static StreamWriterWithTimestamp _sw;

        public static bool OpenLogFile()
        {
            try
            {
                LogFilePath = GetTempPath("icityservices.log");

                _fs = new FileStream(LogFilePath, FileMode.Append);
                _sw = new StreamWriterWithTimestamp(_fs);
                _sw.AutoFlush = true;
                Console.SetOut(_sw);
                Console.SetError(_sw);

                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        private static string _tempPath = null;

        public static string GetTempPath()
        {
            if (_tempPath == null)
            {
                try
                {
                    Directory.CreateDirectory(configManagerSection.logPath);
                    // don't use "/", it will fail when we call explorer /select xxx/ss_win_temp\xxx.log
                    _tempPath = configManagerSection.logPath;
                }
                catch (Exception e)
                {
                    Logging.Error(e);
                    throw;
                }
            }
            return _tempPath;
        }

        // return a full path with filename combined which pointed to the temporary directory
        public static string GetTempPath(string filename)
        {
            return Path.Combine(GetTempPath(), filename);
        }

        private static void WriteToLogFile(object o)
        {
            try
            {
                Console.WriteLine(o);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public static void Error(object o)
        {
            WriteToLogFile("[E] " + o);
        }

        public static void Info(object o)
        {
            WriteToLogFile(o);
        }

        public static void Clear()
        {
            _sw.Close();
            _sw.Dispose();
            _fs.Close();
            _fs.Dispose();
            File.Delete(LogFilePath);
            OpenLogFile();
        }

        // Simply extended System.IO.StreamWriter for adding timestamp workaround
        public class StreamWriterWithTimestamp : StreamWriter
        {
            public StreamWriterWithTimestamp(Stream stream) : base(stream)
            {
            }

            private string GetTimestamp()
            {
                return "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ";
            }

            public override void WriteLine(string value)
            {
                base.WriteLine(GetTimestamp() + value);
            }

            public override void Write(string value)
            {
                base.Write(GetTimestamp() + value);
            }
        }
    }
}
