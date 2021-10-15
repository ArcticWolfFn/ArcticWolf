using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolfLauncher.Services
{
    public static class LauncherService
    {
        public static void LaunchGame()
        {
            string gamePath = @"D:\Games\FN Custom\15.00\FortniteGame\Binaries\Win64\FortniteClient-Win64-Shipping.exe";
            string flToken = "7a848a93a74ba68876c36C1c";

            List<string> baseArgs = new List<string>()
        {
          "-AUTH_LOGIN=user@gmail.com",
          "-AUTH_PASSWORD=unused",
          "-AUTH_TYPE=epic",
          "-epicapp=Fortnite",
          "-epicportal",
          "-fltoken=" + flToken,
          "-fromfl=none",
          "-noeac",
          "-nobe",
          "-skippatchcheck"
        };
            string args = string.Join(" ", baseArgs);

            Process fortniteProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    Arguments = args,
                    FileName = gamePath
                },
                EnableRaisingEvents = true
            };

            fortniteProcess.Start();

            Process _fnLauncherProcess = Process.Start(new ProcessStartInfo()
            {
                Arguments = args,
                CreateNoWindow = true,
                FileName = "Resources/FortniteLauncher.exe"
            });
            Process _fnAntiCheatProcess = Process.Start(new ProcessStartInfo()
            {
                Arguments = args,
                CreateNoWindow = true,
                FileName = "Resources/FortniteClient-Win64-Shipping_BE.exe"
            });
        }
    }
}
