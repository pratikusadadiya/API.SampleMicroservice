namespace API.SampleMicroservice.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }

    public class BadRequestException(string message) : Exception(message)
    {
    }

    public class EntityNullException(string message) : Exception(message)
    {
    }

    public class UnauthorizedException(string message) : Exception(message)
    {
    }

    public class DuplicateRecordException(string message) : Exception(message)
    {
    }

    public class NotAbleToPerformActionException(string message) : Exception(message)
    {
    }

    public class DaysNotValidException(string message) : Exception(message)
    {
    }

    public class NotEditableException(string message) : Exception(message)
    {
    }

    public class ForbiddenException(string message) : Exception(message)
    {
    }
}
