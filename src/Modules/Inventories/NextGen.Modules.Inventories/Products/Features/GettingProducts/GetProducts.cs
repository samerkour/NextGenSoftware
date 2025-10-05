using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Core.Types;
using NextGen.Modules.Inventories.Products.Dtos;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Shared.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Products.Features.GettingProducts;

public record GetProducts : ListQuery<GetProductsResponse>;

public class GetProductsValidator : AbstractValidator<GetProducts>
{
    public GetProductsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetProductsHandler : IQueryHandler<GetProducts, GetProductsResponse>
{
    private readonly IInventoryDbContext _inventoryDbContext;
    private readonly IMapper _mapper;

    public GetProductsHandler(IInventoryDbContext inventoryDbContext, IMapper mapper)
    {
        _inventoryDbContext = inventoryDbContext;
        _mapper = mapper;
    }

    public async Task<GetProductsResponse> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await _inventoryDbContext.Products
            .OrderByDescending(x => x.Created)
            .ApplyIncludeList(request.Includes)
            .ApplyFilter(request.Filters)
            .AsNoTracking()
            .ApplyPagingAsync<Product, ProductDto>(_mapper.ConfigurationProvider, request.Page, request.PageSize, cancellationToken: cancellationToken);

        return new GetProductsResponse(products);
    }
}
