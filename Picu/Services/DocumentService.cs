using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Picu.Repositories.Interfaces;
using Picu.Services.Interfaces;
using Picu.ViewModels;

namespace Picu.Services
{
    public class DocumentService : IDocumentService
    {
        private const string PathFile = @"wwwroot\\files\\critically ill pediatric patients in PICU.csv";

        private readonly IPatientRepository _patientRepository;
        private readonly IVitalSignsRepository _vitalSignsRepository;
        private readonly IMongoDbService _mongoDbService;

        #region Constructor

        public DocumentService(
            IPatientRepository patientRepository, 
            IVitalSignsRepository vitalSignsRepository, 
            IMongoDbService mongoDbService)
        {
            _patientRepository = patientRepository;
            _vitalSignsRepository = vitalSignsRepository;
            _mongoDbService = mongoDbService;
        }

        //mongodb+srv://geek_admin:IwKfdcA1P2BrM94T@cluster0.kevu1.mongodb.net/DataScienceDb?retryWrites=true&w=majority

        #endregion

        #region Post

        public string SaveInSql()
        {
            if (_patientRepository.FindAll().Any())
            {
                throw new Exception("Ya está cargada la información");
            }

            var fileExists = File.Exists(PathFile);
            if (!fileExists)
            {
                throw new Exception("No se ha encontrado el archivo");
            }

            var csvRecords = new List<CsvRecordVm>();

            using var reader = new StreamReader(PathFile);
            reader.ReadLine(); // Skip first line (header)

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                var values = line.Split(',');

                var csvRecord = new CsvRecordVm
                {
                    Id = values[0],
                    Age = values[1],
                    Weight = values[2],
                    Height = values[3],
                    Gender = values[4],
                    Diagnosis = values[5],
                    Outcome = values[6],
                    HourEvent = values[7],
                    HeartRate = values[8],
                    OxygenSaturation = values[9],
                    RespiratoryRate = values[10],
                    SystolicBloodPressure = values[11],
                    DiastolicBloodPressure = values[12],
                    MeanBloodPressure = values[13]
                };

                csvRecords.Add(csvRecord);
            }

            var patients = new List<Patient>();
            var vitalSignsList = new List<VitalSigns>();

            foreach (var csvRecord in csvRecords)
            {
                var patientExists = patients.Any(p => p.PatientId == csvRecord.Id);

                if (!patientExists)
                {
                    var patient = GetPatient(csvRecord);
                    patients.Add(patient);
                }

                var vitalSigns = GetVitalSigns(csvRecord);
                vitalSignsList.Add(vitalSigns);
            }

            _patientRepository.AddAll(patients);
            _vitalSignsRepository.AddAll(vitalSignsList);

            return "Se ha cargado la información exitosamente.";
        }

        public async Task<string> SaveInMongoDb()
        {
            if (!_patientRepository.FindAll().Any())
            {
                throw new Exception("No se ha cargado información en la base de datos SQL.");
            }

            var patient = _patientRepository.FindAll().FirstOrDefault();
            var vitalSignsByPatient = _vitalSignsRepository.FindAllByPatientId(patient!.PatientId).ToList();

            var patientVm = new PatientVm
            {
                Age = patient.Age,
                Diagnosis = patient.Diagnosis,
                Gender = patient.Gender,
                Height = patient.Height,
                PatientId = patient.PatientId,
                Survived = patient.Survived,
                Weight = patient.Weight,
                VitalSignsRecords = GetVitalSignsVm(vitalSignsByPatient)
            };

            await _mongoDbService.AddOneAsync(patientVm);

            return "Se ha cargado la información exitosamente.";
        }

        #endregion

        #region Utils

        private Patient GetPatient(CsvRecordVm csvRecord)
        {
            var patient = new Patient
            {
                PatientId = csvRecord.Id,
                Age = csvRecord.Age,
                Gender = csvRecord.Gender,
                Height = csvRecord.Height,
                Weight = csvRecord.Weight,
                Diagnosis = csvRecord.Diagnosis,
                Survived = csvRecord.Outcome == "survived"
            };

            return patient;
        }

        private VitalSigns GetVitalSigns(CsvRecordVm csvRecord)
        {
            _ = int.TryParse(csvRecord.DiastolicBloodPressure, out var diastolicBloodPressure);
            _ = int.TryParse(csvRecord.HeartRate, out var heartRate);
            _ = int.TryParse(csvRecord.MeanBloodPressure, out var meanBloodPressure);
            _ = int.TryParse(csvRecord.OxygenSaturation, out var oxygenSaturation);
            _ = int.TryParse(csvRecord.RespiratoryRate, out var respiratoryRate);
            _ = int.TryParse(csvRecord.SystolicBloodPressure, out var systolicBloodPressure);

            var vitalSigns = new VitalSigns
            {
                PatientId = csvRecord.Id,
                HourEvent = csvRecord.HourEvent,
                DiastolicBloodPressure = diastolicBloodPressure,
                HeartRate = heartRate,
                MeanBloodPressure = meanBloodPressure,
                OxygenSaturation = oxygenSaturation,
                RespiratoryRate = respiratoryRate,
                SystolicBloodPressure = systolicBloodPressure
            };

            return vitalSigns;
        }

        private List<VitalSignsVm> GetVitalSignsVm(List<VitalSigns> vitalSignList)
        {
            var vitalSignsVms = new List<VitalSignsVm>();

            foreach (var vitalSign in vitalSignList)
            {

                var vitalSignsVm = new VitalSignsVm
                {
                    HourEvent = vitalSign.HourEvent,
                    DiastolicBloodPressure = vitalSign.DiastolicBloodPressure,
                    HeartRate = vitalSign.HeartRate,
                    MeanBloodPressure = vitalSign.MeanBloodPressure,
                    OxygenSaturation = vitalSign.OxygenSaturation,
                    RespiratoryRate = vitalSign.RespiratoryRate,
                    SystolicBloodPressure = vitalSign.SystolicBloodPressure,
                    VitalSignsId = vitalSign.VitalSignsId.ToString()
                };

                vitalSignsVms.Add(vitalSignsVm);
            }

            return vitalSignsVms;
        }

        #endregion

    }
}
