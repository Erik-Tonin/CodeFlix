using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
using FluentAssertions;

[Collection(nameof(CategoryTestFixture))]

public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Agrregates")]
    public void Instantiate()
    {
        //Arrange
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;

        //Act
        var category = new Category(validCategory.Name, validCategory.Description);

        //Assert
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;

        var category = new Category(validCategory.Name, validCategory.Description, isActive);

        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new Category(name!, validCategory.Description);

        action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Agrregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new Category(validCategory.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameIsLessThan3Characteres(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new Category(invalidName, validCategory.Description);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characteres()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action =
            () => new Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characteres()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action =
            () => new Category(validCategory.Name, invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Agrregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Agrregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Agrregates")]
    public void Update()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);
        var newValues = new { Name = "New name", Description = "New Description" };

        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);        
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Agrregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);
        var newValues = new { Name = "New name" };

        category.Update(newValues.Name, category.Description);

        category.Name.Should().Be(newValues.Name);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        Action action =
            () => category.Update(name!);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameIsLessThan3Characteres(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();


        var category = new Category(validCategory.Name, validCategory.Description);

        Action action =
            () => category.Update(invalidName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characteres()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();


        var category = new Category(validCategory.Name, validCategory.Description);

        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action =
            () => category.Update(invalidName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characteres()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new Category(validCategory.Name, validCategory.Description);

        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action =
            () => category.Update("Category new Name", invalidDescription);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long");
    }
}
