using System.Collections.Generic;
using Domain.Model;
using System.Linq;

namespace Picu.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        IQueryable<Patient> FindAll();

        void AddAll(List<Patient> patients);
    }
}
