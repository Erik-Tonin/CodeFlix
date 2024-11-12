using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.IntegrationTests.Base;
using FC.CodeFlix.Catalog.Infra.Data.Ef;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]

    public class CategoryRepositoryTesteFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture>
    {

    }

    public class CategoryRepositoryTestFixture : BaseFixture
    {
        public bool getRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public string GetValidCategoryName()
        {
            var categoryName = "";
            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];
            if (categoryName.Length > 255)
                categoryName = categoryName[..255];
            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription =
                Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10_000)
                categoryDescription =
                    categoryDescription[..10_000];
            return categoryDescription;
        }

        public Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRandomBoolean()
            );

        public List<Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetExampleCategory()).ToList();

        public CodeFlixCatalogDbContext CreateDbContext()
            => new(
                new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
            );
    }
}
