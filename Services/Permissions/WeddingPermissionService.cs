using WeddingPlanner.Api.Infrastructure.Auth;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Services.Permissions
{
    public class WeddingPermissionService : IWeddingPermissionService
    {
        public bool CanCreateWedding(CurrentUser user, bool isSelfManaged)
        {
            if (user.Role == UserRole.Admin) return true;
            if (user.Role == UserRole.Organizer) return true;

            if (user.Role == UserRole.BrideGroom && isSelfManaged)
                return true;

            return false;
        }

        public bool CanEditWedding(CurrentUser user, Wedding wedding)
        {
            if (user.Role == UserRole.Admin) return true;
            if (user.Role == UserRole.Organizer) return true;

            if (user.Role == UserRole.BrideGroom &&
                wedding.OwnerId == user.Id &&
                wedding.IsSelfManaged)
                return true;

            return false;
        }

        public bool CanCompleteWedding(CurrentUser user, Wedding wedding)
        {
            return CanEditWedding(user, wedding);
        }

        public bool CanArchiveWedding(CurrentUser user)
        {
            return user.Role == UserRole.Admin ||
                   user.Role == UserRole.Organizer;
        }

        public bool CanDeleteWedding(CurrentUser user)
        {
            return user.Role == UserRole.Admin;
        }

        public bool CanManageTasks(CurrentUser user, Wedding wedding)
        {
            if (user.Role == UserRole.Admin) return true;
            if (user.Role == UserRole.Organizer) return true;

            if (user.Role == UserRole.BrideGroom &&
                wedding.OwnerId == user.Id &&
                wedding.IsSelfManaged)
                return true;

            return false;
        }

        public bool CanToggleTask(CurrentUser user)
        {
            return user.Role == UserRole.Admin ||
                   user.Role == UserRole.Organizer ||
                   user.Role == UserRole.BrideGroom;
        }
    }
}
