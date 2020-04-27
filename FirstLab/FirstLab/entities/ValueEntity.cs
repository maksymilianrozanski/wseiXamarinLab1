using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FirstLab.entities
{
    public class ValueEntity
    {
        public string Name { get; set; }
        public double Value { get; set; }
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public CurrentEntity CurrentEntity { get; set; }

        [ForeignKey(typeof(CurrentEntity))] public int CurrentEntityId { get; set; }

        public ValueEntity(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public ValueEntity()
        {
        }

        protected bool Equals(ValueEntity other) => Name == other.Name && Value.Equals(other.Value) && Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValueEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                hashCode = (hashCode * 397) ^ Id;
                return hashCode;
            }
        }

        public static bool operator ==(ValueEntity left, ValueEntity right) => Equals(left, right);

        public static bool operator !=(ValueEntity left, ValueEntity right) => !Equals(left, right);
    }
}