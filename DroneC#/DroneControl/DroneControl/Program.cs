﻿using System;
using AR.Drone.Infrastructure;
using System.Windows.Forms;

/**
 * @author: Gerhard Kroes
 * */
namespace DroneControl {
    class Program {
        static void Main(string[] args) {
            // Loading FFmpeg
            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    string ffmpegPath = string.Format(@"../../../FFmpeg.AutoGen/FFmpeg/bin/windows/{0}", Environment.Is64BitProcess ? "x64" : "x86");
                    InteropHelper.RegisterLibrariesSearchPath(ffmpegPath);
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    string libraryPath = Environment.GetEnvironmentVariable(InteropHelper.LD_LIBRARY_PATH);
                    InteropHelper.RegisterLibrariesSearchPath(libraryPath);
                    break;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Connection sockect for incoming commands
            ConnectionSocket cs = ConnectionSocket.Instance;
            cs.Start();
        }
    }
}
