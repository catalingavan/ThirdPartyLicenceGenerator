namespace ThirdPartyLicenceGenerator.Models
{
    public interface IPackage
    {
        bool WasFound { get; }

        string PackageId { get; }
        string Version { get; }
        
        string ProjectPath { get; }
        string LicencePath { get; }
        string SourcePath { get; set; }
    }
}
