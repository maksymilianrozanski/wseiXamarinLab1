using FirstLab.network.models;

namespace FirstLab.entities
{
    public static class EntityConverters
    {
        public static IndexEntity ToIndexEntity(this Index index) =>
            new IndexEntity(index.name, index.value, index.level, index.description, index.advice, index.color);
    }
}