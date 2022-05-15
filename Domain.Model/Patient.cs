using System.ComponentModel.DataAnnotations;
using DataAccess.Base;

namespace Domain.Model
{
    public class Patient : EntityBase
    {
        [Key]
        public string PatientId { get; set; }

        public string Age { get; set; }

        public string Weight { get; set; }

        public string Height { get; set; }

        public string Gender { get; set; }

        public string Diagnosis { get; set; }

        public bool Survived { get; set; }
    }
}
