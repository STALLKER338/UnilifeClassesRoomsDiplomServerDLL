namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Log
    {
        public int Id { get; set; }

        [Column("Log")]
        [Required]
        [StringLength(2000)]
        public string Log1 { get; set; }

        public int AccountId { get; set; }

        public DateTime Time { get; set; }

        public virtual Account Account { get; set; }
    }
}
