using ArcticWolfApi.Models.Profile;

namespace ArcticWolfApi.Services
{
    public interface IProfileService
    {
        Profile GenerateAthenaProfile(string id, int seasonNumber, string profileId = "athena");

        Profile GenerateCommonCoreProfile(string id, string profileId = "common_core");

        Profile GenerateBlankProfile(string id, string profileId);
    }
}
