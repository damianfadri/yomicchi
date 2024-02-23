using FluentAssertions;
using Yomicchi.Core;

namespace Yomicchi.Tests
{
    public class TrieTests
    {
        [Fact]
        public void Initialize()
        {
            var sut = () => new Trie<int>();

            sut.Should().NotThrow();
        }

        [Fact]
        public void InsertAndGet()
        {
            var trie = new Trie<int>();

            trie.Insert("abc", 1);

            var value = trie.Get("abc");
            value.Should().Be(1);
        }

        [Fact]
        public void GetMissingStruct()
        {
            var trie = new Trie<int>();

            var value = trie.Get("b");
            value.Should().Be(0);
        }

        [Fact]
        public void GetMissingObject()
        {
            var trie = new Trie<object>();

            var value = trie.Get("bc");
            value.Should().BeNull();
        }

        [Fact]
        public void GetWithNoKeys()
        {
            var trie = new Trie<string>();

            var value = trie.Get("");
            value.Should().BeNull();
        }

        [Fact]
        public void SplitBranch()
        {
            var trie = new Trie<double>();

            trie.Insert("abc", 3.0);
            trie.Insert("abd", 2.0);

            var value = trie.Get("abc");
            value.Should().Be(3.0);
        }

        [Fact]
        public void InsertOnExisting()
        {
            var trie = new Trie<double>();

            trie.Insert("abc", 3.0);
            trie.Insert("abc", 2.0);

            var value = trie.Get("abc");
            value.Should().Be(3.0);
        }

        [Fact]
        public void InsertMultiple()
        {
            var trie = new Trie<double>();

            trie.Insert("abc", 3.0);
            trie.Insert("abc", 2.0);

            var values = trie.GetAll("abc");
            values.Should().BeEquivalentTo([3.0, 2.0]);
        }

        [Fact]
        public void GetMiddle()
        {
            var trie = new Trie<double>();

            trie.Insert("abc", 3.0);

            var value = trie.Get("ab");
            value.Should().Be(0.0);
        }
    }
}