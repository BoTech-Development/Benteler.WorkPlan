using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Planner
{
    public class Year
    {
        [Key]
        public int Id { get; set; }
        public int YearNumber { get; }
        public List<CalenderWeek> CalenderWeeks { get; set; } = new List<CalenderWeek>();
    }
}
