using System.ComponentModel;

namespace ViharaFund.Domain.Enums
{
    public enum DonationExpenseType
    {
        [Description("Charity Donation")]
        CharityDonation = 1,
        [Description("Religious Donation")]
        ReligiousDonation = 2,
        [Description("Medical Aid")]
        MedicalAid = 3,
        [Description("Educational Support")]
        EducationalSupport = 4,
        [Description("Disaster Relief")]
        DisasterRelief = 5,
        [Description("Community Development")]
        CommunityDevelopment = 6,
        [Description("Event Sponsorship")]
        EventSponsorship = 7,
        [Description("Scholarship Fund")]
        ScholarshipFund = 8,
        [Description("Orphanage Support")]
        OrphanageSupport = 9,
        [Description("Elderly Care Support")]
        ElderlyCareSupport = 10,
        [Description("Animal Welfare Donation")]
        AnimalWelfareDonation = 11,
        [Description("Fundraising Expense")]
        FundraisingExpense = 12,
        [Description("Volunteer Reimbursement")]
        VolunteerReimbursement = 13,
        [Description("Transportation for Aid")]
        TransportationForAid = 14,
        [Description("Promotional Materials")]
        PromotionalMaterials = 15,
        [Description("Administrative Operational Expenses")]
        AdminOperationalExpenses = 16,
        [Description("International Donations")]
        InternationalDonations = 17,
        [Description("Local Community Donation")]
        LocalCommunityDonation = 18,
        [Description("Environmental Cause")]
        EnvironmentalCause = 19,
        [Description("Building Infrastructure Support")]
        BuildingInfrastructureSupport = 20

    }
}
