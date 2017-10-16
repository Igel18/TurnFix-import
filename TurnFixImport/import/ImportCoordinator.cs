namespace TurnFixImport.import
{
    using mappings;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class ImportCoordinator
    {
        public void ImportPostGreSQL(string server, string username, string password, string database, string importFile, string title)
        {
            var connectionStringPostGres = $"Host={server};Username={username};Password={password};Database={database}";
            Import(new NpgsqlConnection(connectionStringPostGres), importFile, title);
        }

        public void ImportSQLite(string dbPath, string importFile, string title)
        {
            var con = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            Import(con, importFile, title);
        }

        private void Import(DbConnection connection, string pathImportFile, string title)
        {
            var now = DateTime.Now;
            var today = DateTime.Now.ToString("yyyy-MM-dd_hh-mm");

            var transformer = new DataTransformer(pathImportFile);

            var root = transformer.TransformToXML();

            using (var dbImporter = new DatabaseImporter(connection))
            {
                var eventId = dbImporter.AddOrUpdateEvent(string.IsNullOrEmpty(title) ? $"Neue Veranstaltung ({today})" : title, now, now);

                foreach (var competition in root.Descendants().Where(x => x.Name == "Wettkampf"))
                {
                    var name = competition.Element(XName.Get("waBezeichnung")).Value;
                    var number = competition.Element(XName.Get("waNr")).Value;
                    var alterMax = Int32.Parse(competition.Element(XName.Get("waAlterMax")).Value);
                    var alterMin = Int32.Parse(competition.Element(XName.Get("waAlterMin")).Value);
                    var bereich = Int32.Parse(competition.Element(XName.Get("waGeschlecht")).Value);

                    var competitionId = dbImporter.AddOrUpdateCompetition(eventId, bereich, now.Year - alterMax, now.Year - alterMin, name, number);

                    foreach (var dicipline in competition.Descendants().Where(x => x.Name == "Disziplin"))
                    {
                        var gymNetDisciplineId = Int32.Parse(dicipline.Element(XName.Get("wedDisNr")).Value);
                        dbImporter.AddOrUpdateDiciplines(competitionId, gymNetDisciplineId);
                    }

                    foreach (var club in competition.Descendants().Where(x => x.Name == "Mannschaft"))
                    {
                        var clubName = club.Element(XName.Get("verKurzname")).Value;

                        var clubId = dbImporter.AddOrUpdateClub(clubName);

                        foreach (var participant in club.Descendants().Where(x => x.Name == "TN"))
                        {
                            var firstName = participant.Element(XName.Get("perVorname")).Value;
                            var lastName = participant.Element(XName.Get("perName")).Value;
                            var sex = SexMapper.Map(Int32.Parse(participant.Element(XName.Get("perGeschlecht")).Value));
                            var birthday = DateTime.ParseExact(participant.Element(XName.Get("perGeburt")).Value, "dd.mm.yyyy", CultureInfo.InvariantCulture);
                            var startNumber = Int32.Parse(participant.Element(XName.Get("espStartnummer")).Value);

                            var userId = dbImporter.AddOrUpdateParticipator(clubId, firstName, lastName, sex, birthday, false, startNumber);

                            dbImporter.AddOrUpdateUserCompetitionAssignment(userId, competitionId, 1);
                        }

                    }
                }

                // If everything was fine, commit the changes!
                dbImporter.Commit();
            }
        }
    }
}
