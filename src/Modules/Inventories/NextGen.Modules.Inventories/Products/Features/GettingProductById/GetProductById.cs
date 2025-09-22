using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Inventories.Products.Dtos;
using NextGen.Modules.Inventories.Products.Exceptions.Application;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Shared.Extensions;
using FluentValidation;

namespace NextGen.Modules.Inventories.Products.Features.GettingProductById;

public record GetProductById(long Id) : IQuery<GetProductByIdResponse>;

internal class GetProductByIdValidator : AbstractValidator<GetProductById>
{
    public GetProductByIdValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id).GreaterThan(0);
    }
}

public class GetProductByIdHandler : IQueryHandler<GetProductById, GetProductByIdResponse>
{
    private readonly IInventoryDbContext _inventoryDbContext;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IInventoryDbContext inventoryDbContext, IMapper mapper)
    {
        _inventoryDbContext = inventoryDbContext;
        _mapper = mapper;
    }

    public async Task<GetProductByIdResponse> Handle(GetProductById query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        var product = await _inventoryDbContext.FindProductByIdAsync(query.Id);
        Guard.Against.NotFound(product, new ProductNotFoundException(query.Id));

        var productsDto = _mapper.Map<ProductDto>(product);

        return new GetProductByIdResponse(productsDto);
    }
}

public record GetProductByIdResponse(ProductDto Product);
