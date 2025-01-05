using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ModelDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    

    public decimal Size { get; set; }

    public int userId { get; set; }

    public Model GetModel(ModelDto modelDto)
    {
        Model model = new Model();
        model.Id = modelDto.Id;
        model.Name = modelDto.Name;
        model.Size = modelDto.Size;
        model.UserId = modelDto.userId;
        return model;
    }

}