using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenData.Database
{
    public class Teacher
    {
        public int TeacherId { get;set; }
        public string Surname { get;set; }
        public string Name { get;set; }
        public string Midname { get;set; }
        public DateTime Birthday { get;set; }
        public string Passport { get;set; }

        public virtual ICollection<Cabinet> Cabinets { get; set; }
        public virtual ICollection<Class> Classes { get;set; }
        public virtual ICollection<Lesson> Lessons { get;set; }

        public string BirthdayDisplay => Birthday.ToShortDateString();
    }
}
