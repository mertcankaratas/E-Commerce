using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommandRequest, RemoveBasketItemCommandResponse>
    {
        public Task<RemoveBasketItemCommandResponse> Handle(RemoveBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
