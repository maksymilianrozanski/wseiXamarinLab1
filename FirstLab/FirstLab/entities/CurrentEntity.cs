using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FirstLab.entities
{
    public class CurrentEntity
    {
        public string FromDateTime { get; set; }
        public string TillDateTime { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public List<ValueEntity> Values { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]

        public List<StandardEntity> Standards { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public List<IndexEntity> Indexes { get; set; }

        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public CurrentEntity()
        {
        }

        public CurrentEntity(string fromDateTime, string tillDateTime, List<ValueEntity> values,
            List<StandardEntity> standards, List<IndexEntity> indexes)
        {
            FromDateTime = fromDateTime;
            TillDateTime = tillDateTime;
            Values = values;
            Standards = standards;
            Indexes = indexes;
        }
    }
}