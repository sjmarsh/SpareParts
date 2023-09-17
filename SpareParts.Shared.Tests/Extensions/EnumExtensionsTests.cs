using SpareParts.Shared.Extensions;
using System.ComponentModel;

namespace SpareParts.Shared.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void Should_ReturnDescriptionForGivenEnum()
        {
            var sut = TestEnumForExtensions.One;
            var result = sut.GetEnumDescription();

            Assert.Equal("This is One", result);
        }

        [Fact]
        public void Should_ReturnNullWhereEnumHasNoDescription()
        {
            var sut = TestEnumForExtensions.Three;
            var result = sut.GetEnumDescription();

            Assert.Null(result);
        }

        [Fact]
        public void Should_ReturnTrueWhenTypeIsEnum()
        {
            var result = typeof(TestEnumForExtensions).IsEnum();
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnTrueWhenTypeIsNullableEnum()
        {
            var result = typeof(TestEnumForExtensions?).IsEnum();
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnFalseWhenTypeIsNotEnum()
        {
            var result = typeof(string).IsEnum();
            Assert.False(result);

            result = typeof(double?).IsEnum();
            Assert.False(result);
        }

        [Fact]
        public void Should_ReturnEnumForString()
        {
            var str = "One";
            var result = str.GetEnumFromString<TestEnumForExtensions>();
            result.Should().Be(TestEnumForExtensions.One);
        }

        [Fact]
        public void Should_ReturnNullWhenNotMatchingEnum()
        {
            var str = "Five";
            var result = str.GetEnumFromString<TestEnumForExtensions>();
            result.Should().BeNull();
        }

        [Fact]
        public void Should_ReturnNullWhenNullString()
        {
            string? str = null;
            var result = str.GetEnumFromString<TestEnumForExtensions>();
            result.Should().BeNull();
        }
    }

    public enum TestEnumForExtensions
    {
        [Description("This is One")]
        One,
        [Description("This is Two")]
        Two, 
        // no description for this one
        Three
    }
}
