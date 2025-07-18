using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.Donor
{
    public class DonationMasterDataDTO
    {
        public List<DropDownDto> DonorPurpose { get; set; } = new List<DropDownDto>();
    }
}
