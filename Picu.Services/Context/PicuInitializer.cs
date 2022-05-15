using System.Linq;

namespace Picu.Services.Context
{
    public class PicuInitializer
    {
        public void Initialize(PicuContext context)
        {
            context.Database.EnsureCreated();
            if (context.Patient.Any()) return;
        }
    }
}
