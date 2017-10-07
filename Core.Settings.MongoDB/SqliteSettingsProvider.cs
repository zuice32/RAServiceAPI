using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Core.Application;
using Core.Logging;
using Sqlite;

namespace Agent.Core.Settings.Sqlite
{
    public class SqliteSettingsProvider : SqliteRepository, ISettingsProvider
    {
        public SqliteSettingsProvider(IAgentIdentity agentIdentity, ILogWriter applicationLog)
            : base(GetSettingsFileName(agentIdentity), applicationLog)
        {
        }

        protected override string ClassName
        {
            get { return "Agent.Core.Settings.Sqlite.SqliteSettingsProvider"; }
        }

        private static string GetSettingsFileName(IAgentIdentity agentIdentity)
        {
            return Path.Combine(agentIdentity.PathToAgentDataDirectory, "agent_" + agentIdentity.ID + ".settings");
        }
        
        private SQLiteCommand BuildGetValueCommand()
        {
             string sql = "SELECT value FROM settings WHERE owner = @owner and setting = @setting;";

            SQLiteCommand  getValueCommand = new SQLiteCommand(sql);

            getValueCommand.Parameters.Add("@owner", DbType.String);
            getValueCommand.Parameters.Add("@setting", DbType.String);

            return getValueCommand;
        }

        private SQLiteCommand BuildUpsertCommand()
        {
             string sql =
                "UPDATE settings SET value = @value where owner = @owner and setting = @setting;" +
                "INSERT INTO settings (owner, setting, value) select @owner, @setting, @value where changes() == 0;";

            SQLiteCommand upsertCommand = new SQLiteCommand(sql);

            upsertCommand.Parameters.Add("@owner", DbType.String);
            upsertCommand.Parameters.Add("@setting", DbType.String);
            upsertCommand.Parameters.Add("@value", DbType.String);

            return upsertCommand;
        }

        public string GetSetting(string owner, string setting, string defaultValue)
        {
            base.CheckInitialized();

            string settingValue;

            using (SQLiteConnection dbConnection = base.GetConnection())
            {
                dbConnection.Open();

                using (SQLiteCommand getValueCommand = this.BuildGetValueCommand())
                {
                    getValueCommand.Connection = dbConnection;

                    getValueCommand.Parameters[0].Value = owner;
                    getValueCommand.Parameters[1].Value = setting;

                    settingValue = getValueCommand.ExecuteScalar() as string;
                }
            }

            if (settingValue == null)
            {
                settingValue = defaultValue;

                this.UpsertSetting(owner, setting, defaultValue);
            }

            return settingValue;
        }

        public void UpsertSetting(string owner, string setting, string value)
        {
            base.CheckInitialized();

            using (SQLiteConnection dbConnection = base.GetConnection())
            {
                dbConnection.Open();

                using (SQLiteCommand upsertCommand = this.BuildUpsertCommand())
                {
                    upsertCommand.Connection = dbConnection;

                    using (SQLiteTransaction transaction = dbConnection.BeginTransaction())
                    {
                        upsertCommand.Parameters[0].Value = owner;
                        upsertCommand.Parameters[1].Value = setting;
                        upsertCommand.Parameters[2].Value = value;

                        upsertCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
            }

            if (this.SettingsChanged != null)
            {
                this.SettingsChanged();
            }
        }

        public Setting[] GetAllSettings()
        {
            base.CheckInitialized();

            List<Setting> settings = new List<Setting>();

            using (SQLiteConnection dbConnection = base.GetConnection())
            {
                dbConnection.Open();

                using (SQLiteCommand getAllSettingsCommand = new SQLiteCommand("select * from settings"))
                {
                    getAllSettingsCommand.Connection = dbConnection;

                    using (SQLiteDataReader dataReader = getAllSettingsCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string id = dataReader[0].ToString();
                            string owner = dataReader[1].ToString();
                            string setting = dataReader[2].ToString();
                            string value = dataReader[3].ToString();
                            


                            settings.Add(new Setting(id,owner,setting,value));
                        }
                    }
                }
            }

            return settings.ToArray();

        }


        public event Action SettingsChanged;

        public void Initialize()
        {
            base.InitializeBase();

            this.VerifySettingsTable();
        }

        private void VerifySettingsTable()
        {
            using (SQLiteConnection dbConnection = base.GetConnection())
            {
                dbConnection.Open();

                bool settingsTableExists;

                const string sql = "SELECT count(name) FROM sqlite_master WHERE name = 'settings'";

                using (SQLiteCommand testCommand = new SQLiteCommand(sql, dbConnection))
                {
                    settingsTableExists = (long) testCommand.ExecuteScalar() > 0;
                }

                if (!settingsTableExists)
                {
                    this.CreateSettingsTable(dbConnection);
                }
            }
        }

        private void CreateSettingsTable(SQLiteConnection dbConnection)
        {
            const string sql =
                "CREATE TABLE settings (ID INTEGER PRIMARY KEY, Owner TEXT NOT NULL, Setting TEXT NOT NULL, Value TEXT NULL, UNIQUE(Owner, Setting))";

            using (SQLiteCommand createTable = new SQLiteCommand(sql, dbConnection))
            {
                createTable.ExecuteNonQuery();
            }
        }
    }
}