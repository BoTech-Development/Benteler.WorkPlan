using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benteler.WorkPlan.Api.SharedModels.Planner
{
    /// <summary>
    /// Represents a section of a working day in the planner.
    /// </summary>
    public class WorkingDaySection
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The start time of the section. 
        /// </summary>
        public TimeOnly StartTime { get; set; }
        /// <summary>
        /// The end time of the section. 
        /// </summary>
        public TimeOnly EndTime { get; set; }
        /// <summary>
        /// User can comment each section.
        /// </summary>
        public string Comment { get; set; } = string.Empty;
    }
}
