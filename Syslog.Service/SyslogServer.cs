/*
 * Copyright 2010 Andrew Draut
 * 
 * This file is part of Syslog Sharp.
 * 
 * Syslog Sharp is free software: you can redistribute it and/or modify it under the terms of the GNU General 
 * Public License as published by the Free Software Foundation, either version 3 of the License, or (at 
 * your option) any later version.
 * 
 * Syslog Sharp is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even 
 * the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with Syslog Sharp. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace Syslog.Service
{
    public partial class SyslogServer : ServiceBase
    {
        private Syslog.Server.Listener server;
        private Syslog.Server.Console.Server consoleServer;

        public SyslogServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (server == null)
            {

                server = Syslog.Server.Listener.CreateInstance(System.Configuration.ConfigurationManager.AppSettings["listenIPAddress"],
                    Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["listenPort"]),
                    Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["bufferFlushFrequency"]));
            }

            if (!server.Start())
            {
                this.OnStop();

                return;
            }

            if (consoleServer == null)
            {
                consoleServer = new Syslog.Server.Console.Server();
            }

            consoleServer.Start();
        }

        protected override void OnStop()
        {
            if (server != null)
            {
                server.Stop();
            }

            if (consoleServer != null)
            {
                consoleServer.Stop();
            }
        }
    }
}
