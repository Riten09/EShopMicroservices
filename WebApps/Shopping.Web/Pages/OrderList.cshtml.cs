namespace Shopping.Web.Pages
{
    public class OrderListModel(IOrderingService orderingService,ILogger<OrderListModel> logger) : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            //assumption customerId is passed in from the UI authentication user: swn
            var customerId = new Guid("b566d21c-0496-4d8c-b597-af4decc36e05");

            var response = await orderingService.GetOrdersByCustomer(customerId);

            Orders = response.Orders;

            return Page();
        }
    }
}
