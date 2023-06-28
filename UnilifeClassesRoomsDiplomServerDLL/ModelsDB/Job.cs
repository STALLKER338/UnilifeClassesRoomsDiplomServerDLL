namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            FilesJobs = new HashSet<FilesJob>();
            LinksJobs = new HashSet<LinksJob>();
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public int? Score { get; set; }

        [StringLength(50)]
        public string Comment { get; set; }

        public int TaskId { get; set; }

        public bool Deleted { get; set; }

        public DateTime Time { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilesJob> FilesJobs { get; set; }

        public virtual Task Task { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LinksJob> LinksJobs { get; set; }
    }
}
