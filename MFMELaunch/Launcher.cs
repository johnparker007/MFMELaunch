using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MFMELaunch
{
    public class Launcher
    {
        private const string kMfmeExeFilename = "MFME.exe";
        private const bool kWaitForMFMEExit = false;

        private string[] _args = null;
        private string _gamFilePath = null;
        private Process _mfmeProcess = null;

        public Launcher(string[] args)
        {
            _args = args;
        }

        public bool Launch()
        {
            if (_args == null
                || _args.Length == 0
                || _args[0].Length == 0)
            {
                Console.WriteLine("Command line argument missing, expecting MFME .gam file path");
                return false;
            }

            if (_args[0].Contains(' '))
            {
                Console.WriteLine("Command line argument of MFME .gam file path can't contain spaces");
                return false;
            }

            _gamFilePath = _args[0];

            _mfmeProcess = new Process();

            _mfmeProcess.StartInfo.FileName = kMfmeExeFilename;
            _mfmeProcess.StartInfo.Arguments = _gamFilePath;
            _mfmeProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            _mfmeProcess.Start();

            // TODO various manipulations to send keys to skip popups etc...

            if(kWaitForMFMEExit)
            {
                _mfmeProcess.WaitForExit();
            }

            return true;
        }
    }
}
