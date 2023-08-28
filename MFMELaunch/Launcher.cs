using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using WindowsInput;

namespace MFMELaunch
{
    public class Launcher
    {
        private const string kMfmeExeFilename = "MFME.exe";

        private const float kKeypressesInitialDelay = 4.0f;
        private const float kKeypressInterval = 0.05f;
        private const float kKeypressesDuration = 4.0f;

        // this seems necessary on Harvey's cab when launching from the TouchPlay
        // frontend, however wasn't necessary on my dev PC
        private const bool kSendFinalF3ToForceFullScreenMax = true;

        private string[] _args = null;
        private string _gamFilePath = null;
        private Process _mfmeProcess = null;

        private InputSimulator _inputSimulator = null;

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

            _mfmeProcess = CreateMFMEProcess();

            if(!_mfmeProcess.Start())
            {
                Console.WriteLine("Error starting MFME process");
                return false;
            }

            Thread.Sleep((int)(kKeypressesInitialDelay * 1000f));

            SendKeyPresses(kKeypressInterval, kKeypressesDuration);

            if(kSendFinalF3ToForceFullScreenMax)
            {
                _inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.F3);
            }

            AwaitMFMEExeProcessExit();

            return true;
        }

        private Process CreateMFMEProcess()
        {
            Process process = new Process();

            process.StartInfo.FileName = kMfmeExeFilename;
            process.StartInfo.Arguments = _gamFilePath;

            return process;
        }

        private void SendKeyPresses(float pressInterval, float pressesDuration)
        {
            _inputSimulator = new InputSimulator();

            int pressCount = (int)(pressesDuration / pressInterval);
            for(int pressIndex = 0; pressIndex < pressCount; ++pressIndex)
            {
                Console.WriteLine("Generate keypress " + (pressIndex + 1) + " / " + pressCount);

                _inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);

                Thread.Sleep((int)(pressInterval * 1000f));
            }
        }

        private void AwaitMFMEExeProcessExit()
        {
            const float kAwaitExitSleepDuration = 0.100f;
            while(!_mfmeProcess.HasExited)
            {
                Thread.Sleep((int)(kAwaitExitSleepDuration * 1000f));
            }
        }
    }
}
