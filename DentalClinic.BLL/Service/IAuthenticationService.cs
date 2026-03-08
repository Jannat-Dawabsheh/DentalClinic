using DentalClinic.DAL.DTO.Request.Auth;
using DentalClinic.DAL.DTO.Response.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<LoginResponse> RefreshTokenAsync(TokenApiModel request);
        Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
    }
}
