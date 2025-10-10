using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Planner
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        public bool IsHoliday { get; set; }
        public DateTime Date { get; set; }
        public WorkingDay? WorkingDay { get; set; }
    }
}
