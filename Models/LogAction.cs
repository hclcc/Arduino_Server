using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arduino.Models;

public partial class LogAction
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Command { get; set; }

    [StringLength(50)]
    public string ValveNr { get; set; }

    [StringLength(50)]
    public string Action { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateExec { get; set; }

    public bool? Result { get; set; }

    [StringLength(500)]
    public string Info { get; set; }
}
