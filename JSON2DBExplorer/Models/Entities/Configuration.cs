using System.ComponentModel.DataAnnotations.Schema;

namespace JSON2DBExplorer.Models.Entities;

public class Configuration
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? Value { get; set; }
    public ulong? ParentId { get; set; }

    public Configuration Parent { get; set; }
    public List<Configuration> Children { get; set; }
}