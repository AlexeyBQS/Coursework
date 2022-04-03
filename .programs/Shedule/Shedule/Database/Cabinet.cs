using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shedule.Database
{
    public class Cabinet
    {
        public int CabinetId { get; set; }
        public int? TeacherId { get; set; }
        public string Name { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Shedule> Shedules { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", CabinetId, Name);
        }
    }
}
