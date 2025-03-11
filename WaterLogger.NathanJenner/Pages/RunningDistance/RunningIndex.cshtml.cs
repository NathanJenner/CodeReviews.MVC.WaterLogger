using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.NathanJenner.Models;

namespace WaterLogger.NathanJenner.Pages.RunningDistance;

public class RunningIndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    public List<RunningModel> Records { get; set; }

    public RunningIndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        Records = GetAllRecords();
        ViewData["Total"] = Records.AsEnumerable().Sum(x => x.Distance);
    }

    private List<RunningModel> GetAllRecords()
    {
        using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM running";

            var tableData = new List<RunningModel>();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            while (reader.Read())
            {
                tableData.Add(
                new RunningModel
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    Distance = reader.GetInt32(2),
                });
            }
            return tableData;
        }
    }
}
