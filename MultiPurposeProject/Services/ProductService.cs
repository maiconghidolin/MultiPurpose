namespace MultiPurposeProject.Services;

using AutoMapper;
using BCrypt.Net;
using MongoDB.Driver;
using MultiPurposeProject.Authorization;
using MultiPurposeProject.Entities;
using MultiPurposeProject.Helpers;
using MultiPurposeProject.Models.Products;


public interface IProductService
{
    List<Product> GetAll();
    Product GetById(string id);
    void Create(CreateRequest model);
    void Update(string id, UpdateRequest model);
    void Delete(string id);
}

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _products;
    private readonly IMapper _mapper;

    public ProductService(
        IMongoDBSettings settings,
        IMongoClient mongoClient,
        IMapper mapper)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _products = database.GetCollection<Product>("Products");

        _mapper = mapper;
    }

    public List<Product> GetAll()
    {
        return _products.Find(product => true).ToList();
    }

    public Product GetById(string id)
    {
        return _products.Find(product => product.Id == id).FirstOrDefault();
    }

    public void Create(CreateRequest model)
    {
        if (_products.Find(product => product.Code == model.Code).Any())
            throw new AppException("Code '" + model.Code + "' already exists");

        var product = _mapper.Map<Product>(model);
        _products.InsertOne(product);
    }

    public void Update(string id, UpdateRequest model)
    {
        var product = _mapper.Map<Product>(model);

        _products.ReplaceOne(product => product.Id == id, product);
    }

    public void Delete(string id)
    {
        _products.DeleteOne(product => product.Id == id);
    }

}

