using System;

namespace MFMELaunch
{
    class Program
    {
        private static Launcher _launcher = null;

        static void Main(string[] args)
        {
            _launcher = new Launcher(args);
            _launcher.Launch();
        }
    }
}
