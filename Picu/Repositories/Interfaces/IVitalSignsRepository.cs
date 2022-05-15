using Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace Picu.Repositories.Interfaces
{
    public interface IVitalSignsRepository
    {
        IQueryable<VitalSigns> FindAllByPatientId(string patientId);

        void AddAll(List<VitalSigns> vitalSignsList);
    }
}
