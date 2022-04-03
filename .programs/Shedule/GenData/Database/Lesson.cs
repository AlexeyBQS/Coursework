using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenData.Database
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int? TeacherId { get; set; }
        public int? ClassId { get; set; }
        public string Name { get; set; }
        public int CountHours { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual Class Class { get; set; }

        public virtual ICollection<Shedule> Shedules { get; set; }
    }
}
