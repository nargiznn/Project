using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Helpers.DTOs.Account;
using Service.Helpers.DTOs.Award;
using Service.Helpers.DTOs.Chef;
using Service.Helpers.DTOs.Cuisine;
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
using Service.Helpers.DTOs.Client;
using Service.Helpers.DTOs.Statistic;
using Service.Helpers.DTOs.MealPackage;
using Service.Helpers.DTOs.LunchSet;
using Service.Helpers.LunchSetProduct;
using Service.Helpers.DTOs.Testimonial;
using Service.Helpers.DTOs.Table;
using Service.Helpers.DTOs.Faq;
using Service.Helpers.DTOs.Subscribe;
using Service.Helpers.DTOs.Comment;
using Domain.Enum;
using System.Reflection.Metadata;
using Service.Helpers.DTOs.AwardLogo;

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
            #region Cuisine
            CreateMap<Cuisine, CuisineDto>();
            CreateMap<CuisineCreateDto, Cuisine>();
            CreateMap<CuisineEditDto, Cuisine>()
             .ForAllMembers(opts =>
             {
                 opts.Condition((src, dest, srcMember) => srcMember != null);
             });

            #endregion

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

            #region Comment and reply
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<ReplyCreateDto, CommentReply>();
            CreateMap<Comment, CommentDto>()
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
           .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.CommentReplies)).ReverseMap();
            CreateMap<CommentReply, CommentReplyDto>() .ReverseMap();
            CreateMap<CommentReply, ReplyDto>().ReverseMap();
            #endregion

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

            #region Event
            CreateMap<Event, EventDto>().ForMember(dest => dest.Tags, opt =>
        opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()));

            CreateMap<EventCreateDto, Event>().ForMember(dest => dest.Tags, opt => opt.Ignore());
            CreateMap<EventEditDto, Event>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion




            #region MenuCategory
            CreateMap<MenuCategory, MenuCategoryDto>();
            CreateMap<MenuCategoryCreateDto, MenuCategory>();
            CreateMap<MenuCategoryEditDto, MenuCategory>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            #endregion

            CreateMap<FoodCategory, FoodCategoryDto>();
            CreateMap<FoodCategoryCreateDto, FoodCategory>();
            CreateMap<FoodCategoryEditDto, FoodCategory>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            #region SpecialCategory
            CreateMap<SpecialCategory, SpecialCategoryDto>();
            CreateMap<SpecialCategoryCreateDto, SpecialCategory>();
            CreateMap<SpecialCategoryEditDto, SpecialCategory>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion
            #region Tag
            CreateMap<Tag, TagDto>();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<TagEditDto, Tag>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion

            CreateMap<Testimonial, TestimonialDto>();
            CreateMap<TestimonialCreateDto, Testimonial>();
            CreateMap<TestimonialEditDto, Testimonial>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            #region Award
            CreateMap<Award, AwardDto>()
                .ForMember(x => x.Year, opt => opt.MapFrom(src => src.Year.ToString("yyyy")));

            CreateMap<AwardCreateDto, Award>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year));

            CreateMap<AwardEditDto, Award>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            #endregion

            #region Faq
            CreateMap<Faq, FaqDto>();
            CreateMap<FaqCreateDto, Faq>();
            CreateMap<FaqEditDto, Faq>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion

            #region Subscribe
            CreateMap<SubscribeCreateDto, Subscribe>();
            CreateMap<Subscribe, SubscribeDto>();
            #endregion

            #region AwardLogo
            CreateMap<AwardLogo, AwardLogoDto>();
            CreateMap<AwardLogoCreateDto, AwardLogo>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image.FileName));
            CreateMap<AwardLogoEditDto, AwardLogo>()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                });
            #endregion



            //CreateMap<AwardLogo, LogoDto>();
            //CreateMap<LogoCreateDto, AwardLogo>();
            //CreateMap<LogoEditDto, AwardLogo>()
            //.ForAllMembers(opts =>
            //{
            //    opts.AllowNull();
            //    opts.Condition((src, dest, srcMember) => srcMember != null);
            //});

            CreateMap<Setting, SettingDto>();
            CreateMap<SettingEditDto, Setting>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });

            CreateMap<Statistic, StatisticDto>();


            #region LunchSet
            CreateMap<LunchSet, LunchSetDto>().ForMember(dest => dest.ProductNames, opt =>
opt.MapFrom(src => src.LunchSetProducts.Select(t => t.ProductId).ToList()));
            CreateMap<LunchSetCreateDto, LunchSet>();
            CreateMap<LunchSetEditDto, LunchSet>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion
            CreateMap<Domain.Entities.LunchSetProduct, LunchSetProductDto>();

            #region MealPackage
            CreateMap<MealPackage, MealPackageDto>()
 .ForMember(dest => dest.ProductNames, opt =>
opt.MapFrom(src => src.MealPackageProducts.Select(t => t.ProductId).ToList()));
            CreateMap<MealPackageCreateDto, MealPackage>();
            CreateMap<MealPackageEditDto, MealPackage>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });
            #endregion





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




            CreateMap<RestaurantTable, RestaurantTableDto>();
            CreateMap<Client, ClientDto>();
            CreateMap<GalleryCategory, GalleryCategoryDto>();
            CreateMap<GalleryImage, GalleryImageDto>()
                       .ForMember(dest => dest.GalleryCategoryName, opt => opt.MapFrom(src => src.GalleryCategory.Name));


        }
        }
    }

