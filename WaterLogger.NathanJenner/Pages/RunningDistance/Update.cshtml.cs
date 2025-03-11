using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.NathanJenner.Models;

namespace WaterLogger.NathanJenner.Pages.RunningDistance;

public class UpdateModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public RunningModel RunningModel { get; set; }

    public UpdateModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet(int id)
    {
        RunningModel = GetById(id);
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

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                   $"UPDATE running SET date ='{RunningModel.Date}', distance = {RunningModel.Distance} WHERE Id = {RunningModel.Id}";
            tableCmd.ExecuteNonQuery();
        }
        return RedirectToPage("/RunningDistance/RunningIndex");
    }
}
