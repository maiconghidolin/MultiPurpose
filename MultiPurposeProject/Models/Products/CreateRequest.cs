namespace MultiPurposeProject.Models.Products;

using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

public class CreateRequest
{

    [Required]
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public int Code { get; set; } = 0;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Brand { get; set; } = string.Empty;

}

