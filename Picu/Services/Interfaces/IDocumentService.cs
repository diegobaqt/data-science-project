using System.Threading.Tasks;

namespace Picu.Services.Interfaces
{
    public interface IDocumentService
    {
        string SaveInSql();

        Task<string> SaveInMongoDb();
    }
}
