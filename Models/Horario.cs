using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Models;

public partial class Horario
{
    [Key]
    public int Id { get; set; }

    public int? IdValve { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastRunDate { get; set; }

    public int? RunForMin { get; set; }

    public int? RunEveryHour { get; set; }

    public int? IsActive { get; set; }

    [ForeignKey("IdValve")]
    [InverseProperty("Horarios")]
    public virtual Valvula IdValveNavigation { get; set; }
}
