using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultiPurposeProject.Authorization;
using MultiPurposeProject.Entities;
using MultiPurposeProject.Helpers;
using MultiPurposeProject.Models.Users;
using MultiPurposeProject.Services;

namespace MultiPurposeProjectUnitTest.Services;


public class UserServiceFake : IUserService
{

    private readonly List<User> _users;
    
    public UserServiceFake()
    {      
        _users = new List<User>() { 
            new User(){ Id = 1, Name = "Test User", Username = "testUser", PasswordHash = "testUser123" },
            new User(){ Id = 2, Name = "New User", Username = "newUser", PasswordHash = "newUser123" }
        };
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _users.SingleOrDefault(x => x.Username == model.Username);

        // validate
        if (user == null || model.Password != user.PasswordHash)
            throw new AppException("Username or password is incorrect");

        // authentication successful
        var response = new AuthenticateResponse() { 
            Id = user.Id,
            Name= user.Name,
            Username= user.Username,
            Token = "tokentest"
        };

        return response;
    }

    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public User GetById(int id)
    {
        return getUser(id);
    }

    public User Register(RegisterRequest model)
    {
        // validate
        if (_users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // map model to new user object
        var user = new User()
        {
            Id = _users.Max(x => x.Id) + 1,
            Name = model.Name,
            Username= model.Username,
            PasswordHash = model.Password
        };

        // save user
        _users.Add(user);

        return user;
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        // validate
        if (model.Username != user.Username && _users.Any(x => x.Username == model.Username))
            throw new AppException("Username '" + model.Username + "' is already taken");

        // copy model to user and save
        user.Name = model.Name;
        user.Username = model.Username;

        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = model.Password;

    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _users.Remove(user);
    }

    private User getUser(int id)
    {
        var user = _users.Where(x => x.Id == id).FirstOrDefault();
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

}

