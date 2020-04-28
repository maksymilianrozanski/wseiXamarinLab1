using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FirstLab.entities
{
    public class InstallationEntity
    {
        [PrimaryKey] public int Id { get; set; }

        public string Location { get; set; }
        public string Address { get; set; }

        [OneToMany(CascadeOperations =
            CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]
        public List<CurrentEntity> CurrentEntities { get; set; }

        public InstallationEntity()
        {
        }

        public InstallationEntity(int id, string location, string address, List<CurrentEntity> currentEntities)
        {
            Id = id;
            Location = location;
            Address = address;
            CurrentEntities = currentEntities;
        }
    }
}