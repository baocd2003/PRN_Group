using BussinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IQuotationManagementRepository
    {
        IEnumerable<Quotation> GetAllQuotations();
        Quotation AddQuotation (Quotation quotation, Project project, List<ProjectMaterial> projectMaterial);
        Quotation GetQuotation(Guid id);
    }
}
