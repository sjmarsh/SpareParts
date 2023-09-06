using SpareParts.Shared.Extensions;

namespace SpareParts.Shared.Tests.Extensions
{
    public class PropertyInfoExtensionsTests
    {
        [Fact]
        public void Should_GetStringValue()
        {
            var sut = new TestClassForPropInfo { ID = 1, Name = "Fred", Amount = 22.2, DateAdded = new DateTime(2020, 01, 01)};
            var props = typeof(TestClassForPropInfo).GetProperties();
            props[0].GetStringValueOrDefault(sut).Should().Be("1");
            props[1].GetStringValueOrDefault(sut).Should().Be("Fred");
            props[2].GetStringValueOrDefault(sut).Should().Be("22.2");
            props[3].GetStringValueOrDefault(sut).Should().Be("1/01/2020 12:00:00 AM");
        }

        [Fact]
        public void Should_GetDefaultValue()
        {
            var sut = new TestClassForPropInfo { ID = 1, Name = "Fred", Amount = null, DateAdded = null };
            var props = typeof(TestClassForPropInfo).GetProperties();
            props[0].GetStringValueOrDefault(sut).Should().Be("1");
            props[1].GetStringValueOrDefault(sut).Should().Be("Fred");
            props[2].GetStringValueOrDefault(sut, defaultValue: "No Data").Should().Be("No Data");
            props[3].GetStringValueOrDefault(sut, defaultValue: "No Date").Should().Be("No Date");
        }

        [Fact]
        public void Should_GetDefaultValueForNullSubject()
        {
            TestClassForPropInfo? sut = null;
            var props = typeof(TestClassForPropInfo).GetProperties();
            props[0].GetStringValueOrDefault(sut).Should().Be("");
            props[1].GetStringValueOrDefault(sut).Should().Be("");
            props[2].GetStringValueOrDefault(sut, defaultValue: "No Data").Should().Be("No Data");
            props[3].GetStringValueOrDefault(sut, defaultValue: "No Date").Should().Be("No Date");
        }

        [Fact]
        public void Should_GetFormattedStringValue()
        {
            var sut = new TestClassForPropInfo { ID = 1, Name = "Fred", Amount = 22.2, DateAdded = new DateTime(2020, 11, 01) };
            var props = typeof(TestClassForPropInfo).GetProperties();
            props[0].GetStringValueOrDefault(sut).Should().Be("1");
            props[1].GetStringValueOrDefault(sut).Should().Be("Fred");
            props[2].GetStringValueOrDefault(sut, formatTemplate: "#.0000").Should().Be("22.2000");
            props[3].GetStringValueOrDefault(sut, formatTemplate: "dd-MM-yyyy").Should().Be("01-11-2020");
        }

        [Fact]
        public void Should_GetStringValueWithDefaultFormat()
        {
            var sut = new TestClassForPropInfo { ID = 1, Name = "Fred", Amount = 22.2, DateAdded = new DateTime(2020, 11, 01) };
            var props = typeof(TestClassForPropInfo).GetProperties();
            props[0].GetStringValueWithDefaultFormat(sut).Should().Be("1");
            props[1].GetStringValueWithDefaultFormat(sut).Should().Be("Fred");
            props[2].GetStringValueWithDefaultFormat(sut).Should().Be("22.20");
            props[3].GetStringValueWithDefaultFormat(sut).Should().Be("01/11/2020");
        }

        [Fact]
        public void Should_GetStringValueWithDefaultFormatForGivenValue()
        {
            double num = 33.3;
            DateTime dt = new DateTime(2010, 12, 01);

            num.GetStringValueWithDefaultFormat().Should().Be("33.30");
            dt.GetStringValueWithDefaultFormat().Should().Be("01/12/2010");
        }
    }

    public class TestClassForPropInfo
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public double? Amount { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
