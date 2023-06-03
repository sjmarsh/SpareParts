using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Client.Tests.Shared.Components.Filter
{
    public class GraphQLRequestBuilderTests
    {
        [Fact]
        public void Should_BuildRequest()
        {
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true),
                new FilterField("Field3", typeof(DateTime), true)
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            const string TheValue = "The Value";

            var filterLine1 = new FilterLine(theFields[0], theOperators[0].FilterOperator, TheValue);
            var theFilterLines = new List<FilterLine> { filterLine1 };

            var builder = new GraphQLRequestBuilder();

            var request = builder.Build<BuilderTestModel>(theFilterLines, theFields);

            request.Should().NotBeNull();
            request.query.Should().NotBeNullOrEmpty();            
            request.query.Should().Contain("buildertestmodels (where: { field1: {eq:\"The Value\" ");
            request.query.Should().Contain("field1\r\nfield2\r\nfield3");
        }

        [Fact]
        public void Should_BuildRequestWithMoreThanOneFilter()
        {
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), true),
                new FilterField("Field3", typeof(DateTime), true)
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            const string TheValue = "The Value";
            const string TheOtherValue = "3";

            var filterLine1 = new FilterLine(theFields[0], theOperators[0].FilterOperator, TheValue);
            var filterLine2 = new FilterLine(theFields[1], theOperators[1].FilterOperator, TheOtherValue);
            var theFilterLines = new List<FilterLine> { filterLine1, filterLine2 };

            var builder = new GraphQLRequestBuilder();

            var request = builder.Build<BuilderTestModel>(theFilterLines, theFields);

            request.Should().NotBeNull();
            request.query.Should().NotBeNullOrEmpty();
            request.query.Should().Contain("buildertestmodels (where: { field1: {eq:\"The Value\" ");
            request.query.Should().Contain("and: { field2: {contains:3");
            request.query.Should().Contain("field1\r\nfield2\r\nfield3");
        }

        [Fact]
        public void Should_BuildRequestForSelectedFields()
        {
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), false), // not selected so should not include
                new FilterField("Field3", typeof(DateTime), false) // not selected so should not include
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            const string TheValue = "The Value";

            var filterLine1 = new FilterLine(theFields[0], theOperators[0].FilterOperator, TheValue);
            var theFilterLines = new List<FilterLine> { filterLine1 };

            var builder = new GraphQLRequestBuilder();

            var request = builder.Build<BuilderTestModel>(theFilterLines, theFields);

            request.Should().NotBeNull();
            request.query.Should().NotBeNullOrEmpty();
            request.query.Should().Contain("buildertestmodels (where: { field1: {eq:\"The Value\" ");
            request.query.Should().Contain("field1\r\n");
        }

        [Fact]
        public void Should_BuildRequestWithGivenRootGraphQLField()
        {
            List<FilterField> theFields = new() {
                new FilterField("Field1", typeof(string), true),
                new FilterField("Field2", typeof(int), false), // not selected so should not include
                new FilterField("Field3", typeof(DateTime), false) // not selected so should not include
            };

            List<NamedFilterOperator> theOperators = new() {
                new NamedFilterOperator("Equals", "eq"),
                new NamedFilterOperator("Contains", "contains")
            };

            const string TheValue = "The Value";

            var filterLine1 = new FilterLine(theFields[0], theOperators[0].FilterOperator, TheValue);
            var theFilterLines = new List<FilterLine> { filterLine1 };

            const string RootGraphQLField = "differentfield";

            var builder = new GraphQLRequestBuilder();

            var request = builder.Build<BuilderTestModel>(theFilterLines, theFields, RootGraphQLField);

            request.Should().NotBeNull();
            request.query.Should().NotBeNullOrEmpty();
            request.query.Should().Contain(RootGraphQLField + " (where: { field1: {eq:\"The Value\" ");
            request.query.Should().Contain("field1\r\n");
        }
    }

    public class BuilderTestModel
    {
        public string Field1 { get; set; }
        public int Field2 { get; set; }
        public DateTime Field3 { get; set; }
    }
}
