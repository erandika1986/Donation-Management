namespace ViharaFund.Domain.Entities.Master
{
    public class Organization
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Package { get; set; }
        public bool IsActive { get; set; }
    }
}
