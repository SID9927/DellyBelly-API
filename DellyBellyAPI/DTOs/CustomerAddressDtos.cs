namespace DellyBellyAPI.DTOs
{
    public record CustomerAddressResponseDto(
        int Id,
        string Type,
        string AddressText,
        bool IsDefault
    );

    public record CreateCustomerAddressDto(
        string Type,
        string AddressText,
        bool IsDefault
    );
}
