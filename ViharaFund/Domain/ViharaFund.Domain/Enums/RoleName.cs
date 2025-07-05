using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum RoleName
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Chief Monk")]
        ChiefMonk = 2,
        [Description("Monk")]
        Monk = 3,
        [Description("Temple Management Committee")]
        TempleManagementCommittee = 4,
        [Description("Temple Financial Management Committee")]
        TempleFinancialManagementCommittee = 5,
        [Description("Temporary Committee")]
        TemporaryCommittee = 6
    }
}
