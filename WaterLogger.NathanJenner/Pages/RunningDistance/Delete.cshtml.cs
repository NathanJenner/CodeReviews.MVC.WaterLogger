using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.NathanJenner.Models;

namespace WaterLogger.NathanJenner.Pages.RunningDistance;

public class DeleteModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public RunningModel Running { get; set; }

    public DeleteModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet(int id)
    {
        Running = GetById(id);
        return Page();
    }

    public RunningModel GetById(int id)
    {
        var runningRecord = new RunningModel();

        using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM running WHERE Id = {id}";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            while (reader.Read())
            {
                runningRecord.Id = reader.GetInt32(0);
                runningRecord.Date = DateTime.Parse(reader.GetString(1));
                runningRecord.Distance = reader.GetInt32(2);
            }
            return runningRecord;
        }
    }

    public IActionResult OnPost(int id)
    {
        using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM running WHERE Id = {id}";
            tableCmd.ExecuteNonQuery();
        }
        return RedirectToPage("/RunningDistance/RunningIndex");
    }
}
