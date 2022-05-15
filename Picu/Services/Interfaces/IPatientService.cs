using Domain.Model;
using System.Collections.Generic;

namespace Picu.Services.Interfaces
{
    public interface IPatientService
    {
        List<Patient> GetAll();

        Patient GetById(string patientId);
    }
}
