using FluentAssertions;
using Yomicchi.Core;

namespace Yomicchi.Tests
{
    public class DeinflectorTests
    {
        [Fact]
        public void Passive()
        {
            var text = "言われる";

            var deinflector = new Deinflector(text);
            var deinflections = deinflector.Deinflect();

            var expected = deinflections.First(deinflection => deinflection.Text.Equals("言う"));

            expected.Inflections.Should().BeEquivalentTo(["passive"]);
        }

        [Fact]
        public void PassiveNegative()
        {
            var text = "食べられない";

            var deinflector = new Deinflector(text);
            var deinflections = deinflector.Deinflect();

            var expected = deinflections.First(deinflection => deinflection.Text.Equals("食べる"));

            expected.Inflections.Should().BeEquivalentTo(["passive", "negative"]);
        }
    }
}
