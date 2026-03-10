using System.Net;
using RestaurantFoods.Exceptions;

namespace RestaurantFoods.Exceptions.Identity;

public class EmailAlreadyExistsException : AppException
{
    public EmailAlreadyExistsException()
        : base(
            message: "Email already registered",
            statusCode: (int)HttpStatusCode.BadRequest,
            errorCode: "EMAIL_ALREADY_EXISTS")
    {
    }
}

public class InvalidCredentialsException : AppException
{
    public InvalidCredentialsException()
        : base(
            "Invalid email or password",
            (int)HttpStatusCode.Unauthorized,
            "INVALID_CREDENTIALS")
    {
    }
}
