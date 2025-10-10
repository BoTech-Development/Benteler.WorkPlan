using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Planner
{
    public class WorkingDay
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// All parts of a working day.
        /// </summary>
        public List<WorkingDaySection> Sections { get; set; } = new List<WorkingDaySection>();
    }
}
