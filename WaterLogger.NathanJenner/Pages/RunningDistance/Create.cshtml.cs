using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.NathanJenner.Models;

namespace WaterLogger.NathanJenner.Pages.RunningDistance;

public class CreateModel : PageModel
{
    private readonly IConfiguration _configuration;

    public CreateModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public RunningModel RunningModel { get; set; }

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
            tableCmd.CommandText = $"INSERT INTO running(date, distance) VALUES('{RunningModel.Date}', {RunningModel.Distance})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        return RedirectToPage("/RunningDistance/RunningIndex");
    }
}
