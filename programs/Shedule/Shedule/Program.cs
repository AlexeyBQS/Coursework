using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Shedule
{
    public static class Program
    {
        private static bool _CancelProgram = false;
        private static readonly string DatabaseNameDefault = "SheduleDatabase";
        private static string _DatabaseName = string.Empty;

        private static bool CancelProgram
        {
            get { return _CancelProgram; }
            set
            {
                _CancelProgram = value;

                if (_CancelProgram == true)
                {
                    Environment.Exit(0);
                }
            }
        }
        private static string DatabaseName
        {
            get
            {
                if (_DatabaseName == string.Empty)
                {
                    _DatabaseName = ConfigurationManager.ConnectionStrings["DatabaseName"].ConnectionString;
                }

                return _DatabaseName;
            }
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string path = assemblyName.Name + ".dll";

            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                path = string.Format(@"{0}\{1}", assemblyName.CultureInfo, path);
            }

            using (Stream stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                byte[] assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);

                return Assembly.Load(assemblyRawBytes);
            }
        }

        private static void CheckConfigProgramFile()
        {
            string fullPath = $"{Directory.GetCurrentDirectory()}\\Shedule.exe.Config";
            if (!File.Exists(fullPath))
            {
                string content =
                    $"<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                    + $"\n<configuration>"
                    + $"\n   <startup>"
                    + $"\n      <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.8\"/>"
                    + $"\n   </startup>"
                    + $"\n   <connectionStrings>"
                    + $"\n      <add name=\"DatabaseName\" connectionString=\"{DatabaseNameDefault}\" providerName=\"\"/>"
                    + $"\n      <add name=\"EducateConnection\" connectionString=\"\" providerName=\"\"/>"
                    + $"\n   </connectionStrings>"
                    + $"\n</configuration>";

                File.WriteAllText(fullPath, content);
            }
        }

        private static bool ExistSqlLocalDb()
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("sqllocaldb info");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            return cmd.StandardOutput.ReadToEnd().Contains("MSSQLLocalDB");
        }

        private static void ChangePathConnectionStringDatabase()
        {
            if (!ExistSqlLocalDb())
            {
                Message.Error_NonExistSqlLocalDb();
                CancelProgram = true;
            }

            string DataSource = "(LocalDB)\\MSSQLLocalDB";
            string AttachDbFilename = $"{Directory.GetCurrentDirectory()}\\{DatabaseName}.mdf";
            bool IntegratedSecurity = true;
            string ApplicationIntent = "ReadWrite";
            bool MultiSubnetFailover = false;
            bool TrustedConnection = true;

            string connectionString =
                $"Data Source = {DataSource};"
                + $" AttachDbFilename = {AttachDbFilename};"
                + $" Integrated Security = {IntegratedSecurity};"
                + $" ApplicationIntent = {ApplicationIntent};"
                + $" MultiSubnetFailover = {MultiSubnetFailover};"
                + $" Trusted_Connection = {TrustedConnection}";

            string providerName = "System.Data.SqlClient";

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

            connectionStringsSection.ConnectionStrings["EducateConnection"].ConnectionString = connectionString;
            connectionStringsSection.ConnectionStrings["EducateConnection"].ProviderName = providerName;
            config.Save();

            ConfigurationManager.RefreshSection("connectionStrings");
        }

        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

            CheckConfigProgramFile();
            ChangePathConnectionStringDatabase();

            App.Main();
        }
    }
}
