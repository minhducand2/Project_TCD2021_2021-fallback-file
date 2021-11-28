using ApiGen.Contracts;
using ApiGen.Data.DataAccess;
using ApiGen.Data.DataManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGen.Infrastructure.Installers
{
    internal class RegisterContractMappings : IServiceRegistration
    {
        public void RegisterAppServices(IServiceCollection services, IConfiguration config)
        {
            // Register Interface Mappings for Repositories
            services.AddTransient<ISinhVienManager, SinhVienManager>();

            services.AddTransient<ID000AccountDataAccess, D000AccountDataAccess>();
            services.AddTransient<ID100MenuDataAccess, D100MenuDataAccess>();
            services.AddTransient<ID200RoleDataAccess, D200RoleDataAccess>();
            services.AddTransient<ID300RoleDetailDataAccess, D300RoleDetailDataAccess>();
            services.AddTransient<ID400BannerDataAccess, D400BannerDataAccess>();
            services.AddTransient<ID500FooterDataAccess, D500FooterDataAccess>();
            services.AddTransient<ID600HeaderInfoDataAccess, D600HeaderInfoDataAccess>();
            services.AddTransient<ID700ShopDataAccess, D700ShopDataAccess>();
            services.AddTransient<ID800ShopComboDataAccess, D800ShopComboDataAccess>();
            services.AddTransient<ID900ShopComboDetailDataAccess, D900ShopComboDetailDataAccess>();
            services.AddTransient<ID1000ShopCommentDataAccess, D1000ShopCommentDataAccess>();
            services.AddTransient<ID1100ShopCategoriesDataAccess, D1100ShopCategoriesDataAccess>();
            services.AddTransient<ID1200MealPlanTypeDataAccess, D1200MealPlanTypeDataAccess>();
            services.AddTransient<ID1300BlogCategoriesDataAccess, D1300BlogCategoriesDataAccess>();
            services.AddTransient<ID1400BlogDataAccess, D1400BlogDataAccess>();
            services.AddTransient<ID1500ContactInfoDataAccess, D1500ContactInfoDataAccess>();
            services.AddTransient<ID1600ContactStatusDataAccess, D1600ContactStatusDataAccess>();
            services.AddTransient<ID1700ContactUsDataAccess, D1700ContactUsDataAccess>();
            services.AddTransient<ID1800UserStatusDataAccess, D1800UserStatusDataAccess>();
            services.AddTransient<ID1900RoleUserDataAccess, D1900RoleUserDataAccess>();
            services.AddTransient<ID2000UserDataAccess, D2000UserDataAccess>();
            services.AddTransient<ID2100PromotionDataAccess, D2100PromotionDataAccess>();
            services.AddTransient<ID2200OrderStatusDataAccess, D2200OrderStatusDataAccess>();
            services.AddTransient<ID2300PaymentStatusDataAccess, D2300PaymentStatusDataAccess>();
            services.AddTransient<ID2400PaymentTypeDataAccess, D2400PaymentTypeDataAccess>();
            services.AddTransient<ID2500CityDataAccess, D2500CityDataAccess>();
            services.AddTransient<ID2600DistrictDataAccess, D2600DistrictDataAccess>();
            services.AddTransient<ID2700ProductTypeDataAccess, D2700ProductTypeDataAccess>();
            services.AddTransient<ID2800OrderShopDataAccess, D2800OrderShopDataAccess>();
            services.AddTransient<ID2900OrderDetailDataAccess, D2900OrderDetailDataAccess>();
            services.AddTransient<ID3000CommentStatusDataAccess, D3000CommentStatusDataAccess>();
            services.AddTransient<ID3100MyPromotionDataAccess, D3100MyPromotionDataAccess>();
            services.AddTransient<ID3200InputProductDataAccess, D3200InputProductDataAccess>();
            services.AddTransient<ID3300WarehouseDataAccess, D3300WarehouseDataAccess>();

        }
    }
}
