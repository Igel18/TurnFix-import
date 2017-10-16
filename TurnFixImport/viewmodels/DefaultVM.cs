namespace TurnFixImport.viewmodels
{
    using configuration;
    using framework;
    using import;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class DefaultVM : ViewModelBase
    {
        private bool _canImport = true;

        public DefaultVM()
        {
            Exit = new CommandEx(DoExit, CanExit);
            Import = new CommandEx(DoImport, CanImport);
            Open = new CommandEx(DoOpen);
            DatabaseType = 0;
            AutomaticSelected = true;
        }

        public int DatabaseType
        {
            get { return GetValue(() => DatabaseType); }
            set
            {
                if (value == 1)
                {
                    Sqlite = true;
                }
                else
                {
                    Sqlite = false;
                }

                SetValue(() => DatabaseType, value);
            }
        }

        public bool Sqlite
        {
            get { return GetValue(() => Sqlite); }
            set { SetValue(() => Sqlite, value); }
        }

        public bool AutomaticSelected
        {
            get { return GetValue(() => AutomaticSelected); }
            set { SetValue(() => AutomaticSelected, value); }
        }

        public string UserName
        {
            get { return GetValue(() => UserName); }
            set { SetValue(() => UserName, value); }
        }

        public string Password
        {
            get { return GetValue(() => Password); }
            set { SetValue(() => Password, value); }
        }

        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }

        public string Server
        {
            get { return GetValue(() => Server); }
            set { SetValue(() => Server, value); }
        }

        public string Database
        {
            get { return GetValue(() => Database); }
            set { SetValue(() => Database, value); }
        }

        public bool ManualSelected
        {
            get { return GetValue(() => ManualSelected); }
            set { SetValue(() => ManualSelected, value); }
        }

        public string DatabasePathSqlite
        {
            get { return GetValue(() => DatabasePathSqlite); }
            set { SetValue(() => DatabasePathSqlite, value); }
        }

        public string PostgresInstallation
        {
            get { return GetValue(() => PostgresInstallation); }
            set { SetValue(() => PostgresInstallation, value); }
        }

        public string ImportFile
        {
            get { return GetValue(() => ImportFile); }
            set { SetValue(() => ImportFile, value); }
        }

        public CommandEx Open { get; set; }
        public CommandEx Exit { get; set; }
        public CommandEx Import { get; set; }

        private void DoExit(object o)
        {
            System.Environment.Exit(0);
        }

        private bool CanExit()
        {
            return _canImport;
        }

        private void DoImport(object o)
        {
            try
            {
                //CheckValidity();

                _canImport = false;
                var importCoordinator = new ImportCoordinator();

                if (Sqlite)
                {
                    var path = DatabasePathSqlite;
                    importCoordinator.ImportSQLite(path, ImportFile, Title);
                }
                else
                {
                    string server = string.Empty;
                    string user = string.Empty;
                    string password = string.Empty;
                    string database = string.Empty;
                    string importFile = ImportFile;

                    if (AutomaticSelected)
                    {
                        var path = PostgresInstallation;

                        if (path.EndsWith("\\"))
                        {
                            path = path.Remove(path.LastIndexOf("\\"));
                        }

                        if (File.Exists(path))
                        {
                            path = Directory.GetParent(path).FullName + @"\turnfix.ini";
                        }
                        else
                        {
                            path = path + @"\turnfix.ini";
                        }

                        var text = File.ReadAllText(path);

                        var line = text.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.StartsWith("[Database]")).FirstOrDefault();

                        var lines = line.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        server = lines.Where(x => x.StartsWith("Server")).FirstOrDefault().Split('=')[1].Trim();
                        user = lines.Where(x => x.StartsWith("Username")).FirstOrDefault().Split('=')[1].Trim();
                        password = lines.Where(x => x.StartsWith("Password")).FirstOrDefault().Split('=')[1].Trim();
                        database = lines.Where(x => x.StartsWith("Database")).FirstOrDefault().Split('=')[1].Trim();
                    }
                    else
                    {
                        server = Server;
                        user = UserName;
                        password = Password;
                        database = Database;
                    }

                    importCoordinator.ImportPostGreSQL(server, user, password, database, importFile, Title);
                }

                _canImport = true;

                MessageBox.Show("Der Import wurde erfolgreich durchgeführt", "Import vollständig");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ein Fehler ist aufgetreten");
                _canImport = true;
            }
        }

        private void CheckValidity()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            path = path + @"\starterInfo.dat";

            try
            { 
                if (!File.Exists(path))
                {
                    var date = Convert.ToString(DateTime.UtcNow, System.Globalization.CultureInfo.InvariantCulture);

                    File.WriteAllText(path, date);

                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
                }
                else
                {
                    var content = File.ReadAllText(path);

                    var date = DateTime.Parse(content, System.Globalization.CultureInfo.InvariantCulture);

                    if (date.AddDays(32) < DateTime.UtcNow)
                    {
                        throw new Exception("Testperiode abgelaufen");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Gültigkeit konnte nicht überpüft werden" + (string.IsNullOrEmpty(ex.Message) ? string.Empty : $": {ex.Message}"));
            }
        }

        private void DoOpen(object o)
        {
            var commandParam = (string)o;
            var value = string.Empty;

            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    value = dialog.FileName;
                }
            }
       
            switch (commandParam)
            {
                case "sqlite":
                    DatabasePathSqlite = value;
                    break;
                case "postgresinstallation":
                    PostgresInstallation = value;
                    break;

                case "importfile":
                    ImportFile = value;
                    break;
            }
        }

        private bool CanImport()
        {
            return _canImport;
        }
    }
}
