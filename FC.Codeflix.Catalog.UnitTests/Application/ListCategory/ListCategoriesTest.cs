﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategory
{
    [Collection(nameof(ListCategoriesFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesFixture _fixture;

        public ListCategoriesTest(ListCategoriesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(List))]
        [Trait("Application", "ListCategories - Use Cases")]
        public async Task List()
        {
            var categoriesExampleList = _fixture.GetExampleCategoriesList();
            var repositoryMock = _fixture.GetRepositoryMock();

            var input = new ListCategoriesInput(
                page: 2,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
            );

            var outputRepositorySearch = new SearchOutput<Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<Category>)categoriesExampleList,
                total: 70
            );

            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);

            var useCase = new ListCategories(repositoryMock.Object);
            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
            ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
            {
                var repositoryCategory = outputRepositorySearch.Items
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(repositoryCategory!.Name);
                outputItem.Description.Should().Be(repositoryCategory.Description);
                outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
                outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
            });
            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
