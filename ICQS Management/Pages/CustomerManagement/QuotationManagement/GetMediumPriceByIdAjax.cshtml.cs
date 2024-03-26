using DataAccessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ICQS_Management.Pages.CustomerManagement.QuotationManagement
{
    public class GetMediumPriceByIdAjaxModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var mediumPrice = MaterialManagementService.GetMediumPriceById(id);
            return new JsonResult(mediumPrice);
        }
    }
}
