using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.NathanJenner.Models;

namespace WaterLogger.NathanJenner.Pages.WaterDrinking;

public class UpdateModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public DrinkingWaterModel DrinkingWater { get; set; }

    public UpdateModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet(int id)
    {
        DrinkingWater = GetById(id);
        return Page();
    }

    public DrinkingWaterModel GetById(int id)
    {
        var drinkingWaterRecord = new DrinkingWaterModel();

        using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM drinking_water WHERE Id = {id}";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            while (reader.Read())
            {
                drinkingWaterRecord.Id = reader.GetInt32(0);
                drinkingWaterRecord.Date = DateTime.Parse(reader.GetString(1));
                drinkingWaterRecord.Quantity = reader.GetInt32(2);
            }
            return drinkingWaterRecord;
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
                   $"UPDATE drinking_water SET date ='{DrinkingWater.Date}', quantity = {DrinkingWater.Quantity} WHERE Id = {DrinkingWater.Id}";
            tableCmd.ExecuteNonQuery();
        }
        return RedirectToPage("/WaterDrinking/WaterIndex");
    }
}
