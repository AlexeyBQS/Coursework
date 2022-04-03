using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenData.Database
{
    public class Shedule
    {
        public DateTime Day { get; set; }
        public int NumberLesson { get; set; }
        public int ClassId { get; set; }
        public int? LessonId { get; set; }
        public int? CabinetId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual Cabinet Cabinet { get; set; }
    }
}
