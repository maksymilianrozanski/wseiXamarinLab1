using SQLite;

namespace FirstLab.entities
{
    public class InstallationEntity
    {
        [PrimaryKey] public int Id { get; set; }

        public string Location { get; set; }
        public string Address { get; set; }

        public InstallationEntity()
        {
        }

        public InstallationEntity(int id, string location, string address)
        {
            Id = id;
            Location = location;
            Address = address;
        }
    }
}