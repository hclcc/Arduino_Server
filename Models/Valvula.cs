using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Models;

public partial class Valvula
{
    [Key]
    public int IdValve { get; set; }

    [StringLength(250)]
    public string Description { get; set; }

    public bool? Status { get; set; }

    [Column("IDPin")]
    public int? Idpin { get; set; }

    [InverseProperty("IdValveNavigation")]
    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
}
