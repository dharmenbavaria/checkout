namespace Checkout.Custom.Common.Model
{
    public class CardDetails
    {
#pragma warning disable CS8618 // Non-nullable property 'CardHolderName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string CardHolderName { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'CardHolderName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'CardNumber' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string CardNumber { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'CardNumber' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'ExpirationMonth' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string ExpirationMonth { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ExpirationMonth' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'ExpirationYear' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string ExpirationYear { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ExpirationYear' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'Cvv' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Cvv { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Cvv' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
