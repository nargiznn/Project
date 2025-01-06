using System;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Repository.Repositories.Interfaces;

namespace Repository
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
		{
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<IWelcomeInfoRepository, WelcomeInfoRepository>();
            services.AddScoped<IWelcomeImageRepository, WelcomeImageRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();
            services.AddScoped<ISpecialCategoryRepository, SpecialCategoryRepository>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<IAwardRepository, AwardRepository>();
            services.AddScoped<IAwardLogoRepository, AwardLogoRepository>();

            services.AddScoped<ICuisineRepository, CuisineRepository>();

            services.AddScoped<ISettingRepository, SettingRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
		}
	}
}

