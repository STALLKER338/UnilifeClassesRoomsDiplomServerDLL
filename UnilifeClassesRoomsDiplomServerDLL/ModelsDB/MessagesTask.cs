namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MessagesTask
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        public DateTime Time { get; set; }

        public int TaskId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Task Task { get; set; }
    }
}
