using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JSON2DBExplorer.Models.Entities;

public class ConfigurationRelationship
{
    public ulong ParentID { get; set; }
    public ulong ChildID { get; set; }

    public virtual Configuration Parent { get; set; }
    public virtual Configuration Child { get; set; }
}
