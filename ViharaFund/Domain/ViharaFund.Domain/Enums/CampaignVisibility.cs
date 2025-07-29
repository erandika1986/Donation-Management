using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum CampaignVisibility
    {
        [Description("Public - Anyone can view or donate")]
        Public = 1, // Public access, visible to everyone
        [Description("Private - Only invited donors")]
        Private = 2, // Private access, visible only to specific users or groups
        [Description("Organization Only - Internal campaign")]
        OrganizationOnly = 3 // Restricted access, may require special permissions or conditions to view
    }
}
