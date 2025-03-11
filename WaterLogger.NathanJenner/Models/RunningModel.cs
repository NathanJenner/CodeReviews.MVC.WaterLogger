using System.ComponentModel.DataAnnotations;

namespace WaterLogger.NathanJenner.Models;

public class RunningModel
{
    public int Id { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public float Distance { get; set; }
}
