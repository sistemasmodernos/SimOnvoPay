using SimOnvoPay.Models.Common;

namespace SimOnvoPay.Models.Customers;

public class CustomerListRequest : ListRequest
{
    public string? Email { get; set; }

    public override Dictionary<string, string> ToQueryParams()
    {
        var dict = base.ToQueryParams();
        if (!string.IsNullOrEmpty(Email)) dict["email"] = Email;
        return dict;
    }
}
