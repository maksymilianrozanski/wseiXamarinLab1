using System.Linq;
using FirstLab.network.models;
using LaYumba.Functional;
using Newtonsoft.Json;
using Xamarin.Essentials;

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

        public static Measurements ToMeasurement(this CurrentEntity currentEntity)
            => currentEntity.Pipe(it => new Measurements(
                new Current(it.FromDateTime, it.TillDateTime, it.Values.Map(v => v.ToValue()).ToList(),
                    it.IndexEntities.Map(i => i.ToIndex()).ToList(),
                    it.Standards.Map(s => s.ToStandard()).ToList()
                )));

        public static Value ToValue(this ValueEntity valueEntity)
            => valueEntity.Pipe(it => new Value(it.Name, it.Value));

        public static Index ToIndex(this IndexEntity indexEntity)
            => indexEntity.Pipe(it => new Index(it.Name, it.Value, it.Level, it.Description, it.Advice, it.Color));

        public static Standard ToStandard(this StandardEntity standardEntity)
            => standardEntity.Pipe(it => new Standard(it.Name, it.Pollutant, it.Limit, it.Percent));

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

        public static Installation ToInstallation(this InstallationEntity installationEntity) =>
            installationEntity.Pipe(it =>
                new Installation(it.Id, JsonConvert.DeserializeObject<Location>(it.Location),
                    JsonConvert.DeserializeObject<Address>(it.Address)));
    }
}