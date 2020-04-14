using LaYumba.Functional;
using NUnit.Framework;

namespace FirstLabUnitTests.utility
{
    public class TestUtility
    {
        public static T GetValueFromEither<T>(Either<Error, T> either)
        {
            var option = new Option<T>();
            either.Match(error => Assert.Fail(error.Message), arg => option = arg);
            return option.GetOrElse(() => null).Result;
        }
    }
}