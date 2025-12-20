namespace ConnectChain.Models
{
    public class BaseModel
    {
        public int ID { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    
}
