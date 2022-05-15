using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Picu.ViewModels
{
    public class PatientVm
    {
        [BsonId]
        public string PatientId { get; set; }

        public string Age { get; set; }

        public string Weight { get; set; }

        public string Height { get; set; }

        public string Gender { get; set; }

        public string Diagnosis { get; set; }

        public bool Survived { get; set; }

        [BsonElement("VitalSignsVm")]
        [JsonPropertyName("VitalSignsRecords")]
        public List<VitalSignsVm> VitalSignsRecords { get; set; }
    }

    public class VitalSignsVm
    {
        [BsonId]
        public string VitalSignsId { get; set; }

        public string HourEvent { get; set; }

        public int? HeartRate { get; set; }

        public int? OxygenSaturation { get; set; }

        public int? RespiratoryRate { get; set; }

        public int? SystolicBloodPressure { get; set; }

        public int? DiastolicBloodPressure { get; set; }

        public int? MeanBloodPressure { get; set; }
    }
}
