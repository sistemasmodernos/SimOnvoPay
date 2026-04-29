namespace SimOnvoPay.Models.Common;

public class ListRequest
{
    public int? Limit { get; set; }
    public string? CursorNext { get; set; }
    public string? CursorBefore { get; set; }

    public virtual Dictionary<string, string> ToQueryParams()
    {
        var dict = new Dictionary<string, string>();
        if (Limit.HasValue) dict["limit"] = Limit.Value.ToString();
        if (!string.IsNullOrEmpty(CursorNext)) dict["cursorNext"] = CursorNext;
        if (!string.IsNullOrEmpty(CursorBefore)) dict["cursorBefore"] = CursorBefore;
        return dict;
    }
}
