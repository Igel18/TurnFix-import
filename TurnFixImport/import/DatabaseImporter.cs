namespace TurnFixImport.import
{
    using configuration;
    using Npgsql;
    using NpgsqlTypes;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DatabaseImporter : IDisposable
    {
        private DbConnection _dbConnection;
        private DbTransaction _dbTransaction;
        private Dictionary<int, int> _gymNetToTurnFix;

        public DatabaseImporter(DbConnection dbConnection)
        {
            dbConnection.Open();
            _dbTransaction = dbConnection.BeginTransaction();
            _dbConnection = dbConnection;

            InitDiciplines();
        }

        private void InitDiciplines()
        {
            var config = RegisterDiciplinesConfig.GetConfig();

            var tmp = new List<Dicipline>();

            // Key = GymNetId; Value = TurnFixId
            _gymNetToTurnFix = new Dictionary<int, int>();

            foreach (var item in config.DiciplineMappings)
            {
                tmp.Add((Dicipline)item);
            }

            // Directly map
            tmp.Where(x => !string.IsNullOrEmpty(x.TurnFixId)).ToList().ForEach(y => _gymNetToTurnFix[y.GymNetId] = int.Parse(y.TurnFixId));

            // Get from DB
            tmp = tmp.Where(x => string.IsNullOrEmpty(x.TurnFixId)).ToList();

            tmp.ForEach(x =>
            {
                using (var command = _dbConnection.CreateCommand())
                {
                    var sql = $"select int_disziplinenid from tfx_disziplinen where var_name = @name and bol_m = @male and bol_w = @female";
                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", x.Name);
                    command.AddParameterWithValue("@male", x.Male);
                    command.AddParameterWithValue("@female", x.Female);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _gymNetToTurnFix[x.GymNetId] = reader.GetInt32(0);
                        }
                    }
                }
            });
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }

        public int AddOrUpdateEvent(string name, DateTime from, DateTime till)
        {
            try
            {
                var competitionPlaceId = GetOrAddCompetitionPlaceId();
                using (var command = _dbConnection.CreateCommand())
                { 
                    var sql = $"select int_veranstaltungenid from tfx_veranstaltungen where var_name = @name";
                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                    }

                    command.Parameters.Clear();

                    sql = $"insert into tfx_veranstaltungen (int_wettkampforteid, dat_von, dat_bis, var_name) values (@wkoid, @from, @till, @name)";
                    command.CommandText = sql;
                    command.AddParameterWithValue("@wkoid", competitionPlaceId);
                    command.AddParameterWithValue("@from", from);
                    command.AddParameterWithValue("@till", till);
                    command.AddParameterWithValue("@name", name);
                    var amount = command.ExecuteNonQuery();

                    sql = $"select int_veranstaltungenid from tfx_veranstaltungen where var_name = @name";
                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        { 
                            throw new ArgumentException("Couldn't persist event");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                _dbTransaction.Rollback();
                throw e;
            }
        }

        public void AddOrUpdateDiciplines(int competitionId, int gymNetDiciplineId)
        {
            try
            {
                int turnFixId;
                if (!_gymNetToTurnFix.TryGetValue(gymNetDiciplineId, out turnFixId))
                {
                    return;
                }

                using (var command = _dbConnection.CreateCommand())
                {
                    var sql = "select int_wettkaempfe_x_disziplinenid from tfx_wettkaempfe_x_disziplinen where int_wettkaempfeid = @competitionId and int_disziplinenid = @disciplineId";
                    command.CommandText = sql;

                    command.AddParameterWithValue("@competitionId", competitionId);
                    command.AddParameterWithValue("@disciplineId", turnFixId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return;
                        }
                    }

                    command.Parameters.Clear();

                    command.CommandText = "insert into tfx_wettkaempfe_x_disziplinen(int_wettkaempfeid, int_disziplinenid) values(@competitionId, @disciplineId)";
                    command.AddParameterWithValue("@competitionId", competitionId);
                    command.AddParameterWithValue("@disciplineId", turnFixId);

                    var amount = command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
                throw ex;
            }
        }

        public int AddOrUpdateClub(string name)
        {
            try
            {
                using (var command = _dbConnection.CreateCommand())
                { 
                    var sql = "select int_gaueid from tfx_gaue where var_name = 'kein Turngau/Turnkreis'";
                    command.CommandText = sql;

                    int gauId = 0;

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            gauId = reader.GetInt32(0);
                        }
                        else
                        {
                            throw new ArgumentException("Couldn't determine id for 'gau'");
                        }

                    }

                    sql = $"select int_vereineid from tfx_vereine where var_name = @name";
                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                    }

                    command.Parameters.Clear();

                    command.CommandText = "insert into tfx_vereine (int_gaueid, var_name) values (@gauid, @name)";
                    command.AddParameterWithValue("@gauid", gauId);
                    command.AddParameterWithValue("@name", name);

                    var amount = command.ExecuteNonQuery();

                    command.Parameters.Clear();

                    sql = $"select int_vereineid from tfx_vereine where var_name = @name";
                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        {
                            throw new ArgumentException("Couldn't persist club!");
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                _dbTransaction.Rollback();
                throw ex;
            }
        }

        public int AddOrUpdateParticipator(
                                            int clubId, 
                                            string firstName, 
                                            string lastName,
                                            int sex,
                                            DateTime birthday, 
                                            bool onlyYear, 
                                            int startNumber)
        {
            try
            {
                using (var command = _dbConnection.CreateCommand())
                {
                    var sql = "select int_teilnehmerid from tfx_teilnehmer where var_vorname = @firstName and var_nachname = @lastName and dat_geburtstag = @birthday and int_vereineid = @clubId";
            
                    command.CommandText = sql;
                    command.AddParameterWithValue("@firstName", firstName);
                    command.AddParameterWithValue("@lastName", lastName);
                    command.AddParameterWithValue("@birthday", birthday);
                    command.AddParameterWithValue("@clubId", clubId);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                    }

                    sql = "insert into tfx_teilnehmer (int_vereineid, var_vorname, var_nachname, int_geschlecht, dat_geburtstag, bool_nur_jahr, int_startpassnummer)" +
                        "values(@clubId, @firstName, @lastName, @sex, @birthday, @onlyYear, @startNumber)";

                    command.Parameters.Clear();

                    command.CommandText = sql;
                    command.AddParameterWithValue("@clubId", clubId);
                    command.AddParameterWithValue("@firstName", firstName);
                    command.AddParameterWithValue("@lastName", lastName);
                    command.AddParameterWithValue("@sex", sex);
                    command.AddParameterWithValue("@birthday", birthday);
                    command.AddParameterWithValue("@onlyYear", onlyYear);
                    command.AddParameterWithValue("@startNumber", startNumber);

                    var amount = command.ExecuteNonQuery();

                    command.Parameters.Clear();

                    sql = "select int_teilnehmerid from tfx_teilnehmer where var_vorname = @firstName and var_nachname = @lastName and dat_geburtstag = @birthday and int_vereineid = @clubId";

                    command.CommandText = sql;
                    command.AddParameterWithValue("@firstName", firstName);
                    command.AddParameterWithValue("@lastName", lastName);
                    command.AddParameterWithValue("@birthday", birthday);
                    command.AddParameterWithValue("@clubId", clubId);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        {
                            throw new ArgumentException("Couldn't persist participant");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
                throw ex;
            }
        }

        public int AddOrUpdateCompetition(int eventId, int bereichId, int from, int till, string name, string number)
        {
            try
            {
                using (var command = _dbConnection.CreateCommand())
                { 
                    var sql = "select int_wettkaempfeid from tfx_wettkaempfe where var_name = @name and var_nummer = @number and int_veranstaltungenid = @eventId";
                
                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", name);
                    command.AddParameterWithValue("@number", number);
                    command.AddParameterWithValue("@eventId", eventId);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }

                    }

                    command.Parameters.Clear();

                    sql = "insert into tfx_wettkaempfe(int_veranstaltungenid, int_bereicheid, yer_von, yer_bis, var_name, var_nummer)" +
                          "values(@eventId, @bereichid, @from, @till, @name, @number)";
                    command.CommandText = sql;

                    command.AddParameterWithValue("@eventId", eventId);
                    command.AddParameterWithValue("@bereichid", bereichId);
                    command.AddParameterWithValue("@from", from);
                    command.AddParameterWithValue("@till", till);
                    command.AddParameterWithValue("@name", name);
                    command.AddParameterWithValue("@number", number);

                    var amount = command.ExecuteNonQuery();

                    command.Parameters.Clear();

                    sql = "select int_wettkaempfeid from tfx_wettkaempfe where var_name = @name and var_nummer = @number and int_veranstaltungenid = @eventId";

                    command.CommandText = sql;
                    command.AddParameterWithValue("@name", name);
                    command.AddParameterWithValue("@number", number);
                    command.AddParameterWithValue("@eventId", eventId);

                    using (var reader = command.ExecuteReader())
                    { 
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                        else
                        {
                            throw new ArgumentException("Couldn't persist competition");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
                throw ex;
            }
        }

        public void AddOrUpdateUserCompetitionAssignment(int participator, int competition, int status)
        {
            using (var command = _dbConnection.CreateCommand())
            { 
                var sql = "select count(1) from tfx_wertungen where int_wettkaempfeid = @competition and int_teilnehmerid = @participator";

                command.CommandText = sql;
                command.AddParameterWithValue("@competition", competition);
                command.AddParameterWithValue("@participator", participator);

                Int64? res = (Int64?)command.ExecuteScalar();

                if (res != null && res > 0)
                {
                    return;
                }

                command.Parameters.Clear();

                sql = "insert into tfx_wertungen(int_wettkaempfeid, int_teilnehmerid, int_statusid) values(@competition, @participator, @status)";
                command.CommandText = sql;
                command.AddParameterWithValue("@competition", competition);
                command.AddParameterWithValue("@participator", participator);
                command.AddParameterWithValue("@status", status);

                var amount = command.ExecuteNonQuery();
            }
        }

        private int GetOrAddCompetitionPlaceId()
        {
            using (var command = _dbConnection.CreateCommand())
            {
                string sql = "select int_wettkampforteid from tfx_wettkampforte where var_name = 'neu'";
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }

                command.CommandText = "insert into tfx_wettkampforte(var_name) values('neu')";
                command.ExecuteNonQuery();

                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        throw new ArgumentException("Couldn't determine id for 'wettkampfort'");
                    }
                }
            }
        }

        //private bool ReadFirstElement(DbCommand cmd, string element, bool throwEx, out int id)
        //{
        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        if (reader.Read())
        //        {
        //            id = reader.GetInt32(0);
        //            return true;
        //        }
        //        else
        //        {
        //            if (throwEx)
        //            { 
        //                throw new ArgumentException($"Couldnt persist {element}");
        //            }
        //            else
        //            {
        //                id = 0;
        //                return false;
        //            }
        //        }
        //    }
        //}
    }

    public static class DbExtensions
    {
        public static void AddParameterWithValue(this DbCommand command, string parameterName, object parameterValue)
        {
            if (command is NpgsqlCommand && parameterValue is DateTime)
            {
                NpgsqlCommand cmd = command as NpgsqlCommand;

                cmd.Parameters.AddWithValue(parameterName, NpgsqlDbType.Date, parameterValue);
            }
            else if (command is SQLiteCommand && parameterValue is bool)
            {
                SQLiteCommand cmd = command as SQLiteCommand;

                var parameter = cmd.CreateParameter();
                parameter.DbType = System.Data.DbType.String;
                parameter.ParameterName = parameterName;
                parameter.Value = (bool)parameterValue ? "true" : "false";
                cmd.Parameters.Add(parameter);
            }
            else
            { 
                var parameter = command.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.Value = parameterValue;
                command.Parameters.Add(parameter);
            }
        }
    }
}
