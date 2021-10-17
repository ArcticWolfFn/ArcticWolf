using ArcticWolfApi.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Services
{
    public interface IProfileService
    {
        Profile GenerateAthenaProfile(
          string id,
          int seasonNumber,
          string profileId = "athena");

        Profile GenerateCommonCoreProfile(
          string id,
          string profileId = "common_core");

        Profile GenerateBlankProfile(string id, string profileId);
    }
}
