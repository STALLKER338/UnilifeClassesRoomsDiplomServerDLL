namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClassAccount")]
    public partial class ClassAccount
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public bool Teacher { get; set; }

        public int ClassId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Class Class { get; set; }
    }
}
