﻿using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.CodeFlix.Catalog.Infra.Data.Ef.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CodeFlixCatalogDbContext _context;
        private DbSet<Category> _categories => _context.Set<Category>();

        public CategoryRepository(CodeFlixCatalogDbContext context) => _context = context;
        public async Task Insert(Category aggregate, CancellationToken cancellationToken) => await _categories.AddAsync(aggregate, cancellationToken);
        public Task Delete(Category aggregate, CancellationToken cancellationToken) => throw new NotImplementedException();
        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categories.FindAsync(new object[] { id }, cancellationToken);

            NotFoundException.ThrowIfNull(category, $"Catgory '{id}' not found.");

            return category!;
        }
        public Task Update(Category aggregate, CancellationToken cancellationToken)
            => Task.FromResult(_categories.Update(aggregate));
        public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
