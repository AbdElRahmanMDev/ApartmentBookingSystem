namespace Domain.Abstraction
{
    public record Error(string Code, string Message)
    {
        public static Error None = new Error("None", "No Error");

        public static Error NullValue = new("NullValue", "Value cannot be null");
    }
}
