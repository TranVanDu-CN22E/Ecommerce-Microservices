using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Abstractions.Services;
using IdentityService.Application.Common;
using IdentityService.Domain.Aggregates.UserAggregate;
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
            var email = UserEmail.Create(request.Email);
            var phone = UserPhone.Create(request.Phone);
            var userName = UserName.Create(request.UserName);
            var passwordHash = PasswordHash.Create(_passwordHasher.HashPassword(request.Password));

            var user = User.Create(email, phone, userName, passwordHash);

            await _userRepository.AddAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            _ = _emailService.SendWelcomeEmailAsync(user.Email.Value, user.UserName.Value, ct);

            return Result<RegisterResponse>.Success(new RegisterResponse(
                user.Id,
                user.Email.Value,
                user.UserName.Value,
                user.CreatedAt
            ));
        }
    }
}
