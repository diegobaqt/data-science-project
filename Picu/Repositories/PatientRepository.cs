using System;
using Domain.Model;
using Picu.Repositories.Interfaces;
using Picu.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Picu.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IPicuService _picuService;

        #region Constructor

        public PatientRepository(IPicuService picuService)
        {
            _picuService = picuService;
        }

        #endregion

        #region Get

        public IQueryable<Patient> FindAll()
        {
            return _picuService.Set<Patient>();
        }

        #endregion

        #region Post

        public void AddAll(List<Patient> patients)
        {
            _picuService.AddAll(patients);
        }

        #endregion
    }
}
