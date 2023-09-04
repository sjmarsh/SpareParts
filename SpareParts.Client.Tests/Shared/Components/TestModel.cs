using FluentValidation;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class TestModel
    {
        public DateTime DateVal { get; set; }
        public double NumberVal { get; set; }
        public string? TextVal { get; set; }
    }

    public class TestModelWithList : TestModel 
    {
        public List<TestDifferentModel>? SomeList { get; set; }
    }

    public class TestDifferentModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(t => t.DateVal).NotEmpty().GreaterThan(new DateTime(2000, 01, 01));
            RuleFor(t => t.NumberVal).NotEmpty().GreaterThan(0);
            RuleFor(t => t.TextVal).NotEmpty().MaximumLength(10);
        }
    }
}
