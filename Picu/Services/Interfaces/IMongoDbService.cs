using Picu.ViewModels;
using System.Threading.Tasks;

namespace Picu.Services.Interfaces
{
    public interface IMongoDbService
    {
        Task AddOneAsync(PatientVm patient);

        Task<PatientVm> GetByIdAsync(string patientId);
    }
}
