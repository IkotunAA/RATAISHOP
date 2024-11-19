namespace RATAISHOP.Enum
{
    public enum PaymentStatus
    {
        Pending,          // Payment has not been initiated or is waiting to be processed
        PendingVerification, // Bank transfer is initiated but not yet verified
        Paid,             // Payment has been successfully completed
        Failed,           // Payment has failed
        Refunded          // Payment has been refunded
    }
}
