using System.Linq;
using FirstLab.network.models;
using LaYumba.Functional;

namespace FirstLab.entities
{
    public static class EntityConverters
    {
        public static IndexEntity ToIndexEntity(this Index index) =>
            new IndexEntity(index.name, index.value, index.level, index.description, index.advice, index.color);

        public static StandardEntity ToStandardEntity(this Standard standard) =>
            new StandardEntity(standard.name, standard.pollutant, standard.limit, standard.percent);

        public static ValueEntity ToValueEntity(this Value value) =>
            new ValueEntity(value.name, value.value);

        public static CurrentEntity ToCurrentEntity(this Current current) =>
            current.Pipe(it => new CurrentEntity(it.fromDateTime, it.tillDateTime,
                it.values.Select(ToValueEntity).ToList(),
                it.standards.Select(ToStandardEntity).ToList(),
                it.indexes.Select(ToIndexEntity).ToList()));
    }
}