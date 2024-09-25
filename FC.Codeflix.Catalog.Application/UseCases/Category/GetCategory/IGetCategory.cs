using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory
{
    interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
    {
    }
}
