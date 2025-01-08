using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Helpers.DTOs.Account;
using Service.Helpers.DTOs.Award;
using Service.Helpers.DTOs.Chef;
using Service.Helpers.DTOs.Cuisine;
using Service.Helpers.DTOs.Customer;
using Service.Helpers.DTOs.Event;
using Service.Helpers.DTOs.FoodCategory;
using Service.Helpers.DTOs.Logo;
using Service.Helpers.DTOs.MenuCategory;
using Service.Helpers.DTOs.Product;
using Service.Helpers.DTOs.Setting;
using Service.Helpers.DTOs.Slider;
using Service.Helpers.DTOs.SocialMediaLink;
using Service.Helpers.DTOs.SpecialCategory;
using Service.Helpers.DTOs.Tag;
using Service.Helpers.DTOs.WelcomeImage;
using Service.Helpers.DTOs.WelcomeInfo;
using Service.Helpers.DTOs.Banner;
using Service.Helpers.Faqs;
using Service.Helpers.DTOs.GalleryCategory;
using Service.Helpers.DTOs.GalleryImage;

namespace Service.Helpers.Mapping
{
    public class MappingProfile:Profile
	{
        public MappingProfile()
        {
            CreateMap<ChefCreateDto, Chef>()
                       .ForMember(dest => dest.SocialMedia, opt => opt.MapFrom(src => new SocialMediaLink
                       {
                           FacebookUrl = src.SocialMedia.FacebookUrl,
                           TwitterUrl = src.SocialMedia.TwitterUrl,
                           InstagramUrl = src.SocialMedia.InstagramUrl
                       }))
                       .ForMember(dest => dest.ChefPosition, opt => opt.MapFrom(src => src.PositionIds.Select(positionId => new ChefPosition
                       {
                           PositionId = positionId
                       }).ToList()));

            CreateMap<Chef, ChefDto>()
                .ForMember(dest => dest.Positions,
                    opt => opt.MapFrom(src => src.ChefPosition.Select(cp => cp.Position.Title)))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.ChefImages.Select(ci => ci.Path)))
                .ForMember(dest => dest.SocialMedia,
                    opt => opt.MapFrom(src => new SocialMediaLinkDto
                    {
                        FacebookUrl = src.SocialMedia.FacebookUrl,
                        TwitterUrl = src.SocialMedia.TwitterUrl,
                        InstagramUrl = src.SocialMedia.InstagramUrl
                    }));

            CreateMap<ChefEditDto, Chef>()
                       .ForMember(dest => dest.SocialMedia, opt => opt.MapFrom(src => new SocialMediaLink
                       {
                           FacebookUrl = src.SocialMedia.FacebookUrl,
                           TwitterUrl = src.SocialMedia.TwitterUrl,
                           InstagramUrl = src.SocialMedia.InstagramUrl
                       })).ForAllMembers(opts =>
                       {
                           opts.AllowNull();
                           opts.Condition((src, dest, srcMember) => srcMember != null);
                       });

            CreateMap<Cuisine, CuisineDto>();
            CreateMap<CuisineCreateDto, Cuisine>();
            CreateMap<CuisineEditDto, Cuisine>()
             .ForAllMembers(opts =>
             {
                 opts.Condition((src, dest, srcMember) => srcMember != null);
             });



            CreateMap<Slider, SliderDto>();
            CreateMap<SliderCreateDto, Slider>();
            CreateMap<SliderEditDto, Slider>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<Banner, BannerDto>();
            CreateMap<BannerCreateDto, Banner>();
            CreateMap<BannerEditDto, Banner>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<WelcomeInfo, WelcomeInfoDto>();
            CreateMap<WelcomeInfoCreateDto, WelcomeInfo>();
            CreateMap<WelcomeInfoEditDto, WelcomeInfo>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<WelcomeImage, WelcomeImageDto>();
            CreateMap<WelcomeImageCreateDto, WelcomeImage>();
            CreateMap<WelcomeImageEditDto, WelcomeImage>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<Event, EventDto>().ForMember(dest => dest.Tags, opt =>
        opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()));

            CreateMap<EventCreateDto, Event>().ForMember(dest => dest.Tags, opt => opt.Ignore());
            CreateMap<EventEditDto, Event>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<MenuCategory, MenuCategoryDto>();
            CreateMap<MenuCategoryCreateDto, MenuCategory>();
            CreateMap<MenuCategoryEditDto, MenuCategory>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<FoodCategory, FoodCategoryDto>();
            CreateMap<FoodCategoryCreateDto, FoodCategory>();
            CreateMap<FoodCategoryEditDto, FoodCategory>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<SpecialCategory, SpecialCategoryDto>();
            CreateMap<SpecialCategoryCreateDto, SpecialCategory>();
            CreateMap<SpecialCategoryEditDto, SpecialCategory>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<Tag, TagDto>();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagEditDto, Tag>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerEditDto, Customer>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<Award, AwardDto>();
            CreateMap<AwardCreateDto, Award>();
            CreateMap<AwardEditDto, Award>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<AwardLogo, LogoDto>();
            CreateMap<LogoCreateDto, AwardLogo>();
            CreateMap<LogoEditDto, AwardLogo>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<Setting, SettingDto>();
            CreateMap<SettingEditDto, Setting>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });



            #region Account
            CreateMap<SignUpDto, AppUser>();
            CreateMap<AppUser, UserDto>();
            CreateMap<IdentityRole, RoleDto>();
            #endregion

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.MenuCategoryName, opt => opt.MapFrom(src => src.MenuCategory.Name))
                .ForMember(dest => dest.SpecialCategoryName, opt => opt.MapFrom(src => src.SpecialCategory.Name))
                .ForMember(dest => dest.FoodCategoryName, opt => opt.MapFrom(src => src.FoodCategory.Name))
                .ForMember(dest => dest.ProductCuisineName, opt => opt.MapFrom(src => src.Cuisine.Name))
               .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.Path).ToList()));

            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductEditDto, Product>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<Event, EventDto>().ForMember(dest => dest.Tags, opt =>
opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()));

            CreateMap<EventCreateDto, Event>().ForMember(dest => dest.Tags, opt => opt.Ignore());
            CreateMap<EventEditDto, Event>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<Faq, FaqDto>();
            CreateMap<GalleryCategory, GalleryCategoryDto>();
            CreateMap<GalleryImage, GalleryImageDto>()
                       .ForMember(dest => dest.GalleryCategoryName, opt => opt.MapFrom(src => src.GalleryCategory.Name));


        }
        }
    }

