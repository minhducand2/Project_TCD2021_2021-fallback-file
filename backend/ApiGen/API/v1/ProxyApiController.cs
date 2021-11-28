using ApiGen.Contracts;
using ApiGen.Data.DataAccess;
using ApiGen.DTO.Response;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ApiGen.API.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProxyApiController : ControllerBase
    {
        private readonly ILogger<ProxyApiController> _logger;
        private readonly IApiConnect _apiConnect;

        private C000AccountController _c000AccountController;
        private C100MenuController _c100MenuController;
        private C200RoleController _c200RoleController;
        private C300RoleDetailController _c300roleDetailController;
        private C400BannerController _c400BannerController; 
        private C500FooterController _c500FooterController; 
        private C600HeaderInfoController _c600HeaderInfoController; 
        private C700ShopController _c700ShopController; 
        private C800ShopComboController _c800ShopComboController; 
        private C900ShopComboDetailController _c900ShopComboDetailController; 
        private C1000ShopCommentController _c1000ShopCommentController; 
        private C1100ShopCategoriesController _c1100ShopCategoriesController; 
        private C1200MealPlanTypeController _c1200MealPlanTypeController; 
        private C1300BlogCategoriesController _c1300BlogCategoriesController; 
        private C1400BlogController _c1400BlogController; 
        private C1500ContactInfoController _c1500ContactInfoController; 
        private C1600ContactStatusController _c1600ContactStatusController; 
        private C1700ContactUsController _c1700ContactUsController; 
        private C1800UserStatusController _c1800UserStatusController; 
        private C1900RoleUserController _c1900RoleUserController; 
        private C2000UserController _c2000UserController; 
        private C2100PromotionController _c2100PromotionController; 
        private C2200OrderStatusController _c2200OrderStatusController; 
        private C2300PaymentStatusController _c2300PaymentStatusController; 
        private C2400PaymentTypeController _c2400PaymentTypeController; 
        private C2500CityController _c2500CityController; 
        private C2600DistrictController _c2600DistrictController; 
        private C2700ProductTypeController _c2700ProductTypeController; 
        private C2800OrderShopController _c2800OrderShopController; 
        private C2900OrderDetailController _c2900OrderDetailController; 
        private C3000CommentStatusController _c3000CommentStatusController; 
        private C3100MyPromotionController _c3100MyPromotionController; 
        private C3200InputProductController _c3200InputProductController; 
        private C3300WarehouseController _c3300WarehouseController; 


        public ProxyApiController(IApiConnect apiConnect,
            ILogger<ProxyApiController> logger,
            ID000AccountDataAccess c000AccountDataAccess,
            ID100MenuDataAccess c100MenuDataAccess,
            ID200RoleDataAccess c200RoleDataAccess,
            ID300RoleDetailDataAccess roleDetailDataAccess,
            ID400BannerDataAccess d400BannerDataAccess, 
            ID500FooterDataAccess d500FooterDataAccess, 
            ID600HeaderInfoDataAccess d600HeaderInfoDataAccess, 
            ID700ShopDataAccess d700ShopDataAccess, 
            ID800ShopComboDataAccess d800ShopComboDataAccess, 
            ID900ShopComboDetailDataAccess d900ShopComboDetailDataAccess, 
            ID1000ShopCommentDataAccess d1000ShopCommentDataAccess, 
            ID1100ShopCategoriesDataAccess d1100ShopCategoriesDataAccess, 
            ID1200MealPlanTypeDataAccess d1200MealPlanTypeDataAccess, 
            ID1300BlogCategoriesDataAccess d1300BlogCategoriesDataAccess, 
            ID1400BlogDataAccess d1400BlogDataAccess, 
            ID1500ContactInfoDataAccess d1500ContactInfoDataAccess, 
            ID1600ContactStatusDataAccess d1600ContactStatusDataAccess, 
            ID1700ContactUsDataAccess d1700ContactUsDataAccess, 
            ID1800UserStatusDataAccess d1800UserStatusDataAccess, 
            ID1900RoleUserDataAccess d1900RoleUserDataAccess, 
            ID2000UserDataAccess d2000UserDataAccess, 
            ID2100PromotionDataAccess d2100PromotionDataAccess, 
            ID2200OrderStatusDataAccess d2200OrderStatusDataAccess, 
            ID2300PaymentStatusDataAccess d2300PaymentStatusDataAccess, 
            ID2400PaymentTypeDataAccess d2400PaymentTypeDataAccess, 
            ID2500CityDataAccess d2500CityDataAccess, 
            ID2600DistrictDataAccess d2600DistrictDataAccess, 
            ID2700ProductTypeDataAccess d2700ProductTypeDataAccess, 
            ID2800OrderShopDataAccess d2800OrderShopDataAccess, 
            ID2900OrderDetailDataAccess d2900OrderDetailDataAccess, 
            ID3000CommentStatusDataAccess d3000CommentStatusDataAccess, 
            ID3100MyPromotionDataAccess d3100MyPromotionDataAccess, 
            ID3200InputProductDataAccess d3200InputProductDataAccess, 
            ID3300WarehouseDataAccess d3300WarehouseDataAccess, 

            IMapper mapper)
        {
            _apiConnect = apiConnect;
            _logger = logger;

            _c000AccountController = new C000AccountController(c000AccountDataAccess, mapper, logger);
            _c100MenuController = new C100MenuController(c100MenuDataAccess, mapper, logger);
            _c200RoleController = new C200RoleController(c200RoleDataAccess, mapper, logger);
            _c300roleDetailController = new C300RoleDetailController(roleDetailDataAccess, mapper, logger);
            _c400BannerController = new C400BannerController(d400BannerDataAccess, mapper, logger); 
            _c500FooterController = new C500FooterController(d500FooterDataAccess, mapper, logger); 
            _c600HeaderInfoController = new C600HeaderInfoController(d600HeaderInfoDataAccess, mapper, logger); 
            _c700ShopController = new C700ShopController(d700ShopDataAccess, mapper, logger); 
            _c800ShopComboController = new C800ShopComboController(d800ShopComboDataAccess, mapper, logger); 
            _c900ShopComboDetailController = new C900ShopComboDetailController(d900ShopComboDetailDataAccess, mapper, logger); 
            _c1000ShopCommentController = new C1000ShopCommentController(d1000ShopCommentDataAccess, mapper, logger); 
            _c1100ShopCategoriesController = new C1100ShopCategoriesController(d1100ShopCategoriesDataAccess, mapper, logger); 
            _c1200MealPlanTypeController = new C1200MealPlanTypeController(d1200MealPlanTypeDataAccess, mapper, logger); 
            _c1300BlogCategoriesController = new C1300BlogCategoriesController(d1300BlogCategoriesDataAccess, mapper, logger); 
            _c1400BlogController = new C1400BlogController(d1400BlogDataAccess, mapper, logger); 
            _c1500ContactInfoController = new C1500ContactInfoController(d1500ContactInfoDataAccess, mapper, logger); 
            _c1600ContactStatusController = new C1600ContactStatusController(d1600ContactStatusDataAccess, mapper, logger); 
            _c1700ContactUsController = new C1700ContactUsController(d1700ContactUsDataAccess, mapper, logger); 
            _c1800UserStatusController = new C1800UserStatusController(d1800UserStatusDataAccess, mapper, logger); 
            _c1900RoleUserController = new C1900RoleUserController(d1900RoleUserDataAccess, mapper, logger); 
            _c2000UserController = new C2000UserController(d2000UserDataAccess, mapper, logger); 
            _c2100PromotionController = new C2100PromotionController(d2100PromotionDataAccess, mapper, logger); 
            _c2200OrderStatusController = new C2200OrderStatusController(d2200OrderStatusDataAccess, mapper, logger); 
            _c2300PaymentStatusController = new C2300PaymentStatusController(d2300PaymentStatusDataAccess, mapper, logger); 
            _c2400PaymentTypeController = new C2400PaymentTypeController(d2400PaymentTypeDataAccess, mapper, logger); 
            _c2500CityController = new C2500CityController(d2500CityDataAccess, mapper, logger); 
            _c2600DistrictController = new C2600DistrictController(d2600DistrictDataAccess, mapper, logger); 
            _c2700ProductTypeController = new C2700ProductTypeController(d2700ProductTypeDataAccess, mapper, logger); 
            _c2800OrderShopController = new C2800OrderShopController(d2800OrderShopDataAccess, d2900OrderDetailDataAccess, mapper, logger); 
            _c2900OrderDetailController = new C2900OrderDetailController(d2900OrderDetailDataAccess, d3300WarehouseDataAccess, mapper, logger); 
            _c3000CommentStatusController = new C3000CommentStatusController(d3000CommentStatusDataAccess, mapper, logger); 
            _c3100MyPromotionController = new C3100MyPromotionController(d3100MyPromotionDataAccess, mapper, logger); 
            _c3200InputProductController = new C3200InputProductController(d3200InputProductDataAccess, mapper, logger); 
            _c3300WarehouseController = new C3300WarehouseController(d3300WarehouseDataAccess, mapper, logger); 

        }

        [HttpPost("SelectAllByWhat")]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status201Created)]
        //[ProducesResponseType(typeof(SinhVienQueryResponse), Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), Status429TooManyRequests)]
        public async Task<string> SelectAllByWhat([FromBody] dynamic paramRequest)
        {
            int what = paramRequest.what;
            switch ((what / 100) * 100)
            {
                // Redirect to Lop controller
                case 0:
                     
                    return await _c000AccountController.execute(what, paramRequest);

                // Redirect to Menu controller
                case 100:
                    
                    return await _c100MenuController.execute(what, paramRequest);

                // Redirect to Role controller
                case 200:
                    return await _c200RoleController.execute(what, paramRequest);

                // Redirect to RoleDetail controller
                case 300:
                    return await _c300roleDetailController.execute(what, paramRequest);

                // Redirect to Banner controller 
                case 400: 
                    return await _c400BannerController.execute(what, paramRequest); 
 
                // Redirect to Footer controller 
                case 500: 
                    return await _c500FooterController.execute(what, paramRequest); 
 
                // Redirect to HeaderInfo controller 
                case 600: 
                    return await _c600HeaderInfoController.execute(what, paramRequest); 
 
                // Redirect to Shop controller 
                case 700: 
                    return await _c700ShopController.execute(what, paramRequest); 
 
                // Redirect to ShopCombo controller 
                case 800: 
                    return await _c800ShopComboController.execute(what, paramRequest); 
 
                // Redirect to ShopComboDetail controller 
                case 900: 
                    return await _c900ShopComboDetailController.execute(what, paramRequest); 
 
                // Redirect to ShopComment controller 
                case 1000: 
                    return await _c1000ShopCommentController.execute(what, paramRequest); 
 
                // Redirect to ShopCategories controller 
                case 1100: 
                    return await _c1100ShopCategoriesController.execute(what, paramRequest); 
 
                // Redirect to MealPlanType controller 
                case 1200: 
                    return await _c1200MealPlanTypeController.execute(what, paramRequest); 
 
                // Redirect to BlogCategories controller 
                case 1300: 
                    return await _c1300BlogCategoriesController.execute(what, paramRequest); 
 
                // Redirect to Blog controller 
                case 1400: 
                    return await _c1400BlogController.execute(what, paramRequest); 
 
                // Redirect to ContactInfo controller 
                case 1500: 
                    return await _c1500ContactInfoController.execute(what, paramRequest); 
 
                // Redirect to ContactStatus controller 
                case 1600: 
                    return await _c1600ContactStatusController.execute(what, paramRequest); 
 
                // Redirect to ContactUs controller 
                case 1700: 
                    return await _c1700ContactUsController.execute(what, paramRequest); 
 
                // Redirect to UserStatus controller 
                case 1800: 
                    return await _c1800UserStatusController.execute(what, paramRequest); 
 
                // Redirect to RoleUser controller 
                case 1900: 
                    return await _c1900RoleUserController.execute(what, paramRequest); 
 
                // Redirect to User controller 
                case 2000: 
                    return await _c2000UserController.execute(what, paramRequest); 
 
                // Redirect to Promotion controller 
                case 2100: 
                    return await _c2100PromotionController.execute(what, paramRequest); 
 
                // Redirect to OrderStatus controller 
                case 2200: 
                    return await _c2200OrderStatusController.execute(what, paramRequest); 
 
                // Redirect to PaymentStatus controller 
                case 2300: 
                    return await _c2300PaymentStatusController.execute(what, paramRequest); 
 
                // Redirect to PaymentType controller 
                case 2400: 
                    return await _c2400PaymentTypeController.execute(what, paramRequest); 
 
                // Redirect to City controller 
                case 2500: 
                    return await _c2500CityController.execute(what, paramRequest); 
 
                // Redirect to District controller 
                case 2600: 
                    return await _c2600DistrictController.execute(what, paramRequest); 
 
                // Redirect to ProductType controller 
                case 2700: 
                    return await _c2700ProductTypeController.execute(what, paramRequest); 
 
                // Redirect to OrderShop controller 
                case 2800: 
                    return await _c2800OrderShopController.execute(what, paramRequest); 
 
                // Redirect to OrderDetail controller 
                case 2900: 
                    return await _c2900OrderDetailController.execute(what, paramRequest); 
 
                // Redirect to CommentStatus controller 
                case 3000: 
                    return await _c3000CommentStatusController.execute(what, paramRequest); 
 
                // Redirect to MyPromotion controller 
                case 3100: 
                    return await _c3100MyPromotionController.execute(what, paramRequest); 
 
                // Redirect to InputProduct controller 
                case 3200: 
                    return await _c3200InputProductController.execute(what, paramRequest); 
 
                // Redirect to Warehouse controller 
                case 3300: 
                    return await _c3300WarehouseController.execute(what, paramRequest); 
 

            }
            return null;
        }
    }
}
