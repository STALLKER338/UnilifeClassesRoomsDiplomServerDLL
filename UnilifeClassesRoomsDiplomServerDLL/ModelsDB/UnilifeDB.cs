using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace UnilifeClassesRoomsDiplomServerDLL.ModelsDB
{
    public partial class UnilifeDB : DbContext
    {
        public UnilifeDB()
            : base("name=UnilifeDB")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<ClassAccount> ClassAccounts { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<FilesJob> FilesJobs { get; set; }
        public virtual DbSet<FilesTask> FilesTasks { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<LinksJob> LinksJobs { get; set; }
        public virtual DbSet<LinksTask> LinksTasks { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<MessagesTask> MessagesTasks { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(e => e.ClassAccounts)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Logs)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.MessagesTasks)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Sessions)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .Property(e => e.KeyClass)
                .IsFixedLength();

            modelBuilder.Entity<Class>()
                .HasMany(e => e.ClassAccounts)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Division>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Division)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.FilesJobs)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.LinksJobs)
                .WithRequired(e => e.Job)
                .HasForeignKey(e => e.JodId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.FilesTasks)
                .WithRequired(e => e.Task)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.Task)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.LinksTasks)
                .WithRequired(e => e.Task)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.MessagesTasks)
                .WithRequired(e => e.Task)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.BossId);
        }
    }
}
