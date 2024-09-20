using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();
            Action action = () =>
                DomainValidation.NotNull(value, "Value");
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            Action action = () =>
                DomainValidation.NotNull(value, "FieldName");
            action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData("   ")]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () =>
                DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} shoud not be null or empty");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            var target = Faker.Commerce.ProductName();

            Action action = () =>
                DomainValidation.NotNullOrEmpty(target, "fieldName");

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanTheMin), parameters: 10)]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {

            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should be at leats {minLength} characters long");
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanTheMin), parameters: 10)]
        public void MinLengthOk(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            Action action = () =>
                DomainValidation.MaxLength(target, maxLength, "fieldName");

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"fieldName should be less or equal {maxLength} characters long");
        }

        [Theory(DisplayName = nameof(MaxLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
        public void MaxLengthOk(string target, int maxLength)
        {
            Action action = () =>
                DomainValidation.MaxLength(target, maxLength, "fieldName");

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesSmallerThanTheMin(int numberOfTests)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(1, 20);
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanTheMin(int numberOfTests)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random()).Next(1, 5);
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - (new Random()).Next(1, 20);
                yield return new object[] { example, maxLength };
            }
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests)
        {
            var faker = new Faker();
            for (int i = 0; i < numberOfTests; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length + (new Random()).Next(1, 20);
                yield return new object[] { example, maxLength };
            }
        }
    }
}
