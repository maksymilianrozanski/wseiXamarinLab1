using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FirstLab.entities
{
    public class InstallationEntity
    {
        [PrimaryKey] public int Id { get; set; }

        public string Location { get; set; }
        public string Address { get; set; }

        [OneToOne(
            // foreignKey: nameof(entities.CurrentEntity.Id),
            CascadeOperations = CascadeOperation.All)]
        public CurrentEntity CurrentEntity { get; set; }

        // [ForeignKey(typeof(CurrentEntity))] public int CurrentEntityId { get; set; }


        public InstallationEntity()
        {
        }

        public InstallationEntity(int id, string location, string address, CurrentEntity currentEntity)
        {
            Id = id;
            Location = location;
            Address = address;
            CurrentEntity = currentEntity;
        }
    }
}