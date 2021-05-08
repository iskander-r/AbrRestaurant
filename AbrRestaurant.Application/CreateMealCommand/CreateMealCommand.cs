using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AbrRestaurant.Application.CreateMealCommand
{
    class CreateMealCommand : IRequest<CreateMealCommandResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureAsBase64 { get; set; }
        public decimal Price { get; set; }
    }


    class CreateMealCommandResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUri { get; set; }
        public decimal Price { get; set; }
    }

    class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
    {
        public CreateMealCommandValidator()
        {

        }
    }

    class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, CreateMealCommandResponse>
    {
        public Task<CreateMealCommandResponse> Handle(
            CreateMealCommand request, 
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
