using DentalClinic.DAL.DTO.Request.Auth;
using DentalClinic.DAL.DTO.Response.Auth;
using DentalClinic.DAL.Models;
using DentalClinic.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPatientRepository _patientRepository;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IPatientRepository patientRepository, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _patientRepository = patientRepository;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {


                var user = registerRequest.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (!result.Succeeded)
                {
                    return new RegisterResponse()
                    {
                        Success = false,
                        Message = "User creation failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(user, "Patient");
                Patient patient = new Patient()
                {
                    UserId = user.Id,
                };
                await _patientRepository.CreatePatient(patient);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"http://localhost:5238/api/auth/Accounts/ConfirmEmail?token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "welcome", $"<h1>welcome..{user.UserName}</h1>" + $"<a href='{emailUrl}'>confirm email</a>");
                return new RegisterResponse()
                {
                    Success = true,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "Exception error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }



        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user is null)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid email",

                    };
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "account is locked, try again later",

                    };

                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
                if (result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "account locked duo to multiple failed attempts",

                    };

                }
                else if (result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "please confirm your email",

                    };
                }

                if (!result.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid password",

                    };
                }
                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);
                return new LoginResponse()
                {
                    Success = true,
                    Message = "login successfully",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,

                };

            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Exception error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }


        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;

            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;
            return true;
        }


        public async Task<LoginResponse> RefreshTokenAsync(TokenApiModel request)
        {
            string accessToken = request.AccessToken;
            string refreshToken = request.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiryToken(accessToken);
            var userName = principal.Identity.Name;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "invalid client request"
                };
            }
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return new LoginResponse()
            {
                Success = true,
                Message = "Token refreshed",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }


        public async Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = "Email Not Found"

                };

            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();
            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddYears(15);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(request.Email, "reset password", $"<p>Use this code to reset your Password:   {code}</p>");

            return new ForgotPasswordResponse
            {
                Success = true,
                Message = "code sent to your email"
            };
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Email Not Found"

                };

            }

            else if (user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "invalid code"

                };

            }
            else if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "code expired"

                };

            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "password reset failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };

            }

            await _emailSender.SendEmailAsync(request.Email, "change password", $"<p>your password is changed</p>");

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "password reset successfully"
            };
        }

    }
}
