namespace Producer
{
    internal class InvoicePostedV1
    {
        public string? InvoiceNumber { get; set; }
        public int? PurchaseOrder { get; set; }
        public string Submitter { get; set; }
    }

    internal class InvoicePostedV2
    {
        public string? InvoiceNumber { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public string submitter { get; set; }
    }

    internal class PurchaseOrder
    {
        public int Number { get; set; }
        public string Type { get; set; }
    }
}
