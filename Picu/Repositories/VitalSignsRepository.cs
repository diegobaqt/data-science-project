using Picu.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Picu.Repositories.Interfaces;

namespace Picu.Repositories
{
    public class VitalSignsRepository : IVitalSignsRepository
    {
        private readonly IPicuService _picuService;

        #region Constructor

        public VitalSignsRepository(IPicuService picuService)
        {
            _picuService = picuService;
        }

        #endregion

        #region Get

        public IQueryable<VitalSigns> FindAllByPatientId(string patientId)
        {
            return _picuService.Set<VitalSigns>().Where(vs => vs.PatientId == patientId);
        }

        #endregion

        #region Post

        public void AddAll(List<VitalSigns> vitalSignsList)
        {
            _picuService.AddAll(vitalSignsList);
        }

        #endregion
    }
}
