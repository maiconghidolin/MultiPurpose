namespace MultiPurposeProject.Models.Products;

using System.ComponentModel.DataAnnotations;

public class UpdateRequest
{

    public string Id { get; set; } = string.Empty;
    
    public int Code { get; set; } = 0;

    public string Description { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

}

