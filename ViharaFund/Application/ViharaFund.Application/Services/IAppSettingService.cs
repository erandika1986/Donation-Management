using ViharaFund.Application.DTOs.Common;
using ViharaFund.Shared.DTOs.CompanySettings;

namespace ViharaFund.Application.Services
{
    public interface IAppSettingService
    {
        Task<CompanyDetailDTO> GetCompanyDetail();
        Task<ResultDto> SaveCompanyDetailAsync(CompanyDetailDTO companyDetail);
        Task<ResultDto> SaveCompanySMTPSettingAsync(CompanySMTPSettingDTO companySMTPSetting);
        Task<CompanySMTPSettingDTO> GetCompanySMTPSetting();
    }
}
