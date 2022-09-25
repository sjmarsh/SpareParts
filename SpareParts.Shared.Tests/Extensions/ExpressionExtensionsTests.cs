using SpareParts.Shared.Extensions;
using System.Linq.Expressions;

namespace SpareParts.Shared.Tests.Extensions
{
    public class ExpressionExtensionsTests
    {
        [Fact]
        public void ShouldReturnCompiledValue()
        {
            const string testName = "Test";
            var tm = new TestModel { Id = 1, Name = testName };
            Expression<Func<string>> exp = () => tm.Name;

            var result = exp.GetValueFromExpression();
            
            Assert.Equal(testName, result);
        }

        [Fact]
        public void ShouldReturnCompiledValueWhenNull()
        {            
            var tm = new TestModel();
            Expression<Func<string>> exp = () => tm.Name!;

            var result = exp.GetValueFromExpression();

            Assert.Null(result);
        }

        [Fact]
        public void ShoudlReturnMemberName()
        {
            var tm = new TestModel();
            Expression<Func<DateTime?>> exp = () => tm.DateOfBirth;

            var result = exp.GetMemberNameFromExpression();

            Assert.Equal("DateOfBirth", result);
        }

        [Fact]
        public void ShouldSetValueToExpression()
        {
            const string value = "Test";
            var tm = new TestModel();
            Expression<Func<string>> exp = () => tm.Name!;

            exp.SetValueToExpression(tm, value);

            Assert.Equal(value, tm.Name);
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public DateTime? DateOfBirth { get; set; }
        }
    }
}
