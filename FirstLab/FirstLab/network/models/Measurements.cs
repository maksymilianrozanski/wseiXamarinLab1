using System;
using System.Collections.Generic;

namespace FirstLab.network.models
{
    public readonly partial struct Measurements : IEquatable<Measurements>
    {
        public readonly Current current;

        public Measurements(Current current)
        {
            this.current = current;
        }
    }

    public readonly partial struct Current
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
    }

    public readonly partial struct Value
    {
        public readonly string name;
        public readonly double value;

        public Value(string name, double value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public readonly partial struct Index : IEquatable<Index>
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
    }

    public readonly partial struct Standard
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
    }

    public readonly partial struct Measurements
    {
        public bool Equals(Measurements other) => current.Equals(other.current);
        public override bool Equals(object obj) => obj is Measurements other && Equals(other);
        public override int GetHashCode() => current.GetHashCode();
        public static bool operator ==(Measurements left, Measurements right) => left.Equals(right);
        public static bool operator !=(Measurements left, Measurements right) => !left.Equals(right);
    }
}