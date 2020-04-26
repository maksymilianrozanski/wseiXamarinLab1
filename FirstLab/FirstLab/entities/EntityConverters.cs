using FirstLab.network.models;

namespace FirstLab.entities
{
    public static class EntityConverters
    {
        public static IndexEntity ToIndexEntity(this Index index) =>
            new IndexEntity(index.name, index.value, index.level, index.description, index.advice, index.color);

        public static StandardEntity ToStandardEntity(this Standard standard) =>
            new StandardEntity(standard.name, standard.pollutant, standard.limit, standard.percent);
    }
}