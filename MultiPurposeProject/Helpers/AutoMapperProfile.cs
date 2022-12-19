namespace MultiPurposeProject.Helpers;

using AutoMapper;
using MultiPurposeProject.Entities;
using MultiPurposeProject.Models.Users;
using MultiPurposeProject.Models.Products;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User -> AuthenticateResponse
        CreateMap<User, AuthenticateResponse>();

        // RegisterRequest -> User
        CreateMap<Models.Users.RegisterRequest, User>();

        // UpdateRequest -> User
        CreateMap<Models.Users.UpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));

        // CreateRequest -> Product
        CreateMap<Models.Products.CreateRequest, Product>();

        // UpdateRequest -> Product
        CreateMap<Models.Products.UpdateRequest, Product>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));
    }
}