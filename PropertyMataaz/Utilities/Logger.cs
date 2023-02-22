using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace PropertyMataaz.Utilities
{
    public class Logger
    {
        private static IConfiguration Configuration;

        private Globals _globals;
        public Logger(IConfiguration configuration, IOptions<Globals> globals)
        {
            _globals = globals.Value;
            Configuration = configuration;

        }

        [Obsolete]
        public static void ConfigureSeriLog()
        {

            var columnOption = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "Type"},
            }
            };
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.MSSqlServer("Data Source=34.135.84.221;Initial Catalog=PropertyMataaz;Persist Security Info=True;User ID=sqlserver;Password=Adelowomi@2322","Logs",columnOptions: columnOption)
                .CreateLogger();

            Log.Information("Just to test");
        }

        public static void LogNow(Exception ex, string Type)
        {
            try
            {
                Log.Logger.ForContext("Type", Type).Information(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static bool Info(string ex)
        {
            try
            {
                LogNow(new Exception(ex), MethodBase.GetCurrentMethod().Name);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static void Error(Exception ex)
        {   
            Log.Error(ex,"Error");
            LogNow(ex, MethodBase.GetCurrentMethod().Name);
        }
        public static void Error(string ex)
        {
            LogNow(new Exception(ex), MethodBase.GetCurrentMethod().Name);
        }

        public static void Debug(string ex)
        {
            LogNow(new Exception(ex), MethodBase.GetCurrentMethod().Name);
        }
    }
}
