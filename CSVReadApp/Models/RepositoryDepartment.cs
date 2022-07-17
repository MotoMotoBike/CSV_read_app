namespace CSVReadApp.Models
{
    public partial class RepositoryDepartment
    {
        public int Id { get; set; }
        
        public int DepartmentId { get; set; }
        
        public string RepositoryId { get; set; }

        public virtual Department Department { get; set; }
        
        public virtual Repository Repository { get; set; }

        public RepositoryDepartment()
        {

        }

        public RepositoryDepartment(int depatrmentId, string repositoryId)
        {
            DepartmentId = depatrmentId;
            RepositoryId = repositoryId;
        }
    }
}
