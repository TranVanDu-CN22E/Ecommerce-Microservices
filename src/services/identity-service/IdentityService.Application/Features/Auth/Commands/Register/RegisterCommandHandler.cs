using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Application.Common;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        public RegisterCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }
        public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken ct)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (existingUser is not null)
            {
                return Result<RegisterResponse>.Failure(
                    new[] { AuthErrors.EmailAlreadyExists }
                );
            }

            var user = await _userRepository.AddAsync(request.Email, request.Phone, request.UserName, _passwordHasher.HashPassword(request.Password), ct);
            await _unitOfWork.SaveChangesAsync(ct);
            await _emailService.SendWelcomeEmailAsync(request.Email, request.UserName, ct);

            return Result<RegisterResponse>.Success(new RegisterResponse(
                user.Id.ToString(),
                user.Email.ToString(),
                user.UserName.ToString(),
                user.CreatedAt
            ));
        }
    }
}
