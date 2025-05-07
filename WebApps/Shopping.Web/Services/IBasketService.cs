namespace Shopping.Web.Services
{
    public interface IBasketService
    {
        [Get("/basket-service/basket/{userName}")]
        Task<GetBasketResponse> GetBasket(string userName);
        [Post("/basket-service/basket")]
        Task<StoreBasketResponse> StoreBasket(StoreBasketRequest storeBasketRequest);
        [Delete("/basket-service/basket/{userName}")]
        Task<DeleteBasketResponse> DeleteBasket(string userName);
        [Post("/basket-service/basket/checkout")]
        Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest Request);


        //This method will be geting or creating a new basket 
        //this is IBasket interface default method, because c# has default interface methods feature-
        //-that comes in c# 8.0, that we can explicity implement interface methods
        public async Task<ShoppingCartModel> LoadUserBasket()
        {
            //Get Basket If Not Exist Create New Basket with Default logged in User Name: swn
            var userName = "swn";
            ShoppingCartModel basket;

            try
            {
                var getBasketResponse = await GetBasket(userName);
                basket = getBasketResponse.Cart;
            }
            catch (ApiException apiException) when(apiException.StatusCode == HttpStatusCode.NotFound)
            {
                basket = new ShoppingCartModel
                {
                    UserName = userName,
                    Items = []
                };
            }
            return basket;
        }
    }
}
