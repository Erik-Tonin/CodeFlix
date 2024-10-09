﻿using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategory
{
    [CollectionDefinition(nameof(ListCategoriesFixture))]
    public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesFixture> { }

    public class ListCategoriesFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetRepositoryMock()
                => new();

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
            var categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }
        public bool GetRandomBoolean()
            => (new Random()).NextDouble() < 0.5;

        public Category GetExampleCategory()
            => new Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

        public List<Category> GetExampleCategoriesList(int length = 10)
        {
            var list = new List<Category>();

            for (int i = 0; i < length; i++)
                list.Add(GetExampleCategory());

            return list;
        }
    }
}