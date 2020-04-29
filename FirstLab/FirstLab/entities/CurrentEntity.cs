using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FirstLab.entities
{
    public class CurrentEntity
    {
        public string FromDateTime { get; set; }
        public string TillDateTime { get; set; }

        [OneToMany(CascadeOperations =
            CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]
        public List<ValueEntity> Values { get; set; }

        [OneToMany(CascadeOperations =
            CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]

        public List<StandardEntity> Standards { get; set; }

        [OneToMany(CascadeOperations =
            CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert | CascadeOperation.CascadeDelete)]
        public List<IndexEntity> IndexEntities { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public InstallationEntity InstallationEntity { get; set; }

        [ForeignKey(typeof(InstallationEntity))]
        public int InstallationId { get; set; }

        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public CurrentEntity()
        {
        }

        public CurrentEntity(string fromDateTime, string tillDateTime, List<ValueEntity> values,
            List<StandardEntity> standards, List<IndexEntity> indexEntities)
        {
            FromDateTime = fromDateTime;
            TillDateTime = tillDateTime;
            Values = values;
            Standards = standards;
            IndexEntities = indexEntities;
        }
    }
}