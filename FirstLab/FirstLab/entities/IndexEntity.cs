using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FirstLab.entities
{
    public class IndexEntity
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string Advice { get; set; }
        public string Color { get; set; }
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead | CascadeOperation.CascadeInsert)]
        public CurrentEntity CurrentEntity { get; set; }

        [ForeignKey(typeof(CurrentEntity))] public int CurrentEntityId { get; set; }

        public IndexEntity()
        {
        }

        public IndexEntity(string name, double value, string level, string description, string advice, string color)
        {
            this.Name = name;
            this.Value = value;
            this.Level = level;
            this.Description = description;
            this.Advice = advice;
            this.Color = color;
        }

        protected bool Equals(IndexEntity other) => Name == other.Name && Value.Equals(other.Value) &&
                                                    Level == other.Level && Description == other.Description &&
                                                    Advice == other.Advice && Color == other.Color && Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IndexEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                hashCode = (hashCode * 397) ^ (Level != null ? Level.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Advice != null ? Advice.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Color != null ? Color.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Id;
                return hashCode;
            }
        }

        public static bool operator ==(IndexEntity left, IndexEntity right) => Equals(left, right);

        public static bool operator !=(IndexEntity left, IndexEntity right) => !Equals(left, right);
    }
}