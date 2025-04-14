using ConnectChain.Models;

public class ActivityCategory : BaseModel
{  
    public string Name { get; set; } = string.Empty;
    public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
