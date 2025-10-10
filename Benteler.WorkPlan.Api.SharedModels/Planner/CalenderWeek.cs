using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Planner
{
    public class CalenderWeek
    {
        [Key]
        public int Id { get; set; }
        public int WeekNumber { get; set; }
        public List<Day> Days { get; set; } = new List<Day>();
    }
}
