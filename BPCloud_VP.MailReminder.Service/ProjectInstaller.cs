﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.MailReminder.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller2_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void serviceProcessInstaller2_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}