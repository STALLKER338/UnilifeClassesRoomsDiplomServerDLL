namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FilesJob
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public byte[] File { get; set; }

        public virtual Job Job { get; set; }
    }
}
