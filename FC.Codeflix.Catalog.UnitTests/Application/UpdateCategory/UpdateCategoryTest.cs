﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Entity;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(Category exampleCategory, UseCase.UpdateCategoryInput input)
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWork = _fixture.GetUnitOfWorkMock();

            repositoryMock.Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWork.Object);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()),Times.Once);
            unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
