using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Paddings;

namespace NetGuardUI.Data
{
    public class UserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ReferByUserCode { get; set; }
        public string? FireBaseIdToken { get; set; }
        public string? Address { get; set; }

        
    }

    public class UserLoginDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public long? PhoneNumber { get; set; }
        public int? OTP { get; set; } 
        public bool? IsManual { get; set; }
        public string? ReferByUserCode { get; set; }
    }

    public class LoginDTO
    {
        //public string? Name { get; set; }
        //public string? Password { get; set; }
        //public string? Email { get; set; }
        //public string? ImageUrl { get; set; }
        public Guid? Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Token { get; set; }
        public string? ReferralCode { get; set; }
        public string? ExpirationTime { get; set; }

        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? UserRole { get; set; }
        public Guid? Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? SignupDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } = DateTime.UtcNow;
        public string? BusinessName { get; set; }
        public string? BusinessGstnumber { get; set; }
        public bool? IsInBusiness { get; set; }
        public string? CompanyImage { get; set; }
        public long? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? OfferDescription { get; set; }
        public long? BalanceMoney { get; set; }
        public long? BalancePoint { get; set; }
        public int? OTP { get; set; }
        public bool? RequestSubmitted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public bool? IsAssign { get; set; }
    }


    public class UserSignInDto
    {
        public string? Email { get; set; }
        public long? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public bool? IsUserUpdatePassword { get; set; }
        public string? OTP { get; set; }
        public string? Name { get; set; }
        public string? ReferralCode { get; set; }
    }


    public class SignupDTO
    {
        //public string? Name { get; set; }
        //public string? Password { get; set; }
        //public string? ConfirmPassword { get; set; }
        //public string? Email { get; set; }
        //public int OTP { get; set; }
        //public string? ImageUrl { get; set; }
        //public Guid? Role { get; set; }
        public string? RandumNumbers { get; set; }
        public string? ValidatingNumbers { get; set; }
        public string? ImageUrl { get; set; }

        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? UserRole { get; set; }
        public Guid? Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? SignupDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } = DateTime.UtcNow;
        public string? BusinessName { get; set; }
        public string? BusinessGstnumber { get; set; }
        public bool? IsInBusiness { get; set; }
        public string? CompanyImage { get; set; }
        public long? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? OfferDescription { get; set; }
        public long? BalanceMoney { get; set; }
        public long? BalancePoint { get; set; }
        public string? OTP { get; set; }
        public string? ReferralCode { get; set; }
        public string? ProviderReferralCode { get; set; }
        public string? ReferByUserCode { get; set; }
    }
    public class SignupUserDto
    {
        public string? ImageUrl { get; set; }

        public string? ConfirmPassword { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? UserRole { get; set; }
        public Guid? Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? SignupDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } = DateTime.UtcNow;
        public string? BusinessName { get; set; }
        public string? BusinessGstnumber { get; set; }
        public bool? IsInBusiness { get; set; }
        public string? CompanyImage { get; set; }
        public long? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? OfferDescription { get; set; }
        public long? BalanceMoney { get; set; }
        public long? BalancePoint { get; set; }
        public int? OTP { get; set; }
    }
}
