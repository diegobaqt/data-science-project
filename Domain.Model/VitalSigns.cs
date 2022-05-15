using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Base;

namespace Domain.Model
{
    public class VitalSigns : EntityBase
    {
        [Key]
        [Required]
        public Guid VitalSignsId { get; set; }

        public string HourEvent { get; set; }

        public int? HeartRate { get; set; }

        public int? OxygenSaturation { get; set; }

        public int? RespiratoryRate { get; set; }

        public int? SystolicBloodPressure { get; set; }

        public int? DiastolicBloodPressure { get; set; }

        public int? MeanBloodPressure { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }

        public Patient Patient { get; set; }
    }
}
