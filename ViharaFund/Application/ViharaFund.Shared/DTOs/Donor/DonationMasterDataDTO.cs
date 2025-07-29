using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.Donor
{
    public class DonationMasterDataDTO
    {
        public List<DropDownDTO> Campaigns { get; set; } = new List<DropDownDTO>();
    }
}
