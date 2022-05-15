using Domain.Model;
using Picu.Repositories.Interfaces;
using Picu.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Picu.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        #region Constructor

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        #endregion

        #region Get

        public List<Patient> GetAll()
        {
            return _patientRepository.FindAll().ToList();
        }

        public Patient GetById(string patientId)
        {
            var patient = _patientRepository.FindAll().FirstOrDefault(p => p.PatientId == patientId);
            if (patient == null) throw new Exception("Paciente no encontrado");

            return patient;
        }

        #endregion
    }
}
