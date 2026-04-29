using SimOnvoPay.Models.Common;

namespace SimOnvoPay.Models.PaymentIntents;

public class PaymentIntentListRequest : ListRequest
{
    public string? Customer { get; set; }
    public string? Status { get; set; }
    public long? CreatedFrom { get; set; }
    public long? CreatedTo { get; set; }

    public override Dictionary<string, string> ToQueryParams()
    {
        var dict = base.ToQueryParams();
        if (!string.IsNullOrEmpty(Customer)) dict["customer"] = Customer;
        if (!string.IsNullOrEmpty(Status)) dict["status"] = Status;
        if (CreatedFrom.HasValue) dict["createdFrom"] = CreatedFrom.Value.ToString();
        if (CreatedTo.HasValue) dict["createdTo"] = CreatedTo.Value.ToString();
        return dict;
    }
}
