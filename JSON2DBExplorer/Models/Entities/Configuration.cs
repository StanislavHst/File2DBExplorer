using System.ComponentModel.DataAnnotations.Schema;

namespace JSON2DBExplorer.Models.Entities;

public class Configuration
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? Value { get; set; }

    public virtual ICollection<ConfigurationRelationship> Parents { get; set; } = new List<ConfigurationRelationship>();
    public virtual ICollection<ConfigurationRelationship> Childrens { get; set; } = new List<ConfigurationRelationship>();
}