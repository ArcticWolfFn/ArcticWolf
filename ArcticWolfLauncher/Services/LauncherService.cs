using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArcticWolfLauncher.Services
{
    public static class LauncherService
    {
        private static Process _fnLauncherProcess = null;
        private static Process _fnAntiCheatProcess = null;
        private static Process _fortniteProcess = null;

        public static void LaunchGame()
        {
            Task.Run(() =>
            {
                string gamePath = Path.Join(AppSettings.Default.FNPath, @"\FortniteGame\Binaries\Win64\FortniteClient-Win64-Shipping.exe");
                if (!File.Exists(gamePath))
                {
                    MessageBox.Show("Invalid game path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

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

                _fortniteProcess = new()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = args,
                        FileName = gamePath
                    },
                    EnableRaisingEvents = true
                };

                _fortniteProcess.Exited += FortniteProcess_Exited;

                _fortniteProcess.Start();

                //prevent mapping the dll too fast
                while (_fortniteProcess.Id == 0)
                {

                }

                _fnLauncherProcess = Process.Start(new ProcessStartInfo()
                {
                    Arguments = args,
                    CreateNoWindow = true,
                    FileName = "Resources/FortniteLauncher.exe"
                });
                _fnAntiCheatProcess = Process.Start(new ProcessStartInfo()
                {
                    Arguments = args,
                    CreateNoWindow = true,
                    FileName = "Resources/FortniteClient-Win64-Shipping_BE.exe"
                }); 

                InjectDll(_fortniteProcess.Id, "PlataniumV2.dll");
            });
        }

        private static void FortniteProcess_Exited(object sender, EventArgs e)
        {
            _fnLauncherProcess?.Kill();
            _fnAntiCheatProcess?.Kill();
        }

        public static void InjectDll(int processId, string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("DLL not found");
                return;
            }
            var handle = Win32.OpenProcess(2 | 0x0400 | 8 | 0x0020 | 0x0010, false, processId);

            var loadLibrary = Win32.GetProcAddress(Win32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            var size = (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char)));
            var address = Win32.VirtualAllocEx(handle, IntPtr.Zero, size, 0x1000 | 0x2000, 4);

            Win32.WriteProcessMemory(handle, address, Encoding.Default.GetBytes(path), size, out UIntPtr bytesWritten);

            Win32.CreateRemoteThread(handle, IntPtr.Zero, 0, loadLibrary, address, 0, IntPtr.Zero);
        }
    }
}
