using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shedule.Database
{
    public class EducateContext : DbContext
    {
        public EducateContext() : base("EducateConnection")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Shedule> Shedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Teachers
            modelBuilder.Entity<Teacher>()
                .HasKey(teacher => teacher.TeacherId);

            modelBuilder.Entity<Teacher>()
                .Property(teacher => teacher.TeacherId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Cabinets
            modelBuilder.Entity<Cabinet>()
                .HasKey(cabinet => cabinet.CabinetId);

            modelBuilder.Entity<Cabinet>()
                .Property(cabinet => cabinet.CabinetId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Cabinet>()
                .HasOptional<Teacher>(cabinet => cabinet.Teacher)
                .WithMany(teacher => teacher.Cabinets)
                .HasForeignKey<int>(cabinet => (int)cabinet.TeacherId)
                .WillCascadeOnDelete(false);

            //Classes
            modelBuilder.Entity<Class>()
                .HasKey(_class => _class.ClassId);

            modelBuilder.Entity<Class>()
                .Property(_class => _class.ClassId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Class>()
                .HasOptional<Teacher>(_class => _class.Teacher)
                .WithMany(teacher => teacher.Classes)
                .HasForeignKey<int>(_class => (int)_class.TeacherId)
                .WillCascadeOnDelete(false);

            //Lessons
            modelBuilder.Entity<Lesson>()
                .HasKey(lesson => lesson.LessonId);

            modelBuilder.Entity<Lesson>()
                .Property(lesson => lesson.LessonId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Lesson>()
                .HasOptional<Teacher>(lesson => lesson.Teacher)
                .WithMany(teacher => teacher.Lessons)
                .HasForeignKey<int>(lesson => (int)lesson.TeacherId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Lesson>()
                .HasOptional<Class>(lesson => lesson.Class)
                .WithMany(_class => _class.Lessons)
                .HasForeignKey<int>(lesson => (int)lesson.ClassId)
                .WillCascadeOnDelete(false);

            //Shedules
            modelBuilder.Entity<Shedule>()
                .HasKey(shedule => new { shedule.Day, shedule.NumberLesson, shedule.ClassId });

            modelBuilder.Entity<Shedule>()
                .HasOptional<Cabinet>(shedule => shedule.Cabinet)
                .WithMany(cabinet => cabinet.Shedules)
                .HasForeignKey<int>(shedule => (int)shedule.CabinetId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shedule>()
                .HasRequired<Class>(shedule => shedule.Class)
                .WithMany(_class => _class.Shedules)
                .HasForeignKey<int>(shedule => (int)shedule.ClassId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shedule>()
                .HasOptional<Lesson>(shedule => shedule.Lesson)
                .WithMany(lessons => lessons.Shedules)
                .HasForeignKey<int>(shedule => (int)shedule.LessonId)
                .WillCascadeOnDelete(false);
        }
    }
}
