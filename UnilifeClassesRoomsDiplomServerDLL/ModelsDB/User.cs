namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.CompilerServices;
    using System.Windows.Media.Imaging;

    public partial class User : INotifyPropertyChanged
    {
        string _name;
        bool _active;
        DateTime _birthday;
        Division _division;
        Post _post; 
        private BitmapImage _photoImage;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Accounts = new HashSet<Account>();
            Users1 = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        public int PostId { get; set; }

        public int DivisionId { get; set; }

        public byte[] Photo { get; set; }

        public int? BossId { get; set; }

        public bool Active { get; set; }

        [NotMapped]
        public virtual BitmapImage PhotoImage
        {
            get { return _photoImage; }
            set 
            { 
                _photoImage = value;
                OnPropertyChanged(nameof(PhotoImage)); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        public virtual Division Division { get; set; }

        public virtual Post Post { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users1 { get; set; }

        public virtual User User1 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
