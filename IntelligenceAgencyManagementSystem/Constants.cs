namespace IntelligenceAgencyManagementSystem;

public record Constants
{
    public const string YearPattern = "(19[0-9]{2})|(20[0-9]{2})";
    public const string DmyDatePattern = "(.+[./,-]19[0-9]{2})|(.+[./,-]20[0-9]{2})";

    public const string DefaultAdminEmail = "admin@ia.com";
    public const string DefaultAdminPassword = "Admin-1";
}