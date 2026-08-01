namespace UptimeMonitor.Domain
{
    public enum MonitorEventType
    {
        MonitorDown,
        MonitorRecovered,
        SlowResponse,
        TlsCertificateExpiring
        /**
        * Future alert types can be added here:
            * - MonitorPaused
            * - MonitorResumed
            * - UnexpectedStatusCode
            * - RequestTimeout
            * - DnsFailure
            * - TlsCertificateExpired
            * - ConnectionFailure
            * - ContentMismatch
        */
    }

    public enum CheckFailureReason
    {
        None,
        Timeout,
        DnsFailure,
        ConnectionFailure,
        InvalidStatusCode,
        ContentMismatch,
        DnsResolutionFailure,
        TlsCertificateError
        /**
        * Future failure reasons can be added here:
            * - RequestTimeout
            * - ConnectionRefused
            * - NetworkUnreachable
            * - TlsHandshakeFailure
            * - HttpRedirectLoop
        */
    }
}
