using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Picu.Services.Interfaces;
using Picu.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Picu.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IConfiguration _config;
        private readonly IMongoCollection<PatientVm> _patientCollection;

        public MongoDbService(IConfiguration config)
        {
            _config = config;

            var settings = new MongoDbSettings
            {
                ConnectionUri = _config["MongoDB:ConnectionUri"],
                DatabaseName = _config["MongoDB:DatabaseName"],
                CollectionName = _config["MongoDB:CollectionName"]
            };

            var client = new MongoClient(settings.ConnectionUri);
            var database = client.GetDatabase(settings.DatabaseName);
            _patientCollection = database.GetCollection<PatientVm>(settings.CollectionName);

        }

        public async Task<PatientVm> GetByIdAsync(string patientId)
        {
            var result = await _patientCollection.Find(x => x.PatientId == patientId).ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task AddOneAsync(PatientVm patient)
        {
            await _patientCollection.InsertOneAsync(patient);
        }

    }
}
