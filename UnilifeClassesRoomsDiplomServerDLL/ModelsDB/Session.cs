namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Session
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionKey { get; set; }

        public int AccountId { get; set; }

        public bool Confirm { get; set; }

        public virtual Account Account { get; set; }
    }
}
