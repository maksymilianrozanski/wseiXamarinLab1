using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstLab.network.models
{
    public readonly struct Measurements : IEquatable<Measurements>
    {
        public readonly Current current;

        public Measurements(Current current)
        {
            this.current = current;
        }

        public bool Equals(Measurements other)
        {
            return current.Equals(other.current);
        }

        public override bool Equals(object obj)
        {
            return obj is Measurements other && Equals(other);
        }

        public override int GetHashCode()
        {
            return current.GetHashCode();
        }

        public static bool operator ==(Measurements left, Measurements right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Measurements left, Measurements right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct Current
    {
        public readonly string fromDateTime;
        public readonly string tillDateTime;
        public readonly List<Value> values;
        public readonly List<Index> indexes;
        public readonly List<Standard> standards;

        public Current(string fromDateTime, string tillDateTime, List<Value> values, List<Index> indexes,
            List<Standard> standards)
        {
            this.fromDateTime = fromDateTime;
            this.tillDateTime = tillDateTime;
            this.values = values;
            this.indexes = indexes;
            this.standards = standards;
        }

        public bool Equals(Current other)
        {
            return fromDateTime == other.fromDateTime && tillDateTime == other.tillDateTime &&
                   values.SequenceEqual(other.values) && indexes.SequenceEqual(other.indexes) &&
                   standards.SequenceEqual(other.standards);
        }

        public override bool Equals(object obj)
        {
            return obj is Current other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (fromDateTime != null ? fromDateTime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (tillDateTime != null ? tillDateTime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (values != null ? values.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (indexes != null ? indexes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (standards != null ? standards.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Current left, Current right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Current left, Current right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct Value
    {
        public readonly string name;
        public readonly double value;

        public Value(string name, double value)
        {
            this.name = name;
            this.value = value;
        }

        public bool Equals(Value other)
        {
            return name == other.name && value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            return obj is Value other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ value.GetHashCode();
            }
        }

        public static bool operator ==(Value left, Value right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Value left, Value right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct Index : IEquatable<Index>
    {
        public readonly string name;
        public readonly double value;
        public readonly string level;
        public readonly string description;
        public readonly string advice;
        public readonly string color;

        public Index(string name, double value, string level, string description, string advice, string color)
        {
            this.name = name;
            this.value = value;
            this.level = level;
            this.description = description;
            this.advice = advice;
            this.color = color;
        }

        public bool Equals(Index other)
        {
            return name == other.name && value.Equals(other.value) && level == other.level &&
                   description == other.description && advice == other.advice && color == other.color;
        }

        public override bool Equals(object obj)
        {
            return obj is Index other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ value.GetHashCode();
                hashCode = (hashCode * 397) ^ (level != null ? level.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (description != null ? description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (advice != null ? advice.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (color != null ? color.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Index left, Index right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Index left, Index right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct Standard 
    {
        public readonly string name;
        public readonly string pollutant;
        public readonly double limit;
        public readonly double percent;

        public Standard(string name, string pollutant, double limit, double percent)
        {
            this.name = name;
            this.pollutant = pollutant;
            this.limit = limit;
            this.percent = percent;
        }

        public bool Equals(Standard other)
        {
            return name == other.name && pollutant == other.pollutant && limit.Equals(other.limit) && percent.Equals(other.percent);
        }

        public override bool Equals(object obj)
        {
            return obj is Standard other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (pollutant != null ? pollutant.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ limit.GetHashCode();
                hashCode = (hashCode * 397) ^ percent.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Standard left, Standard right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Standard left, Standard right)
        {
            return !left.Equals(right);
        }
    }
}