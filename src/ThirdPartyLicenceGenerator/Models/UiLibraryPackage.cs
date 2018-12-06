namespace ThirdPartyLicenceGenerator.Models
{
    public class UiLibraryPackage : IPackage
    {
        public bool WasFound { get; set; }
        public string PackageId { get; set; }
        public string Version { get; set; }
        public string SourcePath { get; set; }
        public string LicencePath { get; set; }
        public string ProjectPath { get; set; }
    }
}
