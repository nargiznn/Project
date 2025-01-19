using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Service.Services.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IChefService, ChefService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IWelcomeInfoService, WelcomeInfoService>();
            services.AddScoped<IWelcomeImageService, WelcomeImageService>();
            services.AddScoped<IEventService, EventService>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFaqService, FaqService>();

            services.AddScoped<IMealPackageService, MealPackageService>();
            services.AddScoped<ILunchSetService, LunchSetService>();

            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IRestaurantTableService, RestaurantTableService>();
            services.AddScoped<ICuisineService, CuisineService>();

            services.AddScoped<IGalleryCategoryService, GalleryCategoryService>();
            services.AddScoped<IGalleryImageService, GalleryImageService>();

            services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<ISpecialCategoryService, SpecialCategoryService>();

            services.AddScoped<ITestimonialService, TestimonialService>();

            services.AddScoped<IAwardService, AwardService>();
            services.AddScoped<IAwardLogoService, AwardLogoService>();

            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IStatisticService, StatisticService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}

