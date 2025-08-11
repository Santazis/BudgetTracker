namespace BudgetTracker.Application.Models.User;

public record UserDto(
    string Firstname,
    string Lastname,
    string Username,
    string Email,
    DateTime CreatedAt,
    bool IsEmailVerified,
    IEnumerable<PaymentMethodDto> PaymentMethods
)
{
    public static UserDto FromEntity(Domain.Models.User.User u) =>
        new UserDto(Firstname: u.Name.Firstname, Lastname: u.Name.Lastname, Email: u.Email, Username: u.Username,
            CreatedAt: u.CreatedAt,
            IsEmailVerified: u.IsEmailVerified, PaymentMethods: u.PaymentMethods.Select(PaymentMethodDto.FromEntity));
}
