using FluentAssertions;
using Yomicchi.Core;

namespace Yomicchi.Tests
{
    public class NormalizerTests
    {
        [Fact]
        public void Initialize()
        {
            var sut = () => new Normalizer("やほ");

            sut.Should().NotThrow();
        }

        [Fact]
        public void SetOriginal()
        {
            var normalizer = new Normalizer("こんにちは");
            normalizer.OriginalText.Should().Be("こんにちは");
        }

        [Fact]
        public void IgnoreKanji()
        {
            var normalizer = new Normalizer("高速道路");
            normalizer.NormalizedText.Should().Be("高速道路");
        }

        [Fact]
        public void IgnoreRomaji()
        {
            var normalizer = new Normalizer("expressway");
            normalizer.NormalizedText.Should().Be("expressway");
        }

        [Fact]
        public void IgnoreHiragana()
        {
            var normalizer = new Normalizer("つづく");
            normalizer.NormalizedText.Should().Be("つづく");
        }

        [Fact]
        public void ConvertKatakanaToHiragana()
        {
            var normalizer = new Normalizer("コーヒー");
            normalizer.NormalizedText.Should().Be("こーひー");
        }

        [Fact]
        public void ConvertMixedToHiragana()
        {
            var normalizer = new Normalizer("食べまシタ");
            normalizer.NormalizedText.Should().Be("食べました");
        }
    }
}
