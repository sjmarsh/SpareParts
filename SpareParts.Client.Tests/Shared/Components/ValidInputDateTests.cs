using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace SpareParts.Client.Tests.Shared.Components
{
    public class ValidInputDateTests
    {
        [Fact]
        public void Should_RenderValidComponent()
        {            
            var testModel = new TestModel();
            var ctx = new BunitContext();
            ctx.Services.AddSingleton<IValidator<TestModel>, TestModelValidator>();
            var cut = ctx.Render<ValidInputDateWrapper>(parameters => parameters
                .Add(p => p.Id, "testDate")
                .Add(p => p.DisplayName, "Test Date")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            component.Should().NotBeNull();

            var label = component.GetElementsByTagName("label")[0];
            label.TextContent.Should().Be("Test Date");
            var input = component.GetElementsByTagName("input")[0];
            input.Id.Should().Be("testDate");
            input.Attributes.FirstOrDefault(a => a.Name == "type" && a.Value == "date").Should().NotBeNull();
            input.ClassList.Should().Contain("valid");
            input.ClassList.Should().Contain("form-control");

            input.Input("2022-01-01");
            component = cut.Find(".form-group");
            input = component.GetElementsByTagName("input")[0];
            input.ClassList.Should().NotContain("invalid");
            component.InnerHtml.Should().NotContain("validation-message");
        }

        [Fact]
        public void Should_RenderInvalidComponent()
        {
            var testModel = new TestModel();
            var ctx = new BunitContext();
            ctx.Services.AddSingleton<IValidator<TestModel>, TestModelValidator>();
            var cut = ctx.Render<ValidInputDateWrapper>(parameters => parameters
                .Add(p => p.Id, "testDate")
                .Add(p => p.DisplayName, "Test Date")
                .Add(p => p.TestModel, testModel)
            );

            var component = cut.Find(".form-group");
            var input = component.GetElementsByTagName("input")[0];
            input.Should().NotBeNull();
            input.ClassList.Should().NotContain("invalid");

            input.Input("1990-12-31");
            input = cut.Find("#testDate");
            input.ClassList.Should().Contain("invalid");
            component.InnerHtml.Should().Contain("validation-message");
        }
    }
}
