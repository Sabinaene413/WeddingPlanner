using WeddingPlanner.Api.Infrastructure.Auth;
using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Services.Permissions
{
    public interface IWeddingPermissionService
    {
        bool CanCreateWedding(CurrentUser user, bool isSelfManaged);
        bool CanEditWedding(CurrentUser user, Wedding wedding);
        bool CanCompleteWedding(CurrentUser user, Wedding wedding);
        bool CanArchiveWedding(CurrentUser user);
        bool CanDeleteWedding(CurrentUser user);

        bool CanManageTasks(CurrentUser user, Wedding wedding);
        bool CanToggleTask(CurrentUser user);
    }
}
