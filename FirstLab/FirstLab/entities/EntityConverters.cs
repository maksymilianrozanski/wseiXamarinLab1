using System.Linq;
using FirstLab.network.models;
using LaYumba.Functional;
using Newtonsoft.Json;

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

        public static CurrentEntity ToCurrentEntity2(this Current current, InstallationEntity installationEntity) =>
            current.Pipe(it =>
                new CurrentEntity(it.fromDateTime, it.tillDateTime,
                    it.values.Select(ToValueEntity).ToList(),
                    it.standards.Select(ToStandardEntity).ToList(),
                    it.indexes.Select(ToIndexEntity).ToList())).Pipe(entity =>
            {
                entity.InstallationEntity = installationEntity;
                return entity;
            });

        public static InstallationEntity ToInstallationEntity(this Installation installation, Current current) =>
            installation.Pipe(it =>
                {
                    var location = JsonConvert.SerializeObject(it.location);
                    var address = JsonConvert.SerializeObject(it.address);
                    return new InstallationEntity(it.id, location, address, current.ToCurrentEntity());
                }
            ).Pipe(it => it.CurrentEntity.InstallationEntity = it);

        public static InstallationEntity ToInstallationEntity(this Installation installation) =>
            installation.Pipe(it =>
                {
                    var location = JsonConvert.SerializeObject(it.location);
                    var address = JsonConvert.SerializeObject(it.address);
                    return new InstallationEntity(it.id, location, address, null);
                }
            );
    }
}