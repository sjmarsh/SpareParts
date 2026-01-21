namespace SpareParts.API.Infrastructure.SimpleMediator
{
    internal interface IRequestHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
