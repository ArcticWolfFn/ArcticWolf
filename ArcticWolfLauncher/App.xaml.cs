﻿using ArcticWolfLauncher.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ArcticWolfLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            AppSettings.Default.Save();
            LauncherService.KillGameProcesses();
        }
    }
}
