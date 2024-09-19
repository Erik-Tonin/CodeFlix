using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Agrregates")]
    public void Instantiate()
    {
        //Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var dateTimeBefore = DateTime.Now;

        //Act
        var category = new Category(validData.Name, validData.Description);

        //Assert
        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var dateTimeBefore = DateTime.Now;

        var category = new Category(validData.Name, validData.Description, isActive);

        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action =
            () => new Category(name!, "Category Descripition");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Agrregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action =
            () => new Category("Category Name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameIsLessThan3Characteres(string invalidName)
    {
        Action action =
            () => new Category(invalidName, "Category ok Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characteres()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action =
            () => new Category(invalidName, "Category ok Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characteres()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action =
            () => new Category("Category Name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Agrregates")]
    public void Activate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new Category(validData.Name, validData.Description, false);
        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Agrregates")]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new Category(validData.Name, validData.Description, true);
        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Agrregates")]
    public void Update()
    {
        var category = new Category("Category Name", "Category description");
        var newValues = new { Name = "New name", Description = "New Description" };

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Agrregates")]
    public void UpdateOnlyName()
    {
        var category = new Category("Category Name", "Category description");
        var newValues = new { Name = "New name" };

        category.Update(newValues.Name, category.Description);

        Assert.Equal(newValues.Name, category.Name);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new Category("Category Name", "Category description");

        Action action =
            () => category.Update(name!);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameIsLessThan3Characteres(string invalidName)
    {
        var category = new Category("Category Name", "Category description");

        Action action =
            () => category.Update(invalidName);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characteres()
    {
        var category = new Category("Category Name", "Category description");

        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action =
            () => category.Update(invalidName);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characteres))]
    [Trait("Domain", "Category - Agrregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characteres()
    {
        var category = new Category("Category Name", "Category description");

        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

        Action action =
            () => category.Update("Category new Name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
    }
}
