namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LinksTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Link { get; set; }

        public int TaskId { get; set; }

        public virtual Task Task { get; set; }
    }
}
